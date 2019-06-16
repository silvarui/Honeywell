using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPE.DataAccess
{
    [Serializable()]
    public abstract class DataElementList : List<DataElement>
    {
        #region .ctor

        protected DataElementList()
        {
        }

        protected DataElementList(IEnumerable<DataElement> collection)
            : base(collection)
        {
        }

        #endregion .ctor


        // Indexer by name
        public DataElement this[string elementName]
        {
            get { return Find(dataElement => dataElement.Name == elementName); }
            set
            {
                int index = FindIndex(dataElement => dataElement.Name == elementName);
                if (index == -1)
                    Add(value);
                else
                    base[index] = value;
            }
        }


        #region Equality members

        protected virtual bool Equals(DataElementList other)
        {
            if (this.Count != other.Count)
                return false;

            for (int i = 0; i < this.Count; i++)
            {
                if (!this[i].Equals(other[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DataElementList)obj);
        }

        /// <summary>
        /// Hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 0;
                foreach (DataElement element in this)
                {
                    hashCode = (hashCode * 7) ^ element.GetHashCode();
                }
                return hashCode;
            }
        }

        #endregion Equality members
    }
}
