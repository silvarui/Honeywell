using System;
using System.Runtime.Serialization;

namespace EPE.BusinessLayer
{
    [Serializable]
    public class InvalidPropertyException : Exception
    {
        public InvalidPropertyException()
        {
        }

        public InvalidPropertyException(string entityType, string columnName)
            : base("Cannot find the property '" + columnName + "' in the entity class '" + entityType + "'. Check the 'get' stored procedures and the entity column names.")
        {
        }

        protected InvalidPropertyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
