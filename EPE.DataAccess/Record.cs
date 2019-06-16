using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EPE.DataAccess
{
    [Serializable]
    public class Record : IEnumerable<DataElement>, ISerializable
    {
        public Record()
        {
            ColumnNames = new List<string>();
            Values = new List<object>();
        }

        public Record(List<string> columnNames, List<object> values)
        {
            ColumnNames = columnNames;
            Values = values;

            //CheckValidity();
        }

        public List<string> ColumnNames { get; set; }

        public List<object> Values { get; set; }

        /// <summary>
        /// Gets or sets the number of the result which the Record belongs to. The default value is 0 which means that the Record belongs to the first result.
        /// If SP returns several results (SELECT 1; SELECT 2;), then the Record corresponding to the second result should have value 1 and so on.
        /// </summary>
        public byte Result { get; internal set; }

        private void CheckValidity()
        {
            if (ColumnNames == null)
                throw new Exception("Record has no column names.");
            if (Values == null)
                throw new Exception("Record has no values.");
            if (ColumnNames.Count != Values.Count)
                throw new Exception("Record has different number of column names than values.");
        }

        public DataElement this[int index]
        {
            get
            {
                CheckValidity();

                if (index >= ColumnNames.Count)
                    throw new ArgumentOutOfRangeException("index");

                return new RecordDataElement(this, index);
            }
        }

        public DataElement this[string columnName]
        {
            get
            {
                CheckValidity();

                int index = ColumnNames.IndexOf(columnName);
                if (index < 0)
                    return null;

                return new RecordDataElement(this, index);
            }
        }

        public void Add(DataElement dataElement)
        {
            CheckValidity();

            if (dataElement == null)
                throw new ArgumentNullException("dataElement");
            if (dataElement.Name == null)
                throw new ArgumentException("Record.Add: dataElement parameter cannot have Name property set to null.");
            if (ColumnNames.Contains(dataElement.Name))
                throw new ArgumentException(string.Format("Record.Add: column {0} already exists.", dataElement.Name));

            ColumnNames.Add(dataElement.Name);
            Values.Add(dataElement.Value);
        }

        public void Add(string name, object value)
        {
            Add(new DataElement(name, value));
        }

        public void AddRange(IEnumerable<DataElement> dataElements)
        {
            CheckValidity();

            if (dataElements == null)
                throw new ArgumentNullException("dataElements");

            foreach (DataElement dataElement in dataElements)
            {
                if (dataElement.Name == null)
                    throw new ArgumentException("Record.AddRange: dataElements cannot have Name property set to null.");

                ColumnNames.Add(dataElement.Name);
                Values.Add(dataElement.Value);
            }
        }

        public void RemoveRange(int index, int count)
        {
            CheckValidity();

            ColumnNames.RemoveRange(index, count);
            Values.RemoveRange(index, count);
        }

        public IEnumerator<DataElement> GetEnumerator()
        {
            CheckValidity();

            for (int index = 0; index < ColumnNames.Count; index++)
                yield return new RecordDataElement(this, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///   Class used to pretend DataElement behaviour. 
        ///   Created for backward compatibility of the Record class.
        /// </summary>
        [Serializable]
        private class RecordDataElement : DataElement
        {
            public RecordDataElement(Record record, int index)
            {
                if (record == null)
                    throw new ArgumentNullException("record");
                if (index < 0)
                    throw new ArgumentException("Parameter index cannot be less than 0.");
                if (index >= record.ColumnNames.Count)
                    throw new ArgumentException("Parameter index is out of range.");
                record.CheckValidity();

                _record = record;
                _index = index;
            }

            private readonly Record _record;

            private Record Record
            {
                get { return _record; }
            }

            private readonly int _index;

            private int Index
            {
                get { return _index; }
            }

            public override string Name
            {
                get { return Record.ColumnNames[Index]; }
            }

            public override object Value
            {
                get { return Record.Values[Index]; }
                set { Record.Values[Index] = value; }
            }
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializationWriter sw = SerializationWriter.GetWriter();
            sw.Write<string>(this.ColumnNames ?? new List<string>());
            sw.Write<object>(this.Values);
            if (this.Result != 0)
                sw.Write(Result);
            sw.AddToInfo(info);
        }

        public Record(SerializationInfo info, StreamingContext context)
        {
            SerializationReader sr = SerializationReader.GetReader(info);
            this.ColumnNames = new List<string>(sr.ReadList<string>());
            this.Values = new List<object>(sr.ReadList<object>());
            if (sr.BaseStream.Position != sr.BaseStream.Length)
                Result = sr.ReadByte();
        }

        #endregion
    }
}
