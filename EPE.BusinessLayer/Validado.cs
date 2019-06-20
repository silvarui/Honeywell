using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
}
