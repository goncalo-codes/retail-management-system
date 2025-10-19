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
    
    public partial class frmManager : Form
    {
        public int UserID { get; set; }  // Propriedade para armazenar o UserID
        public frmManager()
        {
            InitializeComponent();
        }

        private void stockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool aut = true;

            frmStock frmStock = new frmStock(aut);
            frmStock.ShowDialog();
        }

        private void clientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRegisterClient frmRegisterClient = new frmRegisterClient();
            frmRegisterClient.ShowDialog();
        }

        private void productToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRegisterProduct frmRegisterProduct = new frmRegisterProduct();
            frmRegisterProduct.ShowDialog();
        }

        private void saleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRegisterSale frmRegisterSale = new frmRegisterSale(UserID, false);
            frmRegisterSale.ShowDialog();
        }

        private void clientToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bool aut = true;

            frmConsultClient frmConsultClient = new frmConsultClient(aut);
            frmConsultClient.ShowDialog();
        }

        private void saleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmConsultSale frmConsultSale = new frmConsultSale();   
            frmConsultSale.ShowDialog();
        }

        private void productToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmConsultProduct frmConsultProduct = new frmConsultProduct();
            frmConsultProduct.ShowDialog();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.Show();
            this.Close();
        }
    }
}
