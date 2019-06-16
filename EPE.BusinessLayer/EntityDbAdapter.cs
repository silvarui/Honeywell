using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EPE.DataAccess;

namespace EPE.BusinessLayer
{
    public abstract class EntityDbAdapter<T> 
        where T : Entity, new()
    {
        private T entityToUpdate = null;

        protected string ConnectionString { get; }

        public EntityDbAdapter(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected T LoadSingle(string storedProcedure, Parameters parameters)
        {
            List<Record> recordSet = null;

            recordSet = DtAccess.ExecuteQuery(ConnectionString, storedProcedure, parameters);

            if (recordSet.Count > 0)
            {
                if (recordSet.Exists(r => r.Result > 0))
                    return GetEntityFromRecords(recordSet);

                return FromRecord(recordSet[0]);
            }
            return null;
        }

        protected void LoadList(ref List<T> targetList, string storedProcedure, Parameters parameters)
        {
            List<Record> recordSet = null;

            recordSet = DtAccess.ExecuteQuery(ConnectionString, storedProcedure, parameters);

            targetList.AddRange(recordSet.Exists(r => r.Result > 0) ? GetEntitiesFromRecords(recordSet) : recordSet.Select(FromRecord));
        }

        protected virtual List<T> GetEntitiesFromRecords(List<Record> records)
        {
            throw new NotImplementedException();
        }

        protected virtual T GetEntityFromRecords(List<Record> records)
        {
            throw new NotImplementedException();
        }

        protected virtual void StoreSingle(ref T sourceEntity, string storedProcedure)
        {
            this.StoreSingle(ref sourceEntity, storedProcedure, null);
        }

        protected virtual void StoreSingle(ref T sourceEntity, string storedProcedure, Parameters additionalParameters)
        {
            Parameters parameters = new Parameters();
            parameters.AddRange(ToRecord(sourceEntity));
            if (additionalParameters != null)
                parameters.AddRange(additionalParameters);

            Debug.Assert(this.entityToUpdate == null);
            this.entityToUpdate = sourceEntity;
            try
            {
                sourceEntity = this.LoadSingle(storedProcedure, parameters);
            }
            finally
            {
                this.entityToUpdate = null;
            }
        }

        protected void StoreList(List<T> sourceList, string storedProcedure)
        {
            foreach (T entity in sourceList)
            {
                Parameters completeParams = new Parameters();
                Record entityRecord = ToRecord(entity);
                completeParams.AddRange(entityRecord);

                DtAccess.ExecuteNonQuery(ConnectionString, storedProcedure, completeParams);
            }
        }

        protected virtual Record ToRecord(T entity)
        {
            Record entityRecord = new Record();
            string[] columnNames = entity.GetColumnNames();
            foreach (string columnName in columnNames)
                CopyEntityColumnToRecord(columnName, entity, ref entityRecord);
            return entityRecord;
        }

        protected virtual T FromRecord(Record rec)
        {
            T entity = this.entityToUpdate ?? new T();
            foreach (DataElement column in rec)
                CopyEntityColumnFromRecord(column.Name, ref entity, rec);
            return entity;
        }

        protected virtual void CopyEntityColumnToRecord(string columnName, T entity, ref Record record)
        {
            //autopopulates a record
            //for this to work you must make sure that the parameter name required by the stored procedure is identical to the column name 
            //defined in the enum member in each entity class            
            object val = entity[columnName];
            //if (val != null)
            //{
            //    if (val is SimpleEntity) //if the column type is SimpleEntity, take the code
            //        val = ((SimpleEntity)val).Code;
            //    else if (val is Identifier)
            //        val = val.ToString();
            //    else if (val is PIdEntity)
            //    {
            //        PIdEntity pidEntity = (PIdEntity)val;
            //        if (pidEntity.PId.HasValue) val = pidEntity.PId.Value;
            //        else val = null;
            //    }
            //    else if (val is Entity) //if the column type is other Entity (it's not one of the basic types), throw an exception -> we don't know how to map it
            //        throw new CopyToRecordException(entity.GetType().Name, val.GetType().Name);
            //    else if (val is DateTime)
            //        val = DateTimeConverter.DateTimeToDbString((DateTime)val);
            //}
            record.Add(new DataElement(columnName, val));
        }

        protected virtual void CopyEntityColumnFromRecord(string columnName, ref T entity, Record record)
        {
            //autopopulates from record
            //for this to work you must make sure that the column name returned by the stored procedure is identical to the column name 
            //defined in the enum member in each entity class
            DataElement column = record[columnName];
            entity[column.Name] = column.Value;
        }
    }
}
