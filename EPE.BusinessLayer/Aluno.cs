using System;
using System.Collections.Generic;
using EPE.DataAccess;

namespace EPE.BusinessLayer
{
    public class Aluno : Entity
    {
        public const string colIdAluno = "IdAluno";
        public const string colUsername = "Username";
        public const string colNome = "Nome";
        public const string colDtNasc = "DtNasc";
        public const string colEscola = "Escola";
        public const string colProfessor = "Professor";
        public const string colEncEduc = "EncEduc";
        public const string colMorada = "Morada";
        public const string colCPostal = "CPostal";
        public const string colLocalidade = "Localidade";
        public const string colCantao = "Cantao";
        public const string colTelefone = "Telefone";
        public const string colTelemovel = "Telemovel";
        public const string colEmail = "Email";

        public int IdAluno { get; set; }
        public string Username { get; set; }
        public string Nome { get; set; }
        public DateTime? DtNasc { get; set; }
        public string Escola { get; set; }
        public string Professor { get; set; }
        public string EncEduc { get; set; }
        public string Morada { get; set; }
        public double? CPostal { get; set; }
        public string Localidade { get; set; }
        public string Cantao { get; set; }
        public string Telefone { get; set; }
        public string Telemovel { get; set; }
        public string Email { get; set; }

        public override object GetEntityKey()
        {
            return IdAluno;
        }

        public List<Boletim> Boletins = new List<Boletim>();

        public override string[] GetColumnNames()
        {
            var columns = new List<string>
            {
                colIdAluno,
                colUsername,
                colNome,
                colDtNasc,
                colEscola,
                colProfessor,
                colEncEduc,
                colMorada,
                colCPostal,
                colLocalidade,
                colCantao,
                colTelefone,
                colTelemovel,
                colEmail
            };

            return columns.ToArray();
        }
    }

    public class AlunoAdapter : CacheableEntityAdapter<Aluno>
    {
        private static Dictionary<object, Aluno> cache = new Dictionary<object, Aluno>();

        private const string USP_STORE_ALUNO = "USP_STORE_ALUNO";
        private const string USP_GET_ALUNOS = "USP_GET_ALUNOS";

        public AlunoAdapter(string connectionString)
            : base(cache, connectionString, USP_GET_ALUNOS)
        {
        }

        public void StoreAlunos(List<Aluno> alunosToStore)
        {
            foreach (var aluno in alunosToStore)
            {
                var newAluno = aluno;

                StoreSingle(ref newAluno, USP_STORE_ALUNO);

                if (aluno.IdAluno > 0)
                    continue;

                aluno.IdAluno = newAluno.IdAluno;
            }
        }

        public List<Aluno> GetAlunos()
        {
            var alunos = new List<Aluno>();

            LoadList(ref alunos, USP_GET_ALUNOS, null);

            return alunos;
        }

        protected override void CopyEntityColumnFromRecord(string columnName, ref Aluno entity, Record record)
        {
            base.CopyEntityColumnFromRecord(columnName, ref entity, record);

            switch (columnName)
            {
                case Aluno.colIdAluno:
                    {
                        entity.Boletins = new BoletimAdapter(ConnectionString).GetBoletinsForAluno(entity.IdAluno);
                    }
                    break;
            }
        }
    }
}
