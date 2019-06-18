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
                colValor
            };

            return columns.ToArray();
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
                case Boletim.colIdAluno:
                    record.Add(new DataElement(columnName, entity.Aluno.IdAluno));
                    break;

                default:
                    base.CopyEntityColumnToRecord(columnName, entity, ref record);
                    break;
            }
        }
    }
}
