using System;
using System.Reflection;
using System.Xml.Serialization;

namespace EPE.BusinessLayer
{
    public abstract class Entity : IExportEntity
    {
        private const BindingFlags C_BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        public const string StringTrue = "1";
        public const string StringFalse = "0";
        public const string StringYes = "Y";
        public const string StringNo = "N";

        public abstract string[] GetColumnNames();

        public virtual string[] GetColumnNamesForExport()
        {
            return GetColumnNames();
        }

        public virtual string[] GetGridViewColumns()
        {
            return null;
        }

        public virtual string GetColumnType(string columnName)
        {
            return string.Empty;
        }

        public virtual object this[string columnName]
        {
            get
            {
                PropertyInfo getProperty = GetType().GetProperty(columnName, C_BINDING_FLAGS); //find the property that matches the column name
                if (getProperty != null)
                    return getProperty.GetValue(this, null);
                else
                    throw new InvalidPropertyException(GetType().Name, columnName);
            }
            set
            {
                PropertyInfo setProperty = GetType().GetProperty(columnName, C_BINDING_FLAGS); //find the property that matches the column name
                if (setProperty != null)
                {
                    if (value != null)
                    {
                        if (value.GetType() == setProperty.PropertyType) //if the property type is the value type
                        {
                            setProperty.SetValue(this, value, null);
                        }
                        else if (value is string && (((string)value == StringTrue) || (((string)value).ToUpper() == StringYes) || ((string)value == StringFalse) || (((string)value).ToUpper() == StringNo)) && (setProperty.PropertyType.Equals(typeof(bool)) || setProperty.PropertyType.Equals(typeof(bool?)))) //if the property is bool while the value is "1" or "Y" strings
                        {
                            bool boolValue = (((string)value == StringTrue) || (((string)value).ToUpper() == StringYes));
                            setProperty.SetValue(this, boolValue, null);
                        }
                        else if (setProperty.PropertyType.Equals(typeof(string))) //if the property type is string, convert the value to string
                        {
                            setProperty.SetValue(this, value.ToString(), null);
                        }
                        else //try to convert the value, if it cannot be automatically converted, throw and exception. 
                        {
                            try
                            {
                                Type targetType = setProperty.PropertyType;
                                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))) //nullable type?
                                    targetType = Nullable.GetUnderlyingType(targetType);
                                object convertedValue = null;
                                if (targetType.BaseType == typeof(Enum))
                                    convertedValue = Enum.ToObject(targetType, value); //convert a value to an enum type
                                //else if (value is string && targetType.Equals(typeof(DateTime))) //if the value is string and the property type is DateTime, automatically convert string to DateTime using the harcoded db format
                                //    convertedValue = DateTimeConverter.DbStringToDateTime((string)value);
                                else if (value is string && targetType == typeof(Guid))
                                    convertedValue = Guid.Parse((string)value);
                                else
                                    convertedValue = Convert.ChangeType(value, targetType); //convert a value to a type

                                setProperty.SetValue(this, convertedValue, null);
                            }
                            catch
                            {
                                throw new AutoConversionException(GetType().Name, setProperty.Name, setProperty.PropertyType.Name, value);
                            }
                        }
                        //********this code must also handle the case when the propery is datetime and the value is string
                    }
                    else
                        setProperty.SetValue(this, null, null);
                }
                else
                    throw (new InvalidPropertyException(GetType().Name, columnName));
            }
        }

        public virtual object GetEntityKey()
        {
            return this;
        }

        public virtual object GetPLValue(string columnName)
        {
            object theField = this[columnName];
            if (theField is DateTime)
            {
                return ((DateTime)theField).ToString("dd-MM-yyyy");
            }
            else
                return theField; //the object itself
        }

        public virtual string GetPLTitle(string columnName)
        {
            return columnName;
        }

        #region UEID implementation
        private Guid uEId = Guid.NewGuid();
        /// <summary>
        /// Unique entity identifier. This identifier is generated during instanciation and coppied during
        /// cloning unless otherwise specified. It is preserved during a store but is not persisted in the database.
        /// <remarks>Unlike PID this identifier exists at all times even if the entity hasn't been stored.</remarks>
        /// </summary>
        [XmlIgnore]
        public Guid UEID { get { return this.uEId; } }

        /// <summary>
        /// This method coppies the UEID from the source entity, it should not be invoked from any class
        /// other than EntityDBAdapter.
        /// </summary>
        /// <param name="value"></param>
        [Obsolete("ATM there is no need for such a method, please discuss before removing this attribute", true)]
        internal void PreserveUEID(Entity originalEntity) { this.uEId = originalEntity.UEID; }
        #endregion
    }
}
