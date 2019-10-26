using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EPE.BusinessLayer;

namespace EPE.Gui.PresentationModels
{
    public class ValidateModel : BasePresentationModel
    {
        public event EventHandler OnDataExported;

        private enum ExporType
        {
            Validados,
            NaoValidados
        }
        public readonly List<Aluno> Alunos;
        private List<Movimento> movimentosToValidate;
        public List<Validado> Validados { get; }

        public List<Movimento> Analisados
        {
            get { return movimentosToValidate.Where(m => m.Analisado).ToList(); }
        }

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

        public string exportFolder;
        public string ExportFolder
        {
            get { return exportFolder; }
            set
            {
                exportFolder = value;
            
                OnPropertyChanged(nameof(ExportFolder));
            }
        }
        public bool CanExport
        {
            get { return !string.IsNullOrEmpty(ExportFolder); }
        }

        public bool CanValidate
        {
            get { return Alunos.Count > 0 && movimentosToValidate.Count > 0; }
        }

        public bool CanSave
        {
            get
            {
                return Validados.Count > 0 || movimentosToValidate.Exists(m => m.Analisado);
            }
        }

        private List<Movimento> NaoValidados
        {
            get
            {
                return movimentosToValidate.Where(m => m.CanBeValidated && !m.Analisado && !Validados.Exists(v => v.Movimento.IdMov == m.IdMov) ).ToList();
            }
        }

        public List<Movimento> ToManualValidation
        {
            get
            {
                return NaoValidados.Where(m => m.CanBeShown).ToList();
            }
        }

        public ValidateModel()
        {
            Alunos = new AlunoAdapter(connectionString).GetAlunos();

            movimentosToValidate = new List<Movimento>();
            Validados = new List<Validado>();
        }

        private void LoadMovimentos()
        {
            movimentosToValidate = new MovimentoAdapter(connectionString).GetMovimentosNaoValidados(DateFrom, false);

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
                if (string.IsNullOrEmpty(mov.BVR))
                    continue;

                var aluno = Alunos.FirstOrDefault(a => a.Boletins.Exists(b => b.NumBoletim == mov.BVR));

                if (aluno == null)
                    continue;

                ValidateMovimento(mov, aluno);
            }
        }

        private void ValidateMovimento(Movimento movimento, Aluno aluno, double? valor = null)
        {
            Validados.Add(new Validado
            {
                Movimento = movimento,
                Aluno = aluno,
                DtValid = DateTime.Now.Date,
                Valor = valor ?? movimento.Valor
            });

            OnPropertyChanged(nameof(CanSave));
        }

        public void SaveValidation()
        {
            try
            {
                new ValidadoAdapter(connectionString).StoreValidados(Validados);

                new MovimentoAdapter(connectionString).UpdateMovimentosAnalisados(Analisados);

                Validados.Clear();
            }
            catch
            {
                throw;
            }
        }

        public void SaveValidationAndExport()
        {
            try
            {
                var adapter = new ValidadoAdapter(connectionString);

                if (Validados.Count > 0)
                {
                    adapter.StoreValidados(Validados);
                }

                Export(ExporType.Validados);
            }
            catch
            {
                throw;
            }
        }

        public void ExportarPorValidar()
        {
            Export(ExporType.NaoValidados);
        }

        private void Export(ExporType exporType)
        {
            var fileToExport = Path.Combine(ExportFolder, exporType == ExporType.Validados ? "Validados.xlsx" : "Por_Validar.xlsx");

            if (exporType == ExporType.Validados)
            {
                new ValidadoFileAdapter(fileToExport, connectionString).ExportValidados(DateFrom);
            }
            else
            {
                new NaoValidadoFileAdapter(fileToExport, connectionString).ExportNaoValidados(DateFrom);
            }

            OnDataExported?.Invoke(this, null);
        }

        public void ValidateMovimento(Movimento movimento, string username)
        {
            var aluno = Alunos.FirstOrDefault(a => !string.IsNullOrEmpty(a.Username) && a.Username.Contains(username));

            if (aluno == null)
                throw new AlunoNotFoundException("Aluno não encontrado!");

            ValidateMovimento(movimento, aluno);
        }

        public void ValidateMovimento(Movimento movimento, List<Aluno> alunosToAssociate)
        {
            var valorToValidado = movimento.Valor / alunosToAssociate.Count;

            foreach (var aluno in alunosToAssociate)
            {
                ValidateMovimento(movimento, aluno, valorToValidado);
            }
        }

        public void AddAnalisado(Movimento movimento)
        {
            movimentosToValidate.SingleOrDefault(m => m.IdMov == movimento.IdMov).Analisado = true;

            OnPropertyChanged(nameof(CanSave));
        }
    }
}
