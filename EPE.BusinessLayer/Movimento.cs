﻿using System;
using System.Collections.Generic;
using System.Linq;
using EPE.DataAccess;

namespace EPE.BusinessLayer
{
    public class Movimento : Entity
    {
        private const string IMAGEPRINT = "IMAGEPRINT";
        private const string BESR = "BESR BVRB PVBR";
        private const string FRAIS = "Frais";
        private const string MONTANT_SAISI = "Montant saisi";
        private const string ENTREES_BVRB = "Entrees BVRB-QUICK";
        private const string TRANSACTIONS_OFFERTES = "Transactions offertes";

        public const string colIdMov = "IdMov";
        public const string colDtEval = "DtEval";
        public const string colRelBancaria = "RelBancaria";
        public const string colPortofolio = "Portofolio";
        public const string colProduto = "Produto";
        public const string colIBAN = "IBAN";
        public const string colMoeda = "Moeda";
        public const string colDtInicio = "DtInicio";
        public const string colDtFim = "DtFim";
        public const string colDescricao = "Descricao";
        public const string colDtTransac = "DtTransac";
        public const string colDtContab = "DtContab";
        public const string colDtValor = "DtValor";
        public const string colDescricao1 = "Descricao1";
        public const string colDescricao2 = "Descricao2";
        public const string colDescricao3 = "Descricao3";
        public const string colNumTrans = "NumTrans";
        public const string colCursDevis = "CursDevis";
        public const string colSubTotal = "SubTotal";
        public const string colDebito = "Debito";
        public const string colCredito = "Credito";
        public const string colSaldo = "Saldo";

        public const string colValor = "Valor";
        public const string colUsername = "Username";

        public int IdMov { get; set; }
        public DateTime? DtEval { get; set; }
        public double? RelBancaria { get; set; }
        public string Portofolio { get; set; }
        public string Produto { get; set; }
        public string IBAN { get; set; }
        public string Moeda { get; set; }
        public DateTime? DtInicio { get; set; }
        public DateTime? DtFim { get; set; }
        public string Descricao { get; set; }
        public DateTime? DtTransac { get; set; }
        public DateTime? DtContab { get; set; }
        public DateTime? DtValor { get; set; }
        public string Descricao1 { get; set; }
        public string Descricao2 { get; set; }
        public string Descricao3 { get; set; }
        public string NumTrans { get; set; }
        public string CursDevis { get; set; }
        public double? SubTotal { get; set; }
        public double? Debito { get; set; }
        public double? Credito { get; set; }
        public double? Saldo { get; set; }

        public string BVR
        {
            get
            {
                if (Descricao2 == BESR)
                    return Descricao3.Substring(0, 10);

                return !string.IsNullOrEmpty(Descricao1) && Descricao1.StartsWith(BESR) ? Descricao1.Substring(BESR.Length + 1) : string.Empty;
            }
        }

        public double Valor
        {
            get { return Credito.HasValue ? Credito.Value : SubTotal.GetValueOrDefault(0); }
        }

        public bool CanBeShown
        {
            get
            {
                if (!string.IsNullOrEmpty(BVR) && string.IsNullOrEmpty(Descricao2) && string.IsNullOrEmpty(Descricao3))
                    return false;

                return true;
            }
        }

        public bool CanBeValidated
        {
            get
            {
                if (!string.IsNullOrEmpty(Descricao1) &&
                    (Descricao1.StartsWith(ENTREES_BVRB) || Descricao1.StartsWith(FRAIS) || Descricao1.StartsWith(MONTANT_SAISI) ||
                     Descricao1.StartsWith(TRANSACTIONS_OFFERTES)))
                    return false;

                if (!string.IsNullOrEmpty(Descricao3) && Descricao3.Contains(IMAGEPRINT))
                    return false;

                return Valor > 0 && DtValor != new DateTime();
            }
        }

        public bool Analisado { get; set; }

        public string Username { get; } = string.Empty;

