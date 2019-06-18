using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace EPE.BusinessLayer
{
    public class NumberOfRowsEventArgs : EventArgs
    {
        public int NumberOfRows { get; set; }
    }

    public abstract class FileAdapterBase
    {
	    protected string ConnectionString { get; }

		public event EventHandler<NumberOfRowsEventArgs> NumberOfRowsToImportDetermined;

        public event EventHandler RowTreated;

        public event EventHandler DataImported;

        protected string filePath;

        protected FileAdapterBase(string filePath, string connectionString)
        {
            this.filePath = filePath;
            ConnectionString = connectionString;
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
        protected ExcelFileAdapter(string filePath, string connectionString)
        : base(filePath, connectionString)
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
        public AlunoFileAdapter(string filePath, string connectionString)
            : base(filePath, connectionString)
        {
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

                boletim.IdAluno = aluno.IdAluno;

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

            var distinctBoletins = boletins.GroupBy(b => new { b.IdAluno, b.NumBoletim }).Select(g => g.First()).ToList();

            boletimAdapter.StoreBoletins(distinctBoletins);

            OnRowTreated(EventArgs.Empty);
        }
    }

	public class MovimentoExcelAdapter : ExcelFileAdapter
	{
		public MovimentoExcelAdapter(string filePath, string connectionString)
			: base(filePath, connectionString)
		{
		}

		protected override List<string> GetColumnNames()
		{
			var columns = new List<string>();

			columns.Add(Constants.colDateEval);
			columns.Add(Constants.colRelBancaire);
			columns.Add(Constants.colPortfeuille);
			columns.Add(Constants.colProduit);
			columns.Add(Constants.colIBAN);
			columns.Add(Constants.colMonn);
			columns.Add(Constants.colDateDu);
			columns.Add(Constants.colDateAu);
			columns.Add(Constants.colDescription);
			columns.Add(Constants.colDateTrans);
			columns.Add(Constants.colDateComptab);
			columns.Add(Constants.colDateValeur);
			columns.Add(Constants.colDescription1);
			columns.Add(Constants.colDescription2);
			columns.Add(Constants.colDescription3);
			columns.Add(Constants.colNoTransaction);
			columns.Add(Constants.colCoursDevises);
			columns.Add(Constants.colSousMontant);
			columns.Add(Constants.colDebit);
			columns.Add(Constants.colCredit);
			columns.Add(Constants.colSolde);

			return columns;
		}

		protected override void ImportRows(DataRowCollection dataRows)
		{
			var movimentos = new List<Movimento>();

			var movimentoAdapter = new MovimentoAdapter(ConnectionString);

			var existentMovimentos = movimentoAdapter.GetMovimentos();

			OnNumberOfRowsToImportDetermined(new NumberOfRowsEventArgs { NumberOfRows = dataRows.Count });

			foreach (DataRow dataRow in dataRows)
			{
				var movimento = new Movimento();

				foreach (var columnName in GetColumnNames())
				{
					CopyColumnFromFile(ref movimento, columnName, dataRow);
				}

				OnRowTreated(EventArgs.Empty);

				if (string.IsNullOrEmpty(movimento.IBAN) || !IsNewMovimento(existentMovimentos, movimento))
					continue;

				movimentos.Add(movimento);
			}

			movimentoAdapter.StoreMovimentos(movimentos);
		}

		private bool IsNewMovimento(List<Movimento> existentMovimentos, Movimento newMovimento)
		{
			if (existentMovimentos.Count == 0)
				return true;

			var movsToSearch = existentMovimentos
				.Where(m => m.DtValor == newMovimento.DtValor && m.DtContab == newMovimento.DtContab).ToList();

			var existent = movsToSearch.FirstOrDefault(m => m.Descricao1 == newMovimento.Descricao1 &&
			                                                m.Descricao2 == newMovimento.Descricao2 && m.Descricao3 == newMovimento.Descricao3 &&
			                                                (!m.SubTotal.HasValue && !newMovimento.SubTotal.HasValue ||
			                                                 m.SubTotal.HasValue && newMovimento.SubTotal.HasValue && m.SubTotal == newMovimento.SubTotal) &&
			                                                (!m.Debito.HasValue && !newMovimento.Debito.HasValue ||
			                                                 m.Debito.HasValue && newMovimento.Debito.HasValue && m.Debito == newMovimento.Debito) &&
			                                                (!m.Credito.HasValue && !newMovimento.Credito.HasValue ||
			                                                 m.Credito.HasValue && newMovimento.Credito.HasValue && m.Credito == newMovimento.Credito) &&
			                                                (!m.Saldo.HasValue && !newMovimento.Saldo.HasValue ||
			                                                 m.Saldo.HasValue && newMovimento.Saldo.HasValue && m.Saldo == newMovimento.Saldo));


			return existent == null;
		}

		private static void CopyColumnFromFile(ref Movimento movimento, string columnName, DataRow cellValue)
		{
			var cellValueAsString = cellValue[columnName].ToString().Trim();

			if (string.IsNullOrEmpty(cellValueAsString))
				return;

			switch (columnName)
			{
				case Constants.colDateEval:

					if (!DateTime.TryParseExact(cellValueAsString, "dd.MM.yyyy", null, DateTimeStyles.None, out var dateEVal))
						if (!DateTime.TryParseExact(cellValueAsString, "dd-MM-yy", null, DateTimeStyles.None, out dateEVal))
							dateEVal = DateTime.ParseExact(cellValueAsString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

					movimento.DtEval = dateEVal;
					break;
				case Constants.colRelBancaire:
					if (double.TryParse(cellValueAsString.Replace("'", "").Replace(",", "."), out var relBanc))
						movimento.RelBancaria = relBanc;
					break;
				case Constants.colPortfeuille:
					movimento.Portofolio = cellValueAsString;
					break;
				case Constants.colProduit:
					movimento.Produto = cellValueAsString;
					break;
				case Constants.colIBAN:
					movimento.IBAN = cellValueAsString;
					break;
				case Constants.colMonn:
					movimento.Moeda = cellValueAsString;
					break;
				case Constants.colDateDu:
					if (!DateTime.TryParseExact(cellValueAsString, "dd.MM.yyyy", null, DateTimeStyles.None, out var dateDu))
						if (!DateTime.TryParseExact(cellValueAsString, "dd-MM-yy", null, DateTimeStyles.None, out dateDu))
							dateDu = DateTime.ParseExact(cellValueAsString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

					movimento.DtInicio = dateDu;
					break;
				case Constants.colDateAu:
					if (!DateTime.TryParseExact(cellValueAsString, "dd.MM.yyyy", null, DateTimeStyles.None, out var dateAu))
						if (!DateTime.TryParseExact(cellValueAsString, "dd-MM-yy", null, DateTimeStyles.None, out dateAu))
							dateAu = DateTime.ParseExact(cellValueAsString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

					movimento.DtFim = dateAu;
					break;
				case Constants.colDescription:
					movimento.Descricao = cellValueAsString;
					break;
				case Constants.colDateTrans:
					break;
				case Constants.colDateComptab:
					if (!DateTime.TryParseExact(cellValueAsString, "dd.MM.yyyy", null, DateTimeStyles.None, out var dateCont))
						if (!DateTime.TryParseExact(cellValueAsString, "dd-MM-yy", null, DateTimeStyles.None, out dateCont))
							dateCont = DateTime.ParseExact(cellValueAsString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

					movimento.DtContab = dateCont;
					break;
				case Constants.colDateValeur:
					if (!DateTime.TryParseExact(cellValueAsString, "dd.MM.yyyy", null, DateTimeStyles.None, out var dateVal))
						if (!DateTime.TryParseExact(cellValueAsString, "dd-MM-yy", null, DateTimeStyles.None, out dateVal))
							dateVal = DateTime.ParseExact(cellValueAsString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

					movimento.DtValor = dateVal;
					break;
				case Constants.colDescription1:
					movimento.Descricao1 = cellValueAsString;
					break;
				case Constants.colDescription2:
					movimento.Descricao2 = cellValueAsString;
					break;
				case Constants.colDescription3:
					movimento.Descricao3 = cellValueAsString;
					break;
				case Constants.colNoTransaction:
					movimento.NumTrans = cellValueAsString;
					break;
				case Constants.colCoursDevises:
					movimento.CursDevis = cellValueAsString;
					break;
				case Constants.colSousMontant:
					if (double.TryParse(cellValueAsString.Replace("'", "").Replace(",", "."), out var subTot))
						movimento.SubTotal = subTot;
					break;
				case Constants.colDebit:
					if (double.TryParse(cellValueAsString.Replace("'", "").Replace(",", "."), out var debit))
						movimento.Debito = debit;
					break;
				case Constants.colCredit:
					if (double.TryParse(cellValueAsString.Replace("'", "").Replace(",", "."), out var credit))
						movimento.Credito = credit;
					break;
				case Constants.colSolde:
					if (double.TryParse(cellValueAsString.Replace("'", "").Replace(",", "."), out var saldo))
						movimento.Saldo = saldo;
					break;

				default:
					throw new NotSupportedException();
			}
		}
	}

	public class MovimentoCsvAdapter : CsvFileAdapter
	{
		public MovimentoCsvAdapter(string filePath, string connectionString)
			: base(filePath, connectionString)
		{
		}

		protected override List<string> GetColumnNames()
		{
			var columns = new List<string>
			{
				Constants.colDateEval,
				Constants.colRelBancaire,
				Constants.colPortfeuille,
				Constants.colProduit,
				Constants.colIBAN,
				Constants.colMonn,
				Constants.colDateDu,
				Constants.colDateAu,
				Constants.colDescription,
				Constants.colDateTrans,
				Constants.colDateComptab,
				Constants.colDateValeur,
				Constants.colDescription1,
				Constants.colDescription2,
				Constants.colDescription3,
				Constants.colNoTransaction,
				Constants.colCoursDevises,
				Constants.colSousMontant,
				Constants.colDebit,
				Constants.colCredit,
				Constants.colSolde
			};


			return columns;
		}

		protected override void ImportRows(TextFieldParser csvReader)
		{
			var movimentos = new List<Movimento>();

			var movimentoAdapter = new MovimentoAdapter(ConnectionString);

			var existentMovimentos = movimentoAdapter.GetMovimentos();

			OnNumberOfRowsToImportDetermined(new NumberOfRowsEventArgs { NumberOfRows = File.ReadAllLines(filePath).Length - 4 });

			while (!csvReader.EndOfData)
			{
				var movimento = new Movimento();

				var fieldData = csvReader.ReadFields();

				if (fieldData == null)
				{
					OnRowTreated(EventArgs.Empty);
					continue;
				}

				if (AllFieldsEmpty(fieldData))
					break;

				foreach (var columnToRead in columnsToRead)
				{
					CopyExcelFileEntityColumnFromFile(ref movimento, columnToRead.Key, fieldData[columnToRead.Value]);
				}

				OnRowTreated(EventArgs.Empty);

				if (string.IsNullOrEmpty(movimento.IBAN) || !IsNewMovimento(existentMovimentos, movimento))
					continue;

				movimentos.Add(movimento);
			}

			movimentoAdapter.StoreMovimentos(movimentos);
		}

		private bool IsNewMovimento(List<Movimento> existentMovimentos, Movimento newMovimento)
		{
			if (existentMovimentos.Count == 0)
				return true;

			var movsToSearch = existentMovimentos
				.Where(m => m.DtValor == newMovimento.DtValor && m.DtContab == newMovimento.DtContab).ToList();

			var existent = movsToSearch.FirstOrDefault(m => m.Descricao1 == newMovimento.Descricao1 &&
			                                                      m.Descricao2 == newMovimento.Descricao2 && m.Descricao3 == newMovimento.Descricao3 &&
			                                                      (!m.SubTotal.HasValue && !newMovimento.SubTotal.HasValue ||
			                                                       m.SubTotal.HasValue && newMovimento.SubTotal.HasValue && m.SubTotal == newMovimento.SubTotal) &&
			                                                      (!m.Debito.HasValue && !newMovimento.Debito.HasValue ||
			                                                       m.Debito.HasValue && newMovimento.Debito.HasValue && m.Debito == newMovimento.Debito) &&
			                                                      (!m.Credito.HasValue && !newMovimento.Credito.HasValue ||
			                                                       m.Credito.HasValue && newMovimento.Credito.HasValue && m.Credito == newMovimento.Credito) &&
			                                                      (!m.Saldo.HasValue && !newMovimento.Saldo.HasValue ||
			                                                       m.Saldo.HasValue && newMovimento.Saldo.HasValue && m.Saldo == newMovimento.Saldo));


			return existent == null;
		}

		private void CopyExcelFileEntityColumnFromFile(ref Movimento movimento, string columnName, string cellValue)
		{
			if (string.IsNullOrEmpty(cellValue))
				return;

			switch (columnName)
			{
				case Constants.colDateEval:

					if (!DateTime.TryParseExact(cellValue, "dd.MM.yyyy", null, DateTimeStyles.None, out var dateEVal))
						if (!DateTime.TryParseExact(cellValue, "dd-MM-yy", null, DateTimeStyles.None, out dateEVal))
							dateEVal = DateTime.ParseExact(cellValue, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

					movimento.DtEval = dateEVal;
					break;
				case Constants.colRelBancaire:
					if (double.TryParse(cellValue.Replace("'", string.Empty).Replace(",", ".").Replace(".", string.Empty), out var relBanc))
						movimento.RelBancaria = relBanc;
					break;
				case Constants.colPortfeuille:
					movimento.Portofolio = cellValue;
					break;
				case Constants.colProduit:
					movimento.Produto = cellValue;
					break;
				case Constants.colIBAN:
					movimento.IBAN = cellValue;
					break;
				case Constants.colMonn:
					movimento.Moeda = cellValue;
					break;
				case Constants.colDateDu:
					if (!DateTime.TryParseExact(cellValue, "dd.MM.yyyy", null, DateTimeStyles.None, out var dateDu))
						if (!DateTime.TryParseExact(cellValue, "dd-MM-yy", null, DateTimeStyles.None, out dateDu))
							dateDu = DateTime.ParseExact(cellValue, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

					movimento.DtInicio = dateDu;
					break;
				case Constants.colDateAu:
					if (!DateTime.TryParseExact(cellValue, "dd.MM.yyyy", null, DateTimeStyles.None, out var dateAu))
						if (!DateTime.TryParseExact(cellValue, "dd-MM-yy", null, DateTimeStyles.None, out dateAu))
							dateAu = DateTime.ParseExact(cellValue, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

					movimento.DtFim = dateAu;
					break;
				case Constants.colDescription:
					movimento.Descricao = cellValue;
					break;
				case Constants.colDateTrans:
					break;
				case Constants.colDateComptab:
					if (!DateTime.TryParseExact(cellValue, "dd.MM.yyyy", null, DateTimeStyles.None, out var dateCont))
						if (!DateTime.TryParseExact(cellValue, "dd-MM-yy", null, DateTimeStyles.None, out dateCont))
							dateCont = DateTime.ParseExact(cellValue, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

					movimento.DtContab = dateCont;
					break;
				case Constants.colDateValeur:
					if (!DateTime.TryParseExact(cellValue, "dd.MM.yyyy", null, DateTimeStyles.None, out var dateVal))
						if (!DateTime.TryParseExact(cellValue, "dd-MM-yy", null, DateTimeStyles.None, out dateVal))
							dateVal = DateTime.ParseExact(cellValue, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

					movimento.DtValor = dateVal;
					break;
				case Constants.colDescription1:
					movimento.Descricao1 = cellValue;
					break;
				case Constants.colDescription2:
					movimento.Descricao2 = cellValue;
					break;
				case Constants.colDescription3:
					movimento.Descricao3 = cellValue;
					break;
				case Constants.colNoTransaction:
					movimento.NumTrans = cellValue;
					break;
				case Constants.colCoursDevises:
					movimento.CursDevis = cellValue;
					break;
				case Constants.colSousMontant:
					if (double.TryParse(cellValue.Replace("'", "").Replace(",", "."), out var subTot))
						movimento.SubTotal = subTot;
					break;
				case Constants.colDebit:
					if (double.TryParse(cellValue.Replace("'", "").Replace(",", "."), out var debit))
						movimento.Debito = debit;
					break;
				case Constants.colCredit:
					if (double.TryParse(cellValue.Replace("'", "").Replace(",", "."), out var credit))
						movimento.Credito = credit;
					break;
				case Constants.colSolde:
					if (double.TryParse(cellValue.Replace("'", "").Replace(",", "."), out var saldo))
						movimento.Saldo = saldo;
					break;

				default:
					throw new NotSupportedException();
			}
		}
	}

	public abstract class CsvFileAdapter : FileAdapterBase
	{
		protected readonly Dictionary<string, int> columnsToRead = new Dictionary<string, int>();

		protected abstract List<string> GetColumnNames();
		protected CsvFileAdapter(string filePath, string connectionString)
			: base(filePath, connectionString)
		{
		}
		protected abstract void ImportRows(TextFieldParser csvReader);
		private void BuildColumnDictionary(string[] colFields)
		{
			foreach (var column in GetColumnNames())
			{
				for (var i = 0; i < colFields.Length; i++)
				{
					if (column != colFields[i])
						continue;

					columnsToRead.Add(column, i);
					break;
				}
			}
		}

		//If all fields are empty, it means all rowns to be imported are treated.
		//The remaining ones are the totals, which are not to be imported.
		protected bool AllFieldsEmpty(string[] fields)
		{
			return fields.All(string.IsNullOrEmpty);
		}

		public override void LoadData()
		{
			using (var csvReader = new TextFieldParser(filePath, Encoding.GetEncoding("iso-8859-1")))
			{
				csvReader.SetDelimiters(";");
				csvReader.HasFieldsEnclosedInQuotes = true;
				//read column names
				var colFields = csvReader.ReadFields();

				if (colFields == null)
					return;

				BuildColumnDictionary(colFields);

				ImportRows(csvReader);

				OnDataImported(EventArgs.Empty);
			}
		}
	}
	public static class MovimentoAdapterFactory
   {
	   public static FileAdapterBase GetMovimentoAdapter(string filePath, string connectionString)
	    {
		    string fileExtension = Path.GetExtension(filePath);

		    if (fileExtension == ".csv")
			    return new MovimentoCsvAdapter(filePath, connectionString);

		    return new MovimentoExcelAdapter(filePath, connectionString);
	    }
   }
}
