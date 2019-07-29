using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EPE.BusinessLayer;
using EPE.Gui.PresentationModels;

namespace EPE.Gui
{
    public partial class SearchAluno : Form
    {
        private readonly SearchAlunoModel searchModel;

        public List<Aluno> SelectedAlunos
        {
            get { return searchModel.SelectedAlunos; }
        }

        public SearchAluno(Movimento movimento, List<Aluno> alunos)
        {
            if (DesignMode)
                return;

            searchModel = new SearchAlunoModel(movimento, alunos);

            InitializeComponent();

            dgvAlunos.AutoGenerateColumns = false;
            dgvSelectedAlunos.AutoGenerateColumns = false;
        }

        private void SearchAluno_Load(object sender, EventArgs e)
        {
            txtData.Text = searchModel.DataValor;
            txtValor.Text = searchModel.Valor;
            txtDescr1.Text = searchModel.Descricao1;
            txtDescr2.Text = searchModel.Descricao2;
            txtDescr3.Text = searchModel.Descricao3;

            PopulateAlunos(searchModel.Alunos);

            CenterToScreen();

            txtNomeAluno.Focus();
        }

        private void ApplyAlunosFilter()
        {
            var epeNumber = txtUsername.Text;
            var nomeAluno = txtNomeAluno.Text;
            var nomeEncEdu = txtEncEdu.Text;
            var endereco = txtEndereco.Text;
            var codPostal = txtCodPostal.Text;
            var localidade = txtLocalidade.Text;
            var cantao = txtCantao.Text;

            List<Aluno> alunosToShow = searchModel.Alunos;

            if (!string.IsNullOrEmpty(epeNumber))
                alunosToShow = alunosToShow.Where(a => !string.IsNullOrEmpty(a.Username) && a.Username.ToUpper().Contains(epeNumber.ToUpper())).ToList();

            if (!string.IsNullOrEmpty(nomeAluno))
                alunosToShow = alunosToShow.Where(a => !string.IsNullOrEmpty(a.Nome) && a.Nome.ToUpper().Contains(nomeAluno.ToUpper())).ToList();

            if (!string.IsNullOrEmpty(nomeEncEdu))
                alunosToShow = alunosToShow.Where(a => !string.IsNullOrEmpty(a.EncEduc) && a.EncEduc.ToUpper().Contains(nomeEncEdu.ToUpper())).ToList();

            if (!string.IsNullOrEmpty(endereco))
                alunosToShow = alunosToShow.Where(a => !string.IsNullOrEmpty(a.Morada) && a.Morada.ToUpper().Contains(endereco.ToUpper())).ToList();

            if (!string.IsNullOrEmpty(codPostal))
                alunosToShow = alunosToShow.Where(a => !string.IsNullOrEmpty(a.CPostal) && a.CPostal.ToUpper().Contains(codPostal.ToUpper())).ToList();

            if (!string.IsNullOrEmpty(localidade))
                alunosToShow = alunosToShow.Where(a => !string.IsNullOrEmpty(a.Localidade) && a.Localidade.ToUpper().Contains(localidade.ToUpper())).ToList();

            if (!string.IsNullOrEmpty(cantao))
                alunosToShow = alunosToShow.Where(a => !string.IsNullOrEmpty(a.Cantao) && a.Cantao.ToUpper().Contains(cantao.ToUpper())).ToList();

            PopulateAlunos(alunosToShow);
        }

        private void PopulateAlunos(List<Aluno> alunos)
        {
            dgvAlunos.DataSource = alunos;
        }

        private void TxtBox_TextChanged(object sender, EventArgs e)
        {
            ApplyAlunosFilter();
        }

        private void DgvAlunos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var aluno = (Aluno)dgvAlunos.Rows[e.RowIndex].DataBoundItem;

            searchModel.AddAluno(aluno);

            PopulateSelectedAlunos();
        }

        private void DgvSelectedAlunos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var aluno = (Aluno)dgvSelectedAlunos.Rows[e.RowIndex].DataBoundItem;

            searchModel.RemoveAluno(aluno);

            PopulateSelectedAlunos();
        }

        private void PopulateSelectedAlunos()
        {
            var alunosToShow = new List<Aluno>(searchModel.SelectedAlunos);

            dgvSelectedAlunos.DataSource = alunosToShow;

            btnFinishAndReturn.Enabled = alunosToShow.Count > 0;
        }

        private void BtnFinishAndReturn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
