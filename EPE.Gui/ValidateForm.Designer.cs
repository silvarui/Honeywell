namespace EPE.Gui
{
    partial class ValidateForm
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpValidate = new System.Windows.Forms.GroupBox();
            this.dtDateFrom = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExportPorValidar = new System.Windows.Forms.Button();
            this.btnSaveAndExport = new System.Windows.Forms.Button();
            this.btnLimparExportFolder = new System.Windows.Forms.Button();
            this.btnSelectExportFolder = new System.Windows.Forms.Button();
            this.txtDestFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.bsValidate = new System.Windows.Forms.BindingSource(this.components);
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabToValidate = new System.Windows.Forms.TabPage();
            this.tabValidated = new System.Windows.Forms.TabPage();
            this.tabAnalyzed = new System.Windows.Forms.TabPage();
            this.egvPorValidar = new EPE.Controls.EntityGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpValidate.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsValidate)).BeginInit();
            this.tabMain.SuspendLayout();
            this.tabToValidate.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grpValidate);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1052, 102);
            this.splitContainer1.SplitterDistance = 234;
            this.splitContainer1.TabIndex = 1;
            // 
            // grpValidate
            // 
            this.grpValidate.Controls.Add(this.dtDateFrom);
            this.grpValidate.Controls.Add(this.label5);
            this.grpValidate.Controls.Add(this.btnAnalyze);
            this.grpValidate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpValidate.Location = new System.Drawing.Point(0, 0);
            this.grpValidate.Name = "grpValidate";
            this.grpValidate.Size = new System.Drawing.Size(234, 102);
            this.grpValidate.TabIndex = 2;
            this.grpValidate.TabStop = false;
            this.grpValidate.Text = "Validacao";
            // 
            // dtDateFrom
            // 
            this.dtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDateFrom.Location = new System.Drawing.Point(76, 23);
            this.dtDateFrom.Name = "dtDateFrom";
            this.dtDateFrom.Size = new System.Drawing.Size(145, 20);
            this.dtDateFrom.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "A partir de:";
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnAnalyze.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAnalyze.Location = new System.Drawing.Point(42, 56);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(154, 25);
            this.btnAnalyze.TabIndex = 9;
            this.btnAnalyze.Text = "Validar";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnExportPorValidar);
            this.groupBox1.Controls.Add(this.btnSaveAndExport);
            this.groupBox1.Controls.Add(this.btnLimparExportFolder);
            this.groupBox1.Controls.Add(this.btnSelectExportFolder);
            this.groupBox1.Controls.Add(this.txtDestFolder);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(814, 102);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Exportação";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(83, 56);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(198, 25);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "Salvar Validação";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnExportPorValidar
            // 
            this.btnExportPorValidar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExportPorValidar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportPorValidar.Location = new System.Drawing.Point(596, 56);
            this.btnExportPorValidar.Name = "btnExportPorValidar";
            this.btnExportPorValidar.Size = new System.Drawing.Size(210, 25);
            this.btnExportPorValidar.TabIndex = 18;
            this.btnExportPorValidar.Text = "Exportar não validados";
            this.btnExportPorValidar.UseVisualStyleBackColor = true;
            // 
            // btnSaveAndExport
            // 
            this.btnSaveAndExport.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSaveAndExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveAndExport.Location = new System.Drawing.Point(310, 56);
            this.btnSaveAndExport.Name = "btnSaveAndExport";
            this.btnSaveAndExport.Size = new System.Drawing.Size(262, 25);
            this.btnSaveAndExport.TabIndex = 17;
            this.btnSaveAndExport.Text = "Guardar e Exportar Validados";
            this.btnSaveAndExport.UseVisualStyleBackColor = true;
            // 
            // btnLimparExportFolder
            // 
            this.btnLimparExportFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLimparExportFolder.Location = new System.Drawing.Point(731, 22);
            this.btnLimparExportFolder.Name = "btnLimparExportFolder";
            this.btnLimparExportFolder.Size = new System.Drawing.Size(75, 23);
            this.btnLimparExportFolder.TabIndex = 14;
            this.btnLimparExportFolder.Text = "Limpar";
            this.btnLimparExportFolder.UseVisualStyleBackColor = true;
            // 
            // btnSelectExportFolder
            // 
            this.btnSelectExportFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectExportFolder.Location = new System.Drawing.Point(652, 22);
            this.btnSelectExportFolder.Name = "btnSelectExportFolder";
            this.btnSelectExportFolder.Size = new System.Drawing.Size(75, 23);
            this.btnSelectExportFolder.TabIndex = 12;
            this.btnSelectExportFolder.Text = "Seleccionar";
            this.btnSelectExportFolder.UseVisualStyleBackColor = true;
            // 
            // txtDestFolder
            // 
            this.txtDestFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDestFolder.Location = new System.Drawing.Point(83, 23);
            this.txtDestFolder.Name = "txtDestFolder";
            this.txtDestFolder.ReadOnly = true;
            this.txtDestFolder.Size = new System.Drawing.Size(563, 20);
            this.txtDestFolder.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Pasta destino";
            // 
            // bsValidate
            // 
            this.bsValidate.DataSource = typeof(EPE.Gui.PresentationModels.ValidateModel);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabToValidate);
            this.tabMain.Controls.Add(this.tabValidated);
            this.tabMain.Controls.Add(this.tabAnalyzed);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 102);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1052, 635);
            this.tabMain.TabIndex = 2;
            // 
            // tabToValidate
            // 
            this.tabToValidate.Controls.Add(this.egvPorValidar);
            this.tabToValidate.Location = new System.Drawing.Point(4, 22);
            this.tabToValidate.Name = "tabToValidate";
            this.tabToValidate.Padding = new System.Windows.Forms.Padding(3);
            this.tabToValidate.Size = new System.Drawing.Size(1044, 609);
            this.tabToValidate.TabIndex = 0;
            this.tabToValidate.Text = "Por validar";
            this.tabToValidate.UseVisualStyleBackColor = true;
            // 
            // tabValidated
            // 
            this.tabValidated.Location = new System.Drawing.Point(4, 22);
            this.tabValidated.Name = "tabValidated";
            this.tabValidated.Padding = new System.Windows.Forms.Padding(3);
            this.tabValidated.Size = new System.Drawing.Size(1044, 609);
            this.tabValidated.TabIndex = 1;
            this.tabValidated.Text = "Validados";
            this.tabValidated.UseVisualStyleBackColor = true;
            // 
            // tabAnalyzed
            // 
            this.tabAnalyzed.Location = new System.Drawing.Point(4, 22);
            this.tabAnalyzed.Name = "tabAnalyzed";
            this.tabAnalyzed.Padding = new System.Windows.Forms.Padding(3);
            this.tabAnalyzed.Size = new System.Drawing.Size(1044, 609);
            this.tabAnalyzed.TabIndex = 2;
            this.tabAnalyzed.Text = "Analisados";
            this.tabAnalyzed.UseVisualStyleBackColor = true;
            // 
            // egvPorValidar
            // 
            this.egvPorValidar.ColumnInfos = null;
            this.egvPorValidar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.egvPorValidar.Location = new System.Drawing.Point(3, 3);
            this.egvPorValidar.Name = "egvPorValidar";
            this.egvPorValidar.Size = new System.Drawing.Size(1038, 603);
            this.egvPorValidar.TabIndex = 0;
            // 
            // ValidateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 737);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ValidateForm";
            this.Text = "ValidateForm";
            this.Load += new System.EventHandler(this.ValidateForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grpValidate.ResumeLayout(false);
            this.grpValidate.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsValidate)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.tabToValidate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox grpValidate;
        private System.Windows.Forms.DateTimePicker dtDateFrom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExportPorValidar;
        private System.Windows.Forms.Button btnSaveAndExport;
        private System.Windows.Forms.Button btnLimparExportFolder;
        private System.Windows.Forms.Button btnSelectExportFolder;
        private System.Windows.Forms.TextBox txtDestFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.BindingSource bsValidate;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabToValidate;
        private System.Windows.Forms.TabPage tabValidated;
        private System.Windows.Forms.TabPage tabAnalyzed;
        private Controls.EntityGridView egvPorValidar;
    }
}