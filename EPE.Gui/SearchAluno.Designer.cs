namespace EPE.Gui
{
    partial class SearchAluno
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
            this.dgvAlunos = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnFinishAndReturn = new System.Windows.Forms.Button();
            this.grpFiltros = new System.Windows.Forms.GroupBox();
            this.txtCodPostal = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtCantao = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtLocalidade = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtEndereco = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtEncEdu = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtNomeAluno = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.grpMovimento = new System.Windows.Forms.GroupBox();
            this.txtDescr3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDescr2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDescr1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtData = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvSelectedAlunos = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsSearch = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlunos)).BeginInit();
            this.panel1.SuspendLayout();
            this.grpFiltros.SuspendLayout();
            this.grpMovimento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedAlunos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAlunos
            // 
            this.dgvAlunos.AllowUserToAddRows = false;
            this.dgvAlunos.AllowUserToDeleteRows = false;
            this.dgvAlunos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAlunos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAlunos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7});
            this.dgvAlunos.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvAlunos.Location = new System.Drawing.Point(0, 195);
            this.dgvAlunos.Name = "dgvAlunos";
            this.dgvAlunos.ReadOnly = true;
            this.dgvAlunos.Size = new System.Drawing.Size(1052, 299);
            this.dgvAlunos.TabIndex = 14;
            this.dgvAlunos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvAlunos_CellDoubleClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Username";
            this.dataGridViewTextBoxColumn1.HeaderText = "Username";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Nome";
            this.dataGridViewTextBoxColumn2.HeaderText = "Nome";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "EncEduc";
            this.dataGridViewTextBoxColumn3.HeaderText = "Enc. Educação";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Morada";
            this.dataGridViewTextBoxColumn4.HeaderText = "Morada";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "CPostal";
            this.dataGridViewTextBoxColumn5.HeaderText = "Cod. Postal";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Localidade";
            this.dataGridViewTextBoxColumn6.HeaderText = "Localidade";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "Cantao";
            this.dataGridViewTextBoxColumn7.HeaderText = "Cantão";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnFinishAndReturn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 694);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1052, 43);
            this.panel1.TabIndex = 12;
            // 
            // btnFinishAndReturn
            // 
            this.btnFinishAndReturn.Enabled = false;
            this.btnFinishAndReturn.Location = new System.Drawing.Point(438, 10);
            this.btnFinishAndReturn.Name = "btnFinishAndReturn";
            this.btnFinishAndReturn.Size = new System.Drawing.Size(155, 23);
            this.btnFinishAndReturn.TabIndex = 8;
            this.btnFinishAndReturn.Text = "Salvar e Sair";
            this.btnFinishAndReturn.UseVisualStyleBackColor = true;
            this.btnFinishAndReturn.Click += new System.EventHandler(this.BtnFinishAndReturn_Click);
            // 
            // grpFiltros
            // 
            this.grpFiltros.Controls.Add(this.txtCodPostal);
            this.grpFiltros.Controls.Add(this.label12);
            this.grpFiltros.Controls.Add(this.txtCantao);
            this.grpFiltros.Controls.Add(this.label11);
            this.grpFiltros.Controls.Add(this.txtLocalidade);
            this.grpFiltros.Controls.Add(this.label10);
            this.grpFiltros.Controls.Add(this.txtEndereco);
            this.grpFiltros.Controls.Add(this.label9);
            this.grpFiltros.Controls.Add(this.txtEncEdu);
            this.grpFiltros.Controls.Add(this.label8);
            this.grpFiltros.Controls.Add(this.txtNomeAluno);
            this.grpFiltros.Controls.Add(this.label7);
            this.grpFiltros.Controls.Add(this.txtUsername);
            this.grpFiltros.Controls.Add(this.label6);
            this.grpFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFiltros.Location = new System.Drawing.Point(0, 120);
            this.grpFiltros.Name = "grpFiltros";
            this.grpFiltros.Size = new System.Drawing.Size(1052, 75);
            this.grpFiltros.TabIndex = 11;
            this.grpFiltros.TabStop = false;
            this.grpFiltros.Text = "Filtros";
            // 
            // txtCodPostal
            // 
            this.txtCodPostal.Location = new System.Drawing.Point(888, 40);
            this.txtCodPostal.Name = "txtCodPostal";
            this.txtCodPostal.Size = new System.Drawing.Size(152, 20);
            this.txtCodPostal.TabIndex = 13;
            this.txtCodPostal.TextChanged += new System.EventHandler(this.TxtBox_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(819, 44);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "Cód. Postal";
            // 
            // txtCantao
            // 
            this.txtCantao.Location = new System.Drawing.Point(888, 16);
            this.txtCantao.Name = "txtCantao";
            this.txtCantao.Size = new System.Drawing.Size(152, 20);
            this.txtCantao.TabIndex = 11;
            this.txtCantao.TextChanged += new System.EventHandler(this.TxtBox_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(819, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Cantão";
            // 
            // txtLocalidade
            // 
            this.txtLocalidade.Location = new System.Drawing.Point(588, 40);
            this.txtLocalidade.Name = "txtLocalidade";
            this.txtLocalidade.Size = new System.Drawing.Size(209, 20);
            this.txtLocalidade.TabIndex = 9;
            this.txtLocalidade.TextChanged += new System.EventHandler(this.TxtBox_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(504, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Localidade";
            // 
            // txtEndereco
            // 
            this.txtEndereco.Location = new System.Drawing.Point(85, 40);
            this.txtEndereco.Name = "txtEndereco";
            this.txtEndereco.Size = new System.Drawing.Size(406, 20);
            this.txtEndereco.TabIndex = 7;
            this.txtEndereco.TextChanged += new System.EventHandler(this.TxtBox_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Endereço";
            // 
            // txtEncEdu
            // 
            this.txtEncEdu.Location = new System.Drawing.Point(588, 16);
            this.txtEncEdu.Name = "txtEncEdu";
            this.txtEncEdu.Size = new System.Drawing.Size(209, 20);
            this.txtEncEdu.TabIndex = 5;
            this.txtEncEdu.TextChanged += new System.EventHandler(this.TxtBox_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(504, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Enc. Educação";
            // 
            // txtNomeAluno
            // 
            this.txtNomeAluno.Location = new System.Drawing.Point(266, 16);
            this.txtNomeAluno.Name = "txtNomeAluno";
            this.txtNomeAluno.Size = new System.Drawing.Size(225, 20);
            this.txtNomeAluno.TabIndex = 3;
            this.txtNomeAluno.TextChanged += new System.EventHandler(this.TxtBox_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(193, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Nome Aluno";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(85, 16);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 20);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.TextChanged += new System.EventHandler(this.TxtBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Numero EPE";
            // 
            // grpMovimento
            // 
            this.grpMovimento.Controls.Add(this.txtDescr3);
            this.grpMovimento.Controls.Add(this.label5);
            this.grpMovimento.Controls.Add(this.txtDescr2);
            this.grpMovimento.Controls.Add(this.label4);
            this.grpMovimento.Controls.Add(this.txtDescr1);
            this.grpMovimento.Controls.Add(this.label3);
            this.grpMovimento.Controls.Add(this.txtValor);
            this.grpMovimento.Controls.Add(this.label2);
            this.grpMovimento.Controls.Add(this.txtData);
            this.grpMovimento.Controls.Add(this.label1);
            this.grpMovimento.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpMovimento.Location = new System.Drawing.Point(0, 0);
            this.grpMovimento.Name = "grpMovimento";
            this.grpMovimento.Size = new System.Drawing.Size(1052, 120);
            this.grpMovimento.TabIndex = 10;
            this.grpMovimento.TabStop = false;
            this.grpMovimento.Text = "Movimento a associar";
            // 
            // txtDescr3
            // 
            this.txtDescr3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescr3.Location = new System.Drawing.Point(85, 92);
            this.txtDescr3.Name = "txtDescr3";
            this.txtDescr3.ReadOnly = true;
            this.txtDescr3.Size = new System.Drawing.Size(955, 20);
            this.txtDescr3.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Descrição 3";
            // 
            // txtDescr2
            // 
            this.txtDescr2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescr2.Location = new System.Drawing.Point(85, 67);
            this.txtDescr2.Name = "txtDescr2";
            this.txtDescr2.ReadOnly = true;
            this.txtDescr2.Size = new System.Drawing.Size(955, 20);
            this.txtDescr2.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Descrição 2";
            // 
            // txtDescr1
            // 
            this.txtDescr1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescr1.Location = new System.Drawing.Point(85, 42);
            this.txtDescr1.Name = "txtDescr1";
            this.txtDescr1.ReadOnly = true;
            this.txtDescr1.Size = new System.Drawing.Size(955, 20);
            this.txtDescr1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Descrição 1";
            // 
            // txtValor
            // 
            this.txtValor.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSearch, "Valor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtValor.Location = new System.Drawing.Point(227, 19);
            this.txtValor.Name = "txtValor";
            this.txtValor.ReadOnly = true;
            this.txtValor.Size = new System.Drawing.Size(100, 20);
            this.txtValor.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(191, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Valor";
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(85, 19);
            this.txtData.Name = "txtData";
            this.txtData.ReadOnly = true;
            this.txtData.Size = new System.Drawing.Size(100, 20);
            this.txtData.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data";
            // 
            // dgvSelectedAlunos
            // 
            this.dgvSelectedAlunos.AllowUserToAddRows = false;
            this.dgvSelectedAlunos.AllowUserToDeleteRows = false;
            this.dgvSelectedAlunos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSelectedAlunos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelectedAlunos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14});
            this.dgvSelectedAlunos.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvSelectedAlunos.Location = new System.Drawing.Point(0, 500);
            this.dgvSelectedAlunos.Name = "dgvSelectedAlunos";
            this.dgvSelectedAlunos.ReadOnly = true;
            this.dgvSelectedAlunos.Size = new System.Drawing.Size(1052, 194);
            this.dgvSelectedAlunos.TabIndex = 15;
            this.dgvSelectedAlunos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSelectedAlunos_CellDoubleClick);
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "Username";
            this.dataGridViewTextBoxColumn8.HeaderText = "Username";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Nome";
            this.dataGridViewTextBoxColumn9.HeaderText = "Nome";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "EncEduc";
            this.dataGridViewTextBoxColumn10.HeaderText = "Enc. Educação";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "Morada";
            this.dataGridViewTextBoxColumn11.HeaderText = "Morada";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "CPostal";
            this.dataGridViewTextBoxColumn12.HeaderText = "Cod. Postal";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "Localidade";
            this.dataGridViewTextBoxColumn13.HeaderText = "Localidade";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "Cantao";
            this.dataGridViewTextBoxColumn14.HeaderText = "Cantão";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            // 
            // bsSearch
            // 
            this.bsSearch.DataSource = typeof(EPE.Gui.PresentationModels.SearchAlunoModel);
            // 
            // SearchAluno
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 737);
            this.Controls.Add(this.dgvSelectedAlunos);
            this.Controls.Add(this.dgvAlunos);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpFiltros);
            this.Controls.Add(this.grpMovimento);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchAluno";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SearchAluno";
            this.Load += new System.EventHandler(this.SearchAluno_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlunos)).EndInit();
            this.panel1.ResumeLayout(false);
            this.grpFiltros.ResumeLayout(false);
            this.grpFiltros.PerformLayout();
            this.grpMovimento.ResumeLayout(false);
            this.grpMovimento.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedAlunos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSearch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvAlunos;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnFinishAndReturn;
        private System.Windows.Forms.GroupBox grpFiltros;
        private System.Windows.Forms.TextBox txtCodPostal;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtCantao;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtLocalidade;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtEndereco;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtEncEdu;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtNomeAluno;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox grpMovimento;
        private System.Windows.Forms.TextBox txtDescr3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDescr2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDescr1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtValor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource bsSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridView dgvSelectedAlunos;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
    }
}