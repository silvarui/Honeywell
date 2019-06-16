using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Linq;

namespace EPE.BusinessLayer
{
    public class NumberOfRowsEventArgs : EventArgs
    {
        public int NumberOfRows { get; set; }
    }

    public abstract class FileAdapterBase
    {
        public event EventHandler<NumberOfRowsEventArgs> NumberOfRowsToImportDetermined;

        public event EventHandler RowTreated;

        public event EventHandler DataImported;

        protected string filePath;

        protected FileAdapterBase(string filePath)
        {
            this.filePath = filePath;
        }

        public abstract void LoadData();

        protected virtual void OnRowTreated(EventArgs e)
        {
            RowTreated?.Invoke(this, e);
        }

        protected virtual void OnDataImported(EventArgs e)
        {
            DataImported?.Invoke(this, e);
        }

        protected virtual void OnNumberOfRowsToImportDetermined(NumberOfRowsEventArgs e)
        {
            NumberOfRowsToImportDetermined?.Invoke(this, e);
        }
    }

    public abstract class ExcelFileAdapter : FileAdapterBase
    {
        protected ExcelFileAdapter(string filePath)
        : base(filePath)
        {
        }

        protected abstract List<string> GetColumnNames();

        protected abstract void ImportRows(DataRowCollection dataRows);

        private string GetConnectionString()
        {
            var fileExtension = Path.GetExtension(filePath);

            switch (fileExtension)
            {
                case ".xls":
                    return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";" + "Extended Properties='Excel 8.0;HDR=YES;IMEX=1;'";

                case ".xlsx":
                    return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;'";

                default:
                    throw new NotSupportedException();
            }
        }

