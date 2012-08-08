using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LYC.Model;
using LYC.BLL;

namespace LYC.UI
{
    public partial class YGGL : Form
    {
        public YGGL()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.dataGridView1.Rows.Count != 0)
            {
                for (int i = 0; i < this.dataGridView1.Rows.Count;)
                {
                    this.dataGridView1.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Pink;
                    i = i + 2;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string yhdm = this.textBox1.Text;

            if (yhdm != "") 
            {
                LYC.BLL.User user = new LYC.BLL.User();
                LYC.Model.UserInfo userinfo = new LYC.Model.UserInfo();
                userinfo =  user.getUser(yhdm);
            }
           
        }

    
    }
}
