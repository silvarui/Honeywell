using System;
using System.Runtime.Serialization;

namespace EPE.BusinessLayer
{
    [Serializable]
    public class AutoConversionException : Exception
    {
        public AutoConversionException()
        {
        }

        public AutoConversionException(string entityType, string property, string propertyType, object source)
            : base("Cannot automatically convert (" + source.GetType().Name + ")" + source.ToString() + " to (" + propertyType + ")" + entityType + "." + property)
        {
        }

        protected AutoConversionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}
