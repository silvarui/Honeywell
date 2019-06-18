using System;
using System.Collections.Generic;
using EPE.DataAccess;

namespace EPE.BusinessLayer
{
	public class Movimento : Entity
	{
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
		public override string[] GetColumnNames()
		{
			var columns = new List<string>
			{
				//colIdMov,
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
				colSaldo
			};

			return columns.ToArray();
		}
	}

	public class MovimentoAdapter : EntityDbAdapter<Movimento>
	{
		private const string USP_STORE_MOVIMENTO = "USP_STORE_MOVIMENTO";
		private const string USP_GET_MOVIMENTOS_TO_VALIDATE = "USP_GET_MOVIMENTOS_TO_VALIDATE";
		private const string USP_GET_MOVIMENTOS = "USP_GET_MOVIMENTOS";

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

		public List<Movimento> GetMovimentosToValidate(DateTime dateFrom)
		{
			var movimentos = new List<Movimento>();

			var param = new Parameters
			{
				new DataElement("DtFrom", dateFrom)
			};

			LoadList(ref movimentos, USP_GET_MOVIMENTOS_TO_VALIDATE, param);

			return movimentos;
		}
	}
}
