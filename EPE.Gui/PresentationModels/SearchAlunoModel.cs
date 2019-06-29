using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPE.BusinessLayer;

namespace EPE.Gui.PresentationModels
{
    public class SearchAlunoModel : BasePresentationModel
    {
        public readonly List<Aluno> Alunos;
        public List<Aluno> SelectedAlunos;

        private readonly Movimento movimento;

        public string DataValor
        {
            get { return movimento.GetPLValue(Movimento.colDtValor).ToString(); }
        }
        
        public string Valor
        {
            get { return movimento.GetPLValue(Movimento.colValor).ToString(); }
        }

        public string Descricao1
        {
            get
            {
                var val = movimento.GetPLValue(Movimento.colDescricao1);

                return val != null ? val.ToString() : string.Empty;
            }
        }

        public string Descricao2
        {
            get
            {
                var val = movimento.GetPLValue(Movimento.colDescricao2);

                return val != null ? val.ToString() : string.Empty;
            }
        }

        public string Descricao3
        {
            get
            {
                var val = movimento.GetPLValue(Movimento.colDescricao3);

                return val != null ? val.ToString() : string.Empty;
            }
        }

        public void AddAluno(Aluno aluno)
        {
            if (SelectedAlunos == null)
                SelectedAlunos = new List<Aluno>();
        
            if (SelectedAlunos.Exists(a => a.IdAluno == aluno.IdAluno))
                return;

            SelectedAlunos.Add(aluno);
        }

        public void RemoveAluno(Aluno aluno)
        {
            SelectedAlunos.Remove(aluno);
        }

        public SearchAlunoModel(Movimento movimento, List<Aluno> alunos)
        {
            this.Alunos = alunos;
            this.movimento = movimento;
        }
    }
}
