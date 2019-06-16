using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using EPE.BusinessLayer;

namespace EPE.Gui.PresentationModels
{
    public class ImportModel : INotifyPropertyChanged
    {
        public enum ImportFileType
        {
            Alunos,
            Movimentos
        }

        public ImportFileType FileType { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<NumberOfRowsEventArgs> OnNumberOfRowsToImportDetermined;

        public event EventHandler<NumberOfRowsEventArgs> OnNumberOfRowsToImportUpdated;

        public event EventHandler OnRowTreated;

        public event EventHandler OnDataImported;

        private bool isSingleFile = true;
        public bool IsSingleFile
        {
            get => isSingleFile;
            set
            {
                isSingleFile = value;

                OnPropertyChanged(nameof(IsSingleFile));
            }
        }

        public bool IsForAlunos => FileType == ImportFileType.Alunos;

        private string importFilePath;
        public string ImportFilePath
        {
            get => importFilePath;
            set
            {
                importFilePath = value;

                OnPropertyChanged(nameof(ImportFilePath));
            }
        }

        public bool LimparEnabled => !string.IsNullOrEmpty(importFilePath);

        public bool ImportarEnabled => !string.IsNullOrEmpty(importFilePath);

        public int TotalRows { get; set; }

        public int ActualRowsTreated { get; set; }

        public ImportModel(ImportFileType importFileType)
        {
            FileType = importFileType;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Importar()
        {
            switch (FileType)
            {
                case ImportFileType.Alunos:
                    ImportarAlunos();
                    break;

                case ImportFileType.Movimentos:
                    ImportarMovimentos();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ImportarMovimentos()
        {
            //var adapter = MovimentoAdapterFactory.GetMovimentoAdapter(ImportFilePath);

            //adapter.NumberOfRowsToImportDetermined += Adapter_NumberOfRowsToImportDetermined;
            //adapter.RowTreated += Adapter_RowTreated;
            //adapter.DataImported += Adapter_DataImported;

            //adapter.LoadData();
        }

        private void ImportarAlunos()
        {
            var adapter = new AlunoFileAdapter(ImportFilePath, ConfigurationManager.ConnectionStrings["EPEValidation"].ConnectionString);

            adapter.NumberOfRowsToImportDetermined += Adapter_NumberOfRowsToImportDetermined;
            adapter.RowTreated += Adapter_RowTreated;
            adapter.DataImported += Adapter_DataImported;

            adapter.LoadData();
        }

        private void Adapter_DataImported(object sender, EventArgs e)
        {
            OnDataImported?.Invoke(this, e);
        }

        private void Adapter_RowTreated(object sender, EventArgs e)
        {
            OnRowTreated?.Invoke(this, e);
        }

        private void Adapter_NumberOfRowsToImportDetermined(object sender, NumberOfRowsEventArgs e)
        {
            OnNumberOfRowsToImportDetermined?.Invoke(this, e);
        }

        private void Adapter_NumberOfRowsToImportUpdated(object sender, NumberOfRowsEventArgs e)
        {
            OnNumberOfRowsToImportUpdated?.Invoke(this, e);
        }
    }
}