        public override string[] GetColumnNames()
        {
            var columns = new List<string>
            {
                colDtEval,
                colRelBancaria,
                colPortofolio,
                colProduto,
                colIBAN,
                colMoeda,
                colDtInicio,
                colDtFim,
                colDescricao,
                colDtTransac,
                colDtContab,
                colDtValor,
                colDescricao1,
                colDescricao2,
                colDescricao3,
                colNumTrans,
                colCursDevis,
                colSubTotal,
                colDebito,
                colCredito,
                colSaldo,
                colValor,
                colUsername
            };

            return columns.ToArray();
        }

        public override string[] GetColumnNamesForExport()
        {
            var columns = new List<string>(GetColumnNames());

            return columns.Where(c => c != colValor && c != colUsername).ToArray();
        }

        public override string[] GetGridViewColumns()
        {
            var columns = new List<string>
            {
                colValor,
                colDescricao1,
                colDescricao2,
                colDescricao3,
                colDtValor,
                colUsername
            };

            return columns.ToArray();
        }

        public override string GetPLTitle(string columnName)
        {
            switch (columnName)
            {
                case colDtValor:
                    return "Data do movimento";

                default:
                    return base.GetPLTitle(columnName);
            }
        }

        public override string GetColumnType(string columnName)
        {
            switch (columnName)
            {
                case colPortofolio:
                case colProduto:
                case colIBAN:
                case colMoeda:
                case colDescricao:
                case colDescricao1:
                case colDescricao2:
                case colDescricao3:
                case colCursDevis:
                case colDtEval:
                case colDtInicio:
                case colDtFim:
                case colDtTransac:
                case colDtContab:
                case colDtValor:
                case colNumTrans:

                case colRelBancaria:
                case colSubTotal:
                case colDebito:
                case colCredito:
                case colSaldo:
                    return "VARCHAR";

                //case colRelBancaria:
                //case colSubTotal:
                //case colDebito:
                //case colCredito:
                //case colSaldo:
                //    return "DECIMAL";

                default:
                    return string.Empty;
            }
        }
    }

	public class MovimentoAdapter : EntityDbAdapter<Movimento>
	{
		private const string USP_STORE_MOVIMENTO = "USP_STORE_MOVIMENTO";
		private const string USP_GET_MOVIMENTOS_NOT_VALIDATED = "USP_GET_MOVIMENTOS_NOT_VALIDATED";
		private const string USP_GET_MOVIMENTOS = "USP_GET_MOVIMENTOS";
        private const string USP_SET_MOVIMENTO_ANALISADO = "USP_SET_MOVIMENTO_ANALISADO";


        public MovimentoAdapter(string connectionString) 
			: base(connectionString)
		{
		}

		public void StoreMovimentos(List<Movimento> movimentosToStore)
		{
			StoreList(movimentosToStore, USP_STORE_MOVIMENTO);
		}

		public List<Movimento> GetMovimentos()
		{
			var movimentos = new List<Movimento>();

			LoadList(ref movimentos, USP_GET_MOVIMENTOS, null);

			return movimentos;
		}

		public List<Movimento> GetMovimentosNaoValidados(DateTime dateFrom, bool forExport)
		{
			var movimentos = new List<Movimento>();

			var parms = new Parameters
			{
				new DataElement("DtFrom", dateFrom),
                new DataElement("ForExport", forExport ? 1 : 0)
			};

			LoadList(ref movimentos, USP_GET_MOVIMENTOS_NOT_VALIDATED, parms);

			return movimentos;
		}

        public void UpdateMovimentosAnalisados(List<Movimento> movimentos)
        {
            foreach (var movimento in movimentos)
            {
                Parameters parameters = new Parameters();
                parameters.Add(new DataElement("IdMov", movimento.IdMov));

                DtAccess.ExecuteNonQuery(ConnectionString, USP_SET_MOVIMENTO_ANALISADO, parameters);
            }
        }

        protected override void CopyEntityColumnToRecord(string columnName, Movimento entity, ref Record record)
        {
            switch (columnName)
            {
                case Movimento.colUsername:
                case Movimento.colValor:
                    break;

                default:
                    base.CopyEntityColumnToRecord(columnName, entity, ref record);
                    break;
            }
        }
    }
}
