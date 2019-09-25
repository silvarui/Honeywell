using System;
using System.Collections.Generic;
using System.Linq;
using EPE.DataAccess;

namespace EPE.BusinessLayer
{
    public class Validado : Entity
    {
        public const string colIdAluno = "IdAluno";
        public const string colIdMov = "IdMov";
        public const string colDtValid = "DtValid";
        public const string colValor = "Valor";

        public Aluno Aluno { get; set; }
        public Movimento Movimento {get;set;}
        public DateTime DtValid { get; set; }
        public double Valor { get; set; }

        public override string[] GetColumnNames()
        {
            var columns = new List<string>
            {
                colIdAluno,
                colIdMov,
                colDtValid,
                Movimento.colDtValor,
                Aluno.colUsername,
                Aluno.colNome,
                colValor
            };

            return columns.ToArray();
        }

        public override string[] GetGridViewColumns()
        {
            var columns = new List<string>
            {
                Movimento.colDtValor,
                Aluno.colUsername,
                Aluno.colNome,
                colValor
            };

            return columns.ToArray();
        }

        public override object GetPLValue(string columnName)
        {
            switch (columnName)
            {
                case Movimento.colDtValor:
                    return this.Movimento.DtValor.Value.ToString("dd-MM-yyyy");

                case Aluno.colUsername:
                    return Aluno.Username;

                case Aluno.colNome:
                    return Aluno.Nome;

                default:
                    return base.GetPLValue(columnName);
            }
        }

        public override string GetPLTitle(string columnName)
        {
            switch (columnName)
            {
                case Movimento.colDtValor:
                    return "Data do movimento";

                default:
                    return base.GetPLTitle(columnName);
            }
        }
    }

    public class ValidadoAdapter : EntityDbAdapter<Validado>
    {
        private const string USP_STORE_VALIDADO = "USP_STORE_VALIDADO";
        
        public ValidadoAdapter(string connectionString)
            : base(connectionString)
        { }

        public void StoreValidados(List<Validado> validadosToStore)
        {
            StoreList(validadosToStore, USP_STORE_VALIDADO);
        }

        protected override void CopyEntityColumnToRecord(string columnName, Validado entity, ref Record record)
        {
            switch (columnName)
            {
                case Validado.colIdAluno:
                    record.Add(new DataElement(columnName, entity.Aluno.IdAluno));
                    break;

                case Validado.colIdMov:
                    record.Add(new DataElement(columnName, entity.Movimento.IdMov));
                    break;

                case Movimento.colDtValor:
                case Aluno.colUsername:
                case Aluno.colNome:
                    break;

                default:
                    base.CopyEntityColumnToRecord(columnName, entity, ref record);
                    break;
            }
        }
    }

    public class ValidadoForExport : Entity, IExportEntity
    {
        public const string colAnoLectivo = "Ano lectivo";
        public const string colMoeda = "Código da Moeda";
        public const string colModoPagamento = "Modo de pagamento";
        public const string colUsername = "EPE do Aluno";
        public const string colValor = "Valor do pagamento";
        public const string colNome = "Nome do Aluno";

        public string Username { get; set; }

        public string Nome { get; set; }

        public double Valor { get; set; }

        public string AnoLectivo
        {
            get
            {
                var currentYear = DateTime.Now.Year;

                return string.Format("{0}/{1}", currentYear - 1, currentYear);
            }
        }

        public string Moeda => "3";

        public string ModoPagamento => "1";

        public override object this[string columnName]
        {
            set => base[columnName] = value;
            get
            {
                switch (columnName)
                {
                    case colMoeda:
                        return Moeda;

                    case colUsername:
                        return Username;

                    case colAnoLectivo:
                        return AnoLectivo;

                    case colModoPagamento:
                        return ModoPagamento;

                    case colValor:
                        return Valor;

                    case colNome:
                        return Nome;

                    default:
                        return base[columnName];
                }
            }
        }   

        public override string[] GetColumnNames()
        {
            var columns = new List<string>
            {
                colAnoLectivo,
                colUsername,
                colNome,
                colValor,
                colMoeda,
                colModoPagamento
            };

            return columns.ToArray();
        }

        public override string GetColumnType(string columnName)
        {
            switch (columnName)
            {
                case colAnoLectivo:
                case colUsername:
                case colNome:
                    return "VARCHAR";

                case colValor:
                    return "DECIMAL";

                case colModoPagamento:
                case colMoeda:
                    return "INT";

                default:
                    return string.Empty;
            }
        }
    }

    public class ValidadoForExportAdapter : EntityDbAdapter<ValidadoForExport>
    {
        private const string USP_GET_VALIDADOS_FOR_EXPORT = "USP_GET_VALIDADOS_FOR_EXPORT";

        public ValidadoForExportAdapter(string connectionString)
            : base(connectionString)
        { }

        public List<ValidadoForExport> GetValidadosForExport(DateTime dateFrom)
        {
            var validados = new List<ValidadoForExport>();

            var validadosForExport = new List<ValidadoForExport>();

            var param = new Parameters
            {
                new DataElement("DtFrom", dateFrom)
            };

            LoadList(ref validados, USP_GET_VALIDADOS_FOR_EXPORT, param);

            //Ordenar lista para colocar username = null no fim da lista

            validadosForExport.AddRange(validados.Where(v => !string.IsNullOrEmpty(v.Username)).OrderBy(v => v.Username).Concat(validados.Where(v => string.IsNullOrEmpty(v.Username)).OrderBy(v => v.Nome)));

            return validadosForExport;
        }
    }
}
