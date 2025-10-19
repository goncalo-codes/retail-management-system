using projetoLoja.Presentation.CONSULT;
using projetoLoja.Presentation.REGISTER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projetoLoja.Presentation
{
    public partial class frmEmployee : Form
    {
        public int UserID { get; set; }  // Propriedade para armazenar o UserID
        public frmEmployee()
        {
            InitializeComponent();
        }

        private void clientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRegisterClient frmRegCli = new frmRegisterClient();
            frmRegCli.ShowDialog();
        }

        private void vendaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRegisterSale frmRegisterSale = new frmRegisterSale(UserID, false);
            frmRegisterSale.ShowDialog();
        }

        private void stockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool aut = false;

            frmStock frmStock = new frmStock(aut);
            frmStock.ShowDialog();
        }

        private void saleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmConsultSale frmConsultSale = new frmConsultSale();
            frmConsultSale.ShowDialog();
        }

        private void clientToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bool aut = false;

            frmConsultClient frmConsultClient = new frmConsultClient(aut);
            frmConsultClient.ShowDialog();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.Show();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.Show();
            this.Close();
        }
    }
}