        public override void LoadData()
        {
            using (var conn = new OleDbConnection())
            {
                conn.ConnectionString = GetConnectionString();

                using (var comm = new OleDbCommand())
                {
                    try
                    {
                        conn.Open();

                        var tableSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (tableSheet != null)
                        {
                            var sheetName = tableSheet.Rows[0][Constants.TABLE_NAME].ToString();

                            comm.CommandText = string.Format(SelectQuery + "FROM [{0}]", sheetName);

                            comm.Connection = conn;

                            var da = new OleDbDataAdapter(comm);
                            var ds = new DataSet();

                            da.Fill(ds);

                            ImportRows(ds.Tables[0].Rows);

                            OnDataImported(EventArgs.Empty);
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        private string SelectQuery
        {
            get
            {
                var sb = new StringBuilder();

                foreach (var str in GetColumnNames())
                {
                    sb.Append("[" + str + "],");
                }

                return string.Format("SELECT {0} ", sb.ToString().Remove(sb.Length - 1, 1));
            }
        }
    }

    public class AlunoFileAdapter : ExcelFileAdapter
    {
        private string ConnectionString { get; }

        public AlunoFileAdapter(string filePath, string connectionString)
            : base(filePath)
        {
            ConnectionString = connectionString;
        }

        protected override List<string> GetColumnNames()
        {
            var columns = new List<string>();

            columns.Add(Constants.colBoletim);
            columns.Add(Constants.colUsername);
            columns.Add(Constants.colNome);
            columns.Add(Constants.colDataNasc);
            columns.Add(Constants.colEscola);
            columns.Add(Constants.colProfessor);
            columns.Add(Constants.colEncEdu);
            columns.Add(Constants.colMorada);
            columns.Add(Constants.colCPostal);
            columns.Add(Constants.colLocalidade);
            columns.Add(Constants.colCantao);
            columns.Add(Constants.colTelefone);
            columns.Add(Constants.colTelemovel);
            columns.Add(Constants.colEmail);

            return columns;
        }

        private static void CopyColumnFromFile(ref Aluno aluno, string columnName, DataRow cellValue)
        {
            var cellValueAsString = cellValue[columnName].ToString().Trim();

            if (string.IsNullOrEmpty(cellValueAsString))
                return;

            switch (columnName)
            {
                case Constants.colUsername:
                    if (cellValueAsString.ToUpper().StartsWith("EPE"))
                    {
                        aluno.Username = cellValueAsString;
                    }
                    else if (!string.IsNullOrEmpty(cellValueAsString) && int.TryParse(cellValueAsString, out var username))
                    {
                        aluno.Username = string.Format("EPE{0}", cellValueAsString);
                    }
                    break;

                case Constants.colNome:
                    aluno.Nome = cellValueAsString;
                    break;

                case Constants.colDataNasc:
                    aluno.DtNasc = Convert.ToDateTime(cellValueAsString);
                    break;

                case Constants.colEscola:
                    aluno.Escola = cellValueAsString;
                    break;

                case Constants.colProfessor:
                    aluno.Professor = cellValueAsString;
                    break;

                case Constants.colEncEdu:
                    aluno.EncEduc = cellValueAsString;
                    break;

                case Constants.colMorada:
                    aluno.Morada = cellValueAsString;
                    break;

                case Constants.colCPostal:
                    aluno.CPostal = Convert.ToDouble(cellValueAsString);
                    break;

                case Constants.colLocalidade:
                    aluno.Localidade = cellValueAsString;
                    break;

                case Constants.colCantao:
                    aluno.Cantao = cellValueAsString;
                    break;

                case Constants.colTelefone:
                    aluno.Telefone = cellValueAsString;
                    break;

                case Constants.colTelemovel:
                    aluno.Telemovel = cellValueAsString;
                    break;

                case Constants.colEmail:
                    aluno.Email = cellValueAsString;
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        private static void CopyColumnFromFile(ref Boletim boletim, string columnName, DataRow cellValue)
        {
            var cellValueAsString = cellValue[columnName].ToString().Trim();

            if (string.IsNullOrEmpty(cellValueAsString) || !string.IsNullOrEmpty(cellValueAsString) && !int.TryParse(cellValueAsString, out var numBoletim))
                return;

            switch (columnName)
            {
                case Constants.colBoletim:
                    boletim.NumBoletim = cellValueAsString;
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        protected override void ImportRows(DataRowCollection dataRows)
        {
            var alunos = new List<Aluno>();
            var boletins = new List<Boletim>();

            var alunoAdapter = new AlunoAdapter(ConnectionString);
            var boletimAdapter = new BoletimAdapter(ConnectionString);

            var existentAlunos = alunoAdapter.GetAlunos();
            var existentBoletins = boletimAdapter.GetBoletins();

            OnNumberOfRowsToImportDetermined(new NumberOfRowsEventArgs { NumberOfRows = dataRows.Count * 2 + 2 });

            foreach (DataRow dataRow in dataRows)
            {
                var aluno = new Aluno();
                var boletim = new Boletim();

                foreach (var columnName in GetColumnNames())
                {
                    if (columnName == Constants.colBoletim)
                        CopyColumnFromFile(ref boletim, columnName, dataRow);
                    else
                        CopyColumnFromFile(ref aluno, columnName, dataRow);
                }

                if (existentAlunos.Count > 0)
                {
                    var existentAluno = existentAlunos.FirstOrDefault(a => a.Username == aluno.Username && a.Nome == aluno.Nome);

                    if (existentAluno != null)
                        aluno.IdAluno = existentAluno.IdAluno;
                }

                alunos.Add(aluno);

                boletim.Aluno = aluno;

                OnRowTreated(EventArgs.Empty);

                if (string.IsNullOrEmpty(boletim.NumBoletim) || existentBoletins.Exists(b => b.NumBoletim == boletim.NumBoletim))
                {
                    OnRowTreated(EventArgs.Empty);

                    continue;
                }

                boletins.Add(boletim);

                OnRowTreated(EventArgs.Empty);
            }

            alunoAdapter.StoreAlunos(alunos);

            OnRowTreated(EventArgs.Empty);

            var distinctBoletins = boletins.GroupBy(b => new { b.Aluno.IdAluno, b.NumBoletim }).Select(g => g.First()).ToList();

            boletimAdapter.StoreBoletins(distinctBoletins);

            OnRowTreated(EventArgs.Empty);
        }
    }
}
