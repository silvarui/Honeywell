using System.Collections.Generic;
using EPE.DataAccess;

namespace EPE.BusinessLayer
{
    public class Boletim : Entity
    {
        public const string colIdAluno = "IdAluno";
        public const string colNumBoletim = "NumBoletim";

        public Aluno Aluno { get; set; }
        public string NumBoletim { get; set; }

        public override string[] GetColumnNames()
        {
            var columns = new List<string>
            {
                colIdAluno,
                colNumBoletim,
            };
            return columns.ToArray();
        }
    }

    public class BoletimAdapter : EntityDbAdapter<Boletim>
    {
        private const string USP_STORE_BOLETIM = "USP_STORE_BOLETIM";
        private const string USP_GET_BOLETINS = "USP_GET_BOLETINS";

        public BoletimAdapter(string connectionString)
            : base(connectionString)
        {
        }

        public void StoreBoletins(List<Boletim> boletinsToStore)
        {
            StoreList(boletinsToStore, USP_STORE_BOLETIM);
        }

        protected override void CopyEntityColumnToRecord(string columnName, Boletim entity, ref Record record)
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

        public List<Boletim> GetBoletins()
        {
            var boletins = new List<Boletim>();

            LoadList(ref boletins, USP_GET_BOLETINS, null);

            return boletins;
        }
    }
}
