using System.Windows.Forms;

namespace EPE.Gui
{
    public partial class ChildForm : Form
    {
        public ChildForm()
        {
            InitializeComponent();
        }

        private void ChildForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var mdiParent = (MainForm)this.MdiParent;

            mdiParent.UpdateJanelasAbertas(this.Text);
        }

        private void ChildForm_Load(object sender, System.EventArgs e)
        {
            if (DesignMode)
                return;

            var mdiParent = (MainForm)this.MdiParent;

            mdiParent.UpdateJanelasAbertas();
        }
    }
}
