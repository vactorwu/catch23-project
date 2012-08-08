namespace LYC.UI
{
    partial class YGGL
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.xtsb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yhdm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yhzm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yhmc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yhkl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.xtsb,
            this.yhdm,
            this.yhzm,
            this.yhmc,
            this.yhkl});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(548, 253);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 308);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "员工查询";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(93, 308);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 2;
            // 
            // xtsb
            // 
            this.xtsb.Frozen = true;
            this.xtsb.HeaderText = "系统识别";
            this.xtsb.Name = "xtsb";
            this.xtsb.ReadOnly = true;
            // 
            // yhdm
            // 
            this.yhdm.Frozen = true;
            this.yhdm.HeaderText = "用户代码";
            this.yhdm.Name = "yhdm";
            this.yhdm.ReadOnly = true;
            // 
            // yhzm
            // 
            this.yhzm.Frozen = true;
            this.yhzm.HeaderText = "用户所在";
            this.yhzm.Name = "yhzm";
            this.yhzm.ReadOnly = true;
            // 
            // yhmc
            // 
            this.yhmc.Frozen = true;
            this.yhmc.HeaderText = "用户名称";
            this.yhmc.Name = "yhmc";
            this.yhmc.ReadOnly = true;
            // 
            // yhkl
            // 
            this.yhkl.Frozen = true;
            this.yhkl.HeaderText = "用户口令";
            this.yhkl.Name = "yhkl";
            this.yhkl.ReadOnly = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(245, 308);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // YGGL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 409);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "YGGL";
            this.Text = "员工管理";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn xtsb;
        private System.Windows.Forms.DataGridViewTextBoxColumn yhdm;
        private System.Windows.Forms.DataGridViewTextBoxColumn yhzm;
        private System.Windows.Forms.DataGridViewTextBoxColumn yhmc;
        private System.Windows.Forms.DataGridViewTextBoxColumn yhkl;
        private System.Windows.Forms.Button button2;
    }
}