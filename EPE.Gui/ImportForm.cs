using System;
using System.Text;
using System.Windows.Forms;
using EPE.BusinessLayer;
using EPE.Gui.PresentationModels;

using ImportFileType = EPE.Gui.PresentationModels.ImportModel.ImportFileType;

namespace EPE.Gui
{
    public partial class ImportForm : ChildForm
    {
        private readonly ImportModel importModel;
        protected ImportForm(ImportFileType importFileType)
        {
            if (DesignMode)
                return;

            importModel = new ImportModel(importFileType);

            importModel.OnNumberOfRowsToImportDetermined += ImportModel_OnNumberOfRowsToImportDetermined;
            importModel.OnRowTreated += ImportModel_OnRowTreated;
            importModel.OnDataImported += ImportModelOnDataImported;
            importModel.OnNumberOfRowsToImportUpdated += ImportModel_OnNumberOfRowsToImportUpdated;

            InitializeComponent();

            Text = string.Format(Text,
                importModel.FileType == ImportFileType.Alunos ? Constants.ALUNOS : Constants.MOVIMENTOS);

            chkFicheiroUnico.Visible = importModel.IsForAlunos;
        }

        private void ImportForm_Load(object sender, EventArgs e)
        {
            bsImport.DataSource = importModel;
        }

        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            SelectFile(importModel.FileType);
        }

        private void SelectFile(ImportFileType importFileType)
        {
            string fileFilter;

            switch (importFileType)
            {
                case ImportFileType.Alunos:
                    //case ImportFileType.Boletins:
                    fileFilter = "Excel Files|*.xls;*.xlsx;";
                    break;

                case ImportFileType.Movimentos:
                    fileFilter = "CSV Files|*.csv;|Excel Files|*.xls;*.xlsx;";
                    break;

                default:
                    throw new NotSupportedException();
            }

            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = fileFilter;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.Multiselect = importFileType == ImportFileType.Alunos && !importModel.IsSingleFile;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            if (!openFileDialog1.Multiselect)
                txtFilePath.Text = openFileDialog1.FileName;
            else
            {
                var filePaths = new StringBuilder();

                foreach (var selectedFiles in openFileDialog1.FileNames)
                {
                    filePaths.Append(selectedFiles + "|");
                }

                txtFilePath.Text = filePaths.ToString().Remove(filePaths.ToString().LastIndexOf("|", StringComparison.Ordinal));
            }
        }

        private void BtnCleanFile_Click(object sender, EventArgs e)
        {
            txtFilePath.Text = string.Empty;
        }

        private void ImportModelOnDataImported(object sender, EventArgs e)
        {
            MessageBox.Show("Importacao concluída!!", Constants.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ImportModel_OnRowTreated(object sender, EventArgs e)
        {
            progressBar.Value++;
            progressBar.Refresh();
        }

        private void ImportModel_OnNumberOfRowsToImportDetermined(object sender, NumberOfRowsEventArgs e)
        {
            progressBar.Maximum = e.NumberOfRows;
        }

        private void ImportModel_OnNumberOfRowsToImportUpdated(object sender, NumberOfRowsEventArgs e)
        {
            progressBar.Maximum += e.NumberOfRows;
        }

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            progressBar.Value = 0;
            progressBar.Maximum = 100;

            importModel.Importar();
        }
    }

    public class ImportAlunos
        : ImportForm
    {
        public ImportAlunos()
        : base(ImportFileType.Alunos)
        {
        }
    }

    public class ImportMovimentos
        : ImportForm
    {
        public ImportMovimentos()
            : base(ImportFileType.Movimentos)
        {
        }
    }
}
