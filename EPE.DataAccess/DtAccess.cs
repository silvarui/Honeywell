using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace EPE.DataAccess
{
    public static class DtAccess
    {
        public const string XML_ROOT = "root";
        public const char PARAMETER_TOKEN = '@';
        public const string DEFAULT_DB_CSHEMA = "dbo.";

        /// <summary>
        /// Opens a connection, opens a transaction on the connection and returns the transaction.
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static Transaction BeginTrans(String connString)
        {
            return new Transaction(connString);
        }

        private static SqlCommand PrepareCommand(SqlConnection connection, string storedProcedure, IEnumerable<DataElement> parameters)
        {
            var command = new SqlCommand { CommandText = BuildStoredProcedureName(storedProcedure), CommandType = CommandType.StoredProcedure, Connection = connection };
            
            if (parameters != null)
            {
                foreach (DataElement dataElement in parameters)
                {
                    SqlParameter sqlParameter = command.Parameters.AddWithValue(BuildParameterName(dataElement.Name), dataElement.Value.SqlValue());
                    if (dataElement is Parameter)
                    {
                        Parameter param = (Parameter)dataElement;
                        sqlParameter.SqlDbType = param.SqlDbType;
                        sqlParameter.Size = param.Size;
                        sqlParameter.Direction = param.Direction;
                        sqlParameter.TypeName = param.TypeName;
                    }
                }
            }

            return command;
        }


        #region Query

        // Executes a query with an optional transactional scope.
        // Returns a recordset as List<Record>
        // Receives a stored procedure name and a list of parameters
        // Support several results from DB (SELECT 1; SELECT 2;).
        private static List<Record> ExecuteQuery(SqlConnection connection, string storedProcedure, Parameters parameters, bool fillTitlesOfFirstRecordsOnly)
        {
            var records = new List<Record>();
            byte result = 0;

            try
            {
                using (SqlCommand cmd = PrepareCommand(connection, storedProcedure, parameters))
                {
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        bool hasMoreResults = true;
                        do
                        {
                            bool firstRecordOfResult = true;
                            while (dataReader.Read())
                            {
                                var values = new List<object>();
                                for (int i = 0, fieldCount = dataReader.FieldCount; i < fieldCount; i++)
                                {
                                    values.Add(dataReader.GetValue<object>(i, null));
                                }
                                records.Add(new Record(fillTitlesOfFirstRecordsOnly && !firstRecordOfResult ? null : dataReader.GetNames(), values) { Result = result });
                                firstRecordOfResult = false;
                            }
                            result++;

                            try
                            {
                                hasMoreResults = dataReader.NextResult();
                            }
                            catch (SqlException)
                            {
                                hasMoreResults = false;
                            }

                        } while (hasMoreResults);
                    }
                    //If the Command contains output parameters or return values, they will not be available until the DataReader is closed.
                    //So it is important to close DataReader first before calling parameters.UpdateValues method
                    UpdateParameters(parameters, cmd.Parameters);
                    return records;
                }
            }
            catch (Exception ex)
            {
                throw CreateCannotExecProcedureException(storedProcedure, parameters, ex);
            }
        }

        // Executes a query within a transactional scope.
        // Returns a recordset as List<Record>
        // Receives a stored procedure name and a list of parameters
        public static List<Record> ExecuteQuery(SqlConnection connection, string storedProcedure, Parameters parameters)
        {
            return ExecuteQuery(connection, storedProcedure, parameters, false);
        }

        // Creates a connection, does the db work and closes the connection.
        // Returns a recordset as List<Record>
        // Receives a stored procedure name and a list of parameters
        public static List<Record> ExecuteQuery(string connString, string storedProcedure, Parameters parameters)
        {
            using (var sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();

                return ExecuteQuery(sqlConn, storedProcedure, parameters, false);
            }
        }

        public static List<Record> ExecuteQueryReturnSingleColumnNames(string connString, string storedProcedure, Parameters parameters)
        {
            using (var sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                return ExecuteQuery(sqlConn, storedProcedure, parameters, true);
            }
        }

        #endregion


        #region NonQuery

        private static int ExecuteNonQuery(SqlConnection connection, string storedProcedure, Parameters parameters)
        {
            try
            {
                using (SqlCommand cmd = PrepareCommand(connection, storedProcedure, parameters))
                {
                    int rowsNo = cmd.ExecuteNonQuery();
                    UpdateParameters(parameters, cmd.Parameters);
                    return rowsNo;
                }
            }
            catch (Exception ex)
            {
                throw CreateCannotExecProcedureException(storedProcedure, parameters, ex);
            }
        }

        // Creates a connection, does the db work and closes the connection.
        // Receives a stored procedure name and a list of parameters
        // Returns the number of affected rows.
        public static int ExecuteNonQuery(string connString, string storedProcedure, Parameters parameters)
        {
            using (var sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                return ExecuteNonQuery(sqlConn, storedProcedure, parameters);
            }
        }

        #endregion


        //throw message: 
        //Cannot execute: sp_Xxx prm1, prm2, prm3, ...
        private static Exception CreateCannotExecProcedureException(string storedProc, IEnumerable<DataElement> prms, Exception innerException)
        {
            string errMessage = "Cannot execute stored procedure: " + storedProc + "\r\n";
            bool first = true;
            if (prms != null)
            {
                foreach (DataElement prm in prms)
                {
                    if (!first)
                        errMessage += ", ";
                    string paramName = BuildParameterName(prm.Name);
                    string paramValue = "NULL";
                    if (prm.Value.SqlValue() != DBNull.Value)
                    {
                        if (prm.Value is string)
                            paramValue = "'" + prm.Value + "'";
                        else if (prm.Value is DateTime)
                            paramValue = "'" + prm.Value + "'";
                        else if (prm.Value is Guid)
                            paramValue = "'" + prm.Value + "'";
                        else
                            paramValue = prm.Value.ToString();
                    }
                    errMessage += paramName + "=" + paramValue;
                    first = false;
                }
            }
            errMessage += "\r\n";
            return new Exception(errMessage, innerException);
        }

        /// <summary>
        /// Builds a parameter name.
        /// </summary>
        /// <remarks>In SQL Server parameter names must begin with an at sign (@).</remarks>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>A correctly formatted parameter name.</returns>
        public static string BuildParameterName(string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            if (name[0] != PARAMETER_TOKEN)
            {
                return name.Insert(0, new string(PARAMETER_TOKEN, 1));
            }
            return name;
        }

        /// <summary>
        /// Builds a stored procedure name by concatenating the name with the default database schema dbo in case the schema is not provided.
        /// </summary>
        /// <remarks>
        /// When executing a user-defined procedure, it is recommended qualifying the procedure name with the schema name. 
        /// This practice gives a small performance boost because the Database Engine does not have to search multiple schemas. 
        /// It also prevents executing the wrong procedure if a database has procedures with the same name in multiple schemas.
        /// </remarks>
        /// <param name="name">The name of the stored procedure.</param>
        /// <returns>A correctly formatted stored procedure name.</returns>
        public static string BuildStoredProcedureName(string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            if (!name.Contains("."))
            {
                return name.Insert(0, DEFAULT_DB_CSHEMA);
            }
            return name;
        }

        /// <summary>
        /// If <see cref="SqlParameterCollection"/> collection contains output parameters or return values, they will be copied to the values of the corresponding <see cref="Parameter"/> object.
        /// Important: in case DataReader is used, output parameters and return values will not be available until the DataReader is closed. This applies only to DataReader and does not apply to XmlReader.
        /// </summary>
        /// <param name="parameters">The parameters to update.</param>
        /// <param name="sqlParameters">Collection of SQL parameters to take values from.</param>
        private static void UpdateParameters(Parameters parameters, SqlParameterCollection sqlParameters)
        {
            if (parameters == null)
                return;
            Parameter param;
            foreach (DataElement parameter in parameters)
            {
                param = parameter as Parameter;
                if ((param != null) && (param.Direction != ParameterDirection.Input))
                {
                    param.Value = sqlParameters[BuildParameterName(param.Name)].Value;
                }
            }
        }
    }
}
