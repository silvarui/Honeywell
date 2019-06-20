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
        public List<Validado> Validados { get; }

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

        public bool CanSave
        {
            get { return Validados.Count > 0; }
        }

        public List<Movimento> NaoValidados
        {
            get
            {
                return movimentosToValidate.Where(m => m.CanBeValidated && !Validados.Exists(v => v.Movimento.IdMov == m.IdMov)).ToList();
            }
        }      

        public ValidateModel()
        {
            alunos = new AlunoAdapter(connectionString).GetAlunos();

            movimentosToValidate = new List<Movimento>();
            Validados = new List<Validado>();
        }

        private void LoadMovimentos()
        {
            movimentosToValidate = new MovimentoAdapter(connectionString).GetMovimentosToValidate(DateFrom);

            OnPropertyChanged(nameof(CanValidate));
        }

        public void CheckValidados()
        {
            OnPropertyChanged(nameof(CanSave));
        }

        public void PerformValidation()
        {
            foreach (var mov in NaoValidados)
            {
                var aluno = alunos.FirstOrDefault(a => a.Boletins.Exists(b => b.NumBoletim == mov.BVR));

                if (aluno == null)
                    continue;

                ValidateMovimento(mov, aluno);
            }
        }

        private void ValidateMovimento(Movimento movimento, Aluno aluno)
        {
            Validados.Add(new Validado
            {
                Movimento = movimento,
                Aluno = aluno,
                DtValid = DateTime.Now.Date,
                Valor = movimento.Valor
            });
        }

        public void SaveValidation()
        {
            try
            {
                new ValidadoAdapter(connectionString).StoreValidados(Validados);

                Validados.Clear();
            }
            catch
            {
                throw;
            }
        }
    }
}
