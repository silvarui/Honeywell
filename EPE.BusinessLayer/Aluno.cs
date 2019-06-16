using System;
using System.Collections.Generic;

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

    public class AlunoAdapter : EntityDbAdapter<Aluno>
    {
        private const string USP_STORE_ALUNO = "USP_STORE_ALUNO";
        private const string USP_GET_ALUNOS = "USP_GET_ALUNOS";

        public AlunoAdapter(string connectionString)
            : base(connectionString)
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
    }
}
