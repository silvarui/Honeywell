using System;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;
using EPE.BusinessLayer;

namespace EPE.Gui
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private bool Sair()
        {
            if (MessageBox.Show("Tem a certeza que deseja sair?", Constants.APP_NAME, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) != DialogResult.Yes)
                return false;

            FormClosing -= MainForm_FormClosing;

            Application.Exit();

            return true;
        }

        public void UpdateJanelasAbertas(string childText = null)
        {
            janelasToolStripMenuItem.DropDownItems.Clear();

            foreach (var childForm in this.MdiChildren)
            {
                if (childText == childForm.Text)
                    continue;

                var menuItem = new ToolStripMenuItem(childForm.Text);
                menuItem.Click += MenuItem_Click;

                janelasToolStripMenuItem.DropDownItems.Add(menuItem);
            }
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;

            var childForm = this.MdiChildren.FirstOrDefault(c => c.Text == menuItem.Text);

            if (childForm == null)
                return;

            childForm.Activate();
        }

        private void SairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sair();
        }

        private void AlunosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var importAlunos = (ImportAlunos)MdiChildren.FirstOrDefault(c => c.GetType() == typeof(ImportAlunos));

            if (importAlunos == null)
            {
                importAlunos = new ImportAlunos { MdiParent = this };

                importAlunos.Show();
            }
            else
            {
                importAlunos.Activate();
            }
        }

        private void MovimentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var importMovimentos = (ImportMovimentos)MdiChildren.FirstOrDefault(c => c.GetType() == typeof(ImportMovimentos));

            if (importMovimentos == null)
            {
                importMovimentos = new ImportMovimentos { MdiParent = this };

                importMovimentos.Show();
            }
            else
            {
                importMovimentos.Activate();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Sair())
                e.Cancel = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = false;

            lblStripStatusUser.Text = WindowsIdentity.GetCurrent().Name;

            UpdateActiveWindow();
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            lblStripStatusDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }

        private void UpdateActiveWindow()
        {
            lblStripStatusActiveWindow.Text = ActiveMdiChild != null ? ActiveMdiChild.Text : string.Empty;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            UpdateActiveWindow();
        }

        private void StatusStrip_SizeChanged(object sender, EventArgs e)
        {
            Size size = new Size();
            size = this.Size;
            size.Width -= 16/*statusStrip.GripRectangle.Width - this is always zero*/;
            size.Width /= 3;

            lblStripStatusActiveWindow.Size = size;
            lblStripStatusUser.Size = size;

            size.Width /= 2;
            lblStripStatusDate.Size = size;
        }

        private void ValidarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var validMovimentos = (ValidateForm)MdiChildren.FirstOrDefault(c => c.GetType() == typeof(ValidateForm));

            if (validMovimentos == null)
            {
                validMovimentos = new ValidateForm() { MdiParent = this };

                validMovimentos.Show();
            }
            else
            {
                validMovimentos.Activate();
            }
        }
    }
}
