using System;
using System.Collections.Generic;
using System.Data;

namespace EPE.DataAccess
{
    [Serializable]
    public class Parameters : DataElementList
    {
        public Parameters()
        {
        }

        /// <summary>
        /// This constructor is obsolete. Use Parameters(IEnumerable<Parameter> collection) instead.
        /// </summary>
        /// <param name="collection"></param>
        public Parameters(IEnumerable<DataElement> collection) : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameters"/> class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection"></param>
        public Parameters(IEnumerable<Parameter> collection) : base(collection)
        {
        }

        /// <summary>
        /// This method is obsolete. Use Add(string parameterName, object value, SqlDbType sqlDbType) instead.
        /// </summary>
        /// <param name="elemName"></param>
        /// <param name="elemValue"></param>
        public void Add(string elemName, object elemValue)
        {
            base.Add(new DataElement(elemName, elemValue));
        }

        /// <summary>
        /// Adds a new <see cref="Parameter"/> object to the list.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="sqlDbType">The <see cref="SqlDbType"/> of the parameter.</param>
        public void Add(string parameterName, object value, SqlDbType sqlDbType)
        {
            base.Add(new Parameter(parameterName, value, sqlDbType));
        }

        /// <summary>
        /// Update values of the current parameters by the corresponding values from the specified <see cref="Parameters"/> object.
        /// </summary>
        /// <param name="sourceParams">The object with the list of parameters to take values from.</param>
        public void UpdateValues(Parameters sourceParams)
        {
            Parameter param;
            foreach (DataElement dataElement in this)
            {
                param = dataElement as Parameter;
                if (param != null)
                {
                    DataElement newParam = sourceParams.Find(p => p.Name == param.Name);
                    if (newParam != null)
                    {
                        param.Value = newParam.Value;
                    }
                }
            }
        }

        public bool Contains(string name)
        {
            return Exists(p => p.Name == name);
        }
    }
}
