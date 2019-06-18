using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

            InitializeComponent();
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
        }
    }
}
