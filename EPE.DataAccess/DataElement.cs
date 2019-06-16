using System;

namespace EPE.DataAccess
{
    [Serializable()]
    public class DataElement
    {
        #region Fields

        private readonly string elementName;

        // Value is of kept as Object reference to the one of the System types below:
        // int; for integer data
        // double; for floating point data
        // boolean; for boolean data
        // char; for single character data
        // string; for string data
        // byte[]; for binary data
        // Guid; for unique identifier data
        private Object elementValue;

        #endregion Fields


        #region .ctor

        protected DataElement()
        {
        }

        /// <summary>
        /// Do not use DataElement as a parameter. Use <see ref="Parameter"/> instead.
        /// </summary>
        /// <param name="elemName">Name of the column.</param>
        /// <param name="elemValue">Value of the column.</param>
        public DataElement(string elemName, Object elemValue)
        {
            elementName = elemName;
            elementValue = elemValue;
        }

        #endregion .ctor


        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public virtual string Name
        {
            get { return elementName; }
        }

        /// <summary>
        /// Gets or sets the value of the column
        /// </summary>
        public virtual Object Value
        {
            get { return elementValue; }
            set { elementValue = value; }
        }

        #region Methods

        /// <summary>
        /// Gets the value of the column as a <see cref="Nullable{T}"/>. Return null if the value is of type <see cref="DBNull"/>.
        /// </summary>
        /// <typeparam name="T">The underlying value type of the <see cref="Nullable{T}"/> generic type.</typeparam>
        /// <returns>The <see cref="Nullable{T}"/> equivalent of the <see cref="DataElement.Value"/>.</returns>
        public T? GetNullable<T>() where T : struct
        {
            return Convert.IsDBNull(Value) ? null : (T?)Value;
        }

        /// <summary>
        /// Gets the value of the column as a string. Return null if the value is of type <see cref="DBNull"/>.
        /// </summary>
        /// <returns>The string representation of the <see cref="DataElement.Value"/>.</returns>
        public string GetString()
        {
            return Convert.IsDBNull(Value) ? null : (string)Value;
        }

        #endregion Methods


        #region Equality members

        protected virtual bool Equals(DataElement other)
        {
            return string.Equals(elementName, other.elementName) && Equals(elementValue, other.elementValue);
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
            return Equals((DataElement)obj);
        }

        /// <summary>
        /// Hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return (elementName == null ? 0 : elementName.GetHashCode());
        }

        #endregion Equality members
    }
}
