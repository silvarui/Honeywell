namespace EPE.Gui
{
    partial class LongOperation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.lblWaitingDb = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // lblWaitingDb
            // 
            this.lblWaitingDb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWaitingDb.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaitingDb.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblWaitingDb.Location = new System.Drawing.Point(0, 0);
            this.lblWaitingDb.Name = "lblWaitingDb";
            this.lblWaitingDb.Size = new System.Drawing.Size(209, 28);
            this.lblWaitingDb.TabIndex = 3;
            this.lblWaitingDb.Text = "Por favor aguarde...";
            this.lblWaitingDb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LongOperation
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(209, 28);
            this.ControlBox = false;
            this.Controls.Add(this.lblWaitingDb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LongOperation";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.LongOperation_Load);
            this.Shown += new System.EventHandler(this.LongOperation_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Label lblWaitingDb;
    }
}