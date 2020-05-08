using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using EPE.BusinessLayer;
using EPE.Gui.PresentationModels;

namespace EPE.Gui
{
    public partial class ValidateForm : Form
    {
        private readonly ValidateModel validateModel;

        public ValidateForm()
        {
            if (DesignMode)
                return;

            validateModel = new ValidateModel();

            validateModel.OnDataExported += ValidateModel_OnDataExported;

            InitializeComponent();
        }

        private void ValidateModel_OnDataExported(object sender, EventArgs e)
        {
            MessageBox.Show("Exportação concluída!!");
        }

        private void ValidateForm_Load(object sender, EventArgs e)
        {
            var t = typeof(MainForm);

            var pi = t.GetProperty("MdiClient", BindingFlags.Instance | BindingFlags.NonPublic);
            MdiClient cli = (MdiClient)pi.GetValue(this.MdiParent, null);
            Location = new Point(0, 0);
            Size = new Size(cli.Width - 4, cli.Height - 4);

            bsValidate.DataSource = validateModel;

            egvPorValidar.Initialize(typeof(Movimento), new List<Movimento>());
            egvValidados.Initialize(typeof(Validado), new List<Validado>());
            egvAnalisados.Initialize(typeof(Movimento), new List<Movimento>());
            
            egvPorValidar.OnCellMouseMiddleClicked += EgvPorValidar_OnCellMouseRightClicked;
        }

        private void EgvPorValidar_OnCellMouseRightClicked(object sender, DataGridViewCellEventArgs e)
        {
            var selectedRow = egvPorValidar.GetRow(e.RowIndex);

            var movimentoAnalisado = (Movimento)selectedRow.Tag;

            validateModel.AddAnalisado(movimentoAnalisado);

            egvPorValidar.RemoveEntity(movimentoAnalisado);

            LoadGridViews(false);

            validateModel.CheckValidados();
        }

        private void BtnAnalyze_Click(object sender, EventArgs e)
        {
            LongOperation.StartNonUIOperation(delegate
            {
                validateModel.PerformValidation();
            });

            LoadGridViews();

            validateModel.CheckValidados();
        }

        private void LoadGridViews(bool loadPorValidar = true)
        {
            egvValidados.Initialize(typeof(Validado), validateModel.Validados);
            egvAnalisados.Initialize(typeof(Movimento), validateModel.Analisados);

            if (!loadPorValidar)
                return;

            egvPorValidar.SetEditableColumn(Movimento.colUsername);
            egvPorValidar.Initialize(typeof(Movimento), validateModel.ToManualValidation);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            LongOperation.StartNonUIOperation(delegate
            {
                validateModel.SaveValidation();
            });

            LoadGridViews(false);
        }

        private void BtnSaveAndExport_Click(object sender, EventArgs e)
        {
            LongOperation.StartNonUIOperation(delegate
            {
                validateModel.SaveValidationAndExport();
            });
        }

        private void BtnSelectExportFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = true;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDestFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void BtnLimparExportFolder_Click(object sender, EventArgs e)
        {
            txtDestFolder.Text = string.Empty;
        }

        private void BtnExportPorValidar_Click(object sender, EventArgs e)
        {
            LongOperation.StartNonUIOperation(delegate
            {
                validateModel.ExportarPorValidar();
            });
        }

        private void EgvPorValidar_OnCellEditEnded(object sender, DataGridViewCellEventArgs e)
        {
            var editedRow = egvPorValidar.GetRow(e.RowIndex);

            try
            {
                if (editedRow.Cells[e.ColumnIndex].Value == null)
                    return;

                var usernameToSearch = editedRow.Cells[e.ColumnIndex].Value.ToString();

                if (string.IsNullOrEmpty(usernameToSearch) || usernameToSearch.Length != 8)
                    return;

                var movimentoToValidate = (Movimento)editedRow.Tag;

                validateModel.ValidateMovimento(movimentoToValidate, usernameToSearch);

                egvPorValidar.RemoveEntity(movimentoToValidate);

                LoadGridViews(false);

                validateModel.CheckValidados();
            }
            catch (AlunoNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Validação EPE", MessageBoxButtons.OK, MessageBoxIcon.Error);

                editedRow.Cells[e.ColumnIndex].Value = string.Empty;

                OpenSearchAluno(editedRow);
            }
        }

        private void EgvPorValidar_OnCellDoubleClicked(object sender, DataGridViewCellEventArgs e)
        {
            var editedRow = egvPorValidar.GetRow(e.RowIndex);

            OpenSearchAluno(editedRow);
        }

        private void OpenSearchAluno(DataGridViewRow dataGridViewRow)
        {
            var movimentoToValidate = (Movimento)dataGridViewRow.Tag;

            var searchAluno = new SearchAluno(movimentoToValidate, validateModel.Alunos);

            if (searchAluno.ShowDialog() != DialogResult.OK)
                return;

            validateModel.ValidateMovimento(movimentoToValidate, searchAluno.SelectedAlunos);

            egvPorValidar.RemoveEntity(movimentoToValidate);

            LoadGridViews(false);

            validateModel.CheckValidados();
        }
    }
}
