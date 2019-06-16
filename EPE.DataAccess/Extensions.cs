using System;
using System.Collections.Generic;
using System.Data;

namespace EPE.DataAccess
{
    public static class Extensions
    {
        /// <summary>
        /// Populates a list of strings with the column names of the current record.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns>The list of strings with the column names of the current record.</returns>
        public static List<string> GetNames(this IDataReader dataReader)
        {
            var columnNames = new List<string>();
            for (int i = 0, fieldCount = dataReader.FieldCount; i < fieldCount; i++)
            {
                columnNames.Add(dataReader.GetName(i));
            }
            return columnNames;
        }

        /// <summary>
        /// Gets the value of the column with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of the returned value.</typeparam>
        /// <param name="dataRecord"></param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultValue">The value which will be returned in case the column is set to null.</param>
        /// <returns>The value of the specified column if it is not null, otherwise the specified default value.</returns>
        public static T GetValue<T>(this IDataRecord dataRecord, string name, T defaultValue)
        {
            return dataRecord.IsDBNull(dataRecord.GetOrdinal(name)) ? defaultValue : ((T)dataRecord[name]);
        }

        /// <summary>
        /// Gets the value of the column at the specified index.
        /// </summary>
        /// <typeparam name="T">The type of the returned value.</typeparam>
        /// <param name="dataRecord"></param>
        /// <param name="index">The zero-based index of the column to find.</param>
        /// <param name="defaultValue">The value which will be returned in case the column is set to null.</param>
        /// <returns>The value of the specified column if it is not null, otherwise the specified default value.</returns>
        public static T GetValue<T>(this IDataRecord dataRecord, int index, T defaultValue)
        {
            return dataRecord.IsDBNull(index) ? defaultValue : ((T)dataRecord[index]);
        }

        /// <summary>
        /// Gets the value of the object if it is not set to null, otherwise DBNull.Value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The value of the object if it is not set to null, otherwise DBNull.Value.</returns>
        public static object SqlValue(this object value)
        {
            return value ?? DBNull.Value;
        }
    }
}
