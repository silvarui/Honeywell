using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPE.DataAccess
{
    [Serializable()]
    public class Parameter : DataElement
    {
        #region .ctor

        public Parameter(string parameterName, Object value, SqlDbType sqlDbType, int size)
            : base(parameterName, value)
        {
            SqlDbType = sqlDbType;
            Size = size;
            Direction = ParameterDirection.Input;
        }

        public Parameter(string parameterName, Object value, SqlDbType sqlDbType)
            : this(parameterName, value, sqlDbType, 0)
        {
        }

        public Parameter(string parameterName, SqlDbType sqlDbType)
            : this(parameterName, null, sqlDbType, 0)
        {
        }

        #endregion .ctor


        #region Properties

        /// <summary>
        /// Gets or sets the SqlDbType of the parameter.
        /// </summary>
        public SqlDbType SqlDbType { get; set; }

        /// <summary>
        /// Gets or sets the maximum size of the parameter.
        /// </summary>
        /// <remarks>
        /// For character strings (char, varchar) and Unicode character strings (nchar, nvarchar) the size specified is in characters.
        /// For binary data types of either fixed length (binary) or variable length (varbinary) the size refers to the number of bytes.
        /// For fixed length data types (bit, tinyint, smallint, int, bigint, smallmoney, money, numeric, decimal, date, time, smalldatetime, datetime, datetime2, datetimeoffset) the value of Size is ignored. For Xml type the size is ignored.
        /// If not explicitly set, the size is inferred from the actual size of the specified parameter value.
        /// </remarks>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the parameter is input-only, output-only, bidirectional, or a stored procedure return value parameter. The default is Input.
        /// </summary>
        public ParameterDirection Direction { get; set; }

        /// <summary>
        /// Gets or sets the type name for a table-valued parameter.
        /// </summary>
        /// <example>
        /// tvpParam.SqlDbType = SqlDbType.Structured;
        /// tvpParam.TypeName = "dbo.CategoryTableType";
        /// </example>
        public string TypeName { get; set; }

        #endregion Properties


        #region Equality members

        protected override bool Equals(DataElement other)
        {
            var otherParameter = (Parameter)other;
            return base.Equals(other) && SqlDbType == otherParameter.SqlDbType && Size == otherParameter.Size && string.Equals(TypeName, otherParameter.TypeName);
        }

        /// <summary>
        /// Hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)SqlDbType;
                hashCode = (hashCode * 397) ^ Size;
                hashCode = (hashCode * 397) ^ (TypeName == null ? 0 : TypeName.GetHashCode());
                return hashCode;
            }
        }

        #endregion Equality members
    }
}
