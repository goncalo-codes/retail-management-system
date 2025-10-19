using projetoLoja.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace projetoLoja.Presentation.UPDATE
{
    public partial class frmUpdateStock : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn
           (
               int nLeft,
               int nTop,
               int nRight,
               int nBottom,
               int nWidthEllipse,
               int nHeigthEllipse
           );

        bool edit = false;
        public frmUpdateStock(string prdID, string sizeID, string sizeName, string prdName, string qtd)
        {
            InitializeComponent();

            txtProdID.Text = prdID;
            txtSizeID.Text = sizeID;
            txtSize.Text = sizeName;
            txtPrdName.Text = prdName;
            txtCrtQtd.Text = qtd;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            edit = true;
            btnUpdate.Enabled = true;
            txtNewQtd.Enabled = true;
            btnEdit.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool operSuccess = false;

            int prdID = int.Parse(txtProdID.Text);
            int sizeID = int.Parse(txtSizeID.Text);
            int newQtd = int.Parse(txtNewQtd.Text);
            DateTime dataUpdt = DateTime.Now;
            Ctrl ctrl = new Ctrl();


            operSuccess = ctrl.updateStock(prdID, sizeID, newQtd, dataUpdt);
            string msg = ctrl.ctrlMessage;
            MessageBox.Show(msg);
            btnUpdate.Enabled = false;
            txtNewQtd.Enabled= false;
            btnEdit.Enabled= true;
        }

        private void txtNewQtd_TextChanged(object sender, EventArgs e)
        {
            if (txtNewQtd.Text != "" && edit == true)
            {
                btnUpdate.Enabled =true;
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUpdateStock_Load(object sender, EventArgs e)
        {
            btnEdit.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnEdit.Width, btnEdit.Height, 40, 40));
            btnReturn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReturn.Width, btnReturn.Height, 40, 40));
            btnUpdate.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnUpdate.Width, btnUpdate.Height, 40, 40));
        }
    }
}
