using System.Collections.Generic;
using EPE.BusinessLayer;
using System;
using System.Linq;

namespace EPE.Gui.PresentationModels
{
    public class ValidateModel : BasePresentationModel
    {
        private readonly List<Aluno> alunos;
        private List<Movimento> movimentosToValidate;
        private List<Validado> validados;

        private DateTime dateFrom;
        public DateTime DateFrom
        {
            get { return dateFrom; }
            set
            {
                dateFrom = value;

                LoadMovimentos();

                OnPropertyChanged(nameof(DateFrom));
            }
        }

        public bool CanValidate
        {
            get { return alunos.Count > 0 && movimentosToValidate.Count > 0; }
        }

        private List<Movimento> NaoValidados
        {
            get
            {
                return movimentosToValidate.Where(m => m.CanBeValidated && !validados.Exists(v => v.Movimento.IdMov == m.IdMov)).ToList();
            }
        }      

        public ValidateModel()
        {
            alunos = new AlunoAdapter(connectionString).GetAlunos();

            movimentosToValidate = new List<Movimento>();
            validados = new List<Validado>();
        }

        private void LoadMovimentos()
        {
            movimentosToValidate = new MovimentoAdapter(connectionString).GetMovimentosToValidate(DateFrom);

            OnPropertyChanged(nameof(CanValidate));
        }

        public void PerformValidation()
        {
            foreach (var mov in NaoValidados)
            {
                //var boletim = boletins.FirstOrDefault(b => b.NumBoletim == mov.BVR);

                //if (boletim == null)
                //    continue;

                var aluno = alunos.FirstOrDefault(a => a.Boletins.Exists(b => b.NumBoletim == mov.BVR));

                if (aluno == null)
                    continue;

                ValidateMovimento(mov, aluno);
            }
        }

        private void ValidateMovimento(Movimento movimento, Aluno aluno)
        {
            validados.Add(new Validado
            {
                Movimento = movimento,
                Aluno = aluno,
                DtValid = DateTime.Now.Date,
                Valor = movimento.Valor
            });
        }
    }
}
