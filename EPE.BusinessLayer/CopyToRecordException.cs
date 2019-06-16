using System;
using System.Runtime.Serialization;

namespace EPE.BusinessLayer
{
    [Serializable]
    public class CopyToRecordException : Exception
    {
        public CopyToRecordException()
        {
        }

        public CopyToRecordException(string entityType, string columnType)
            : base("Cannot automatically copy to Record a column of type '" + columnType + "' from the entity class '" + entityType + "'. Consider overriding CopyEntityColumnToRecord method.")
        {
        }

        protected CopyToRecordException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
