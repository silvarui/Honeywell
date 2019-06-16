using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace EPE.Gui
{
    public partial class LongOperation : Form
    {
        internal Exception longOperationException;

        /// <summary>
        /// Call this static method to start a time-consuming operation involving user interface (the operation may access form controls). The user is notified through "Please wait..." window.
        /// </summary>
        public static void StartUIOperation(Control parent, LongOperationEventHandler operation)
        {
            LongOperation dlg = new LongOperation(parent, operation);
            dlg.ShowDialog();
            if (dlg.longOperationException != null)
                throw dlg.longOperationException;
        }

        /// <summary>
        /// Call this static method to start a time-consuming operation that DOES NOT involve user interface (the operation may NOT access form controls). 
        /// For example, you may call this method to execute an entity adapter method that takes several seconds to complete. 
        /// The user is notified through "Please wait..." window.
        /// </summary>
        public static void StartNonUIOperation(LongOperationEventHandler operation)
        {
            LongOperation dlg = new LongOperation(null, operation);
            dlg.ShowDialog();
            if (dlg.longOperationException != null)
                throw new Exception(null, dlg.longOperationException);
        }

        /// <summary>
        /// Call this static method to start a time-consuming operation. 
        /// The user is notified through a hourglass icon.
        /// </summary>
        public static void StartUIOperationWithHourglass(Control ctrl, LongOperationEventHandler operation)
        {
            Cursor oldCursor = ctrl.Cursor;
            ctrl.Cursor = Cursors.WaitCursor;
            try
            {
                operation();
            }
            finally
            {
                ctrl.Cursor = oldCursor;
            }
        }

        protected LongOperation()
        {
            InitializeComponent();
        }

        public LongOperation(Control parent, LongOperationEventHandler operation)
        {
            InitializeComponent();
            this.operation = operation;
            this.parent = parent;
        }

        private LongOperationEventHandler operation;
        private Control parent;


        private void LongOperation_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            CenterToScreen();
        }

        private void LongOperation_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (this.operation == null)
            {
                DialogResult = DialogResult.OK;
                return;
            }

            if (this.parent != null)
            {
                try
                {
                    this.operation();
                }
                catch (Exception ex)
                {
                    this.longOperationException = ex;
                }
                finally
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                this.backgroundWorker.RunWorkerAsync();
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //since this is a new thread, make sure the culture is set to invariant otherwise it will inherit the current OS culture
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            this.operation();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                this.longOperationException = e.Error; //to throw back the error to the main thread

            DialogResult = DialogResult.OK;
        }

        public delegate void LongOperationEventHandler();
    }
}
