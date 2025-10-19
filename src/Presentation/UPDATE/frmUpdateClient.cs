using projetoLoja.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace projetoLoja.Presentation
{
    public partial class frmUpdateClient : Form
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
        public frmUpdateClient(string updUserID, string updCliID, string updNIF, string updFullName, string updTel, string updAdd, string updUsername, string updEmail, string updPassword)
        {
            InitializeComponent();
            txtUserID.Text = updUserID;
            txtCliID.Text = updCliID;
            txtName.Text = updFullName;
            mskTel.Text = updTel;
            mskNIF.Text = updNIF;
            txtAddress.Text = updAdd;
            txtLogin.Text = updUsername;
            txtEmail.Text = updEmail;
            txtPassword.Text = updPassword;
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            edit = true;
            btnUpdate.Enabled = true;
            txtName.Enabled = true;
            mskTel.Enabled = true;
            mskNIF.Enabled = true;
            txtAddress.Enabled = true;
            txtLogin.Enabled = true;
            txtEmail.Enabled = true;
            txtPassword.Enabled = true;
            btnEdit.Enabled = false;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" &&  txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && mskNIF.Text != "" && txtAddress.Text != "" && edit == true)
            {
                btnUpdate.Enabled = true;
            }
            else
            {
                btnUpdate.Enabled = false;
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && mskNIF.Text != "" && txtAddress.Text != "" && edit == true)
            {
                btnUpdate.Enabled = true;
            }
            else
            {
                btnUpdate.Enabled = false;
            }
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && mskNIF.Text != "" && txtAddress.Text != "" && edit == true)
            {
                btnUpdate.Enabled = true;
            }
            else
            {
                btnUpdate.Enabled = false;
            }
        }

        private void txtLogin_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && mskNIF.Text != "" && txtAddress.Text != "" && edit == true)
            {
                btnUpdate.Enabled = true;
            }
            else
            {
                btnUpdate.Enabled = false;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && mskNIF.Text != "" && txtAddress.Text != "" && edit == true)
            {
                btnUpdate.Enabled = true;
            }
            else
            {
                btnUpdate.Enabled = false;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool operSuccess = false;

            btnEdit.Enabled = true;
            edit = false;
            int userID = int.Parse(txtUserID.Text);
            int cliID = int.Parse(txtCliID.Text);
            string fullName = txtName.Text;
            string NIF = mskNIF.Text;
            string tel = mskTel.Text;
            string add = txtAddress.Text;
            string loginName = txtLogin.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            if (!email.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email containing '@'.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                Ctrl ctrl = new Ctrl();
                operSuccess = ctrl.updateClient(userID, cliID, NIF, fullName, tel, add, loginName, email, password);
                string msg = ctrl.ctrlMessage;

                MessageBox.Show(msg);
                btnUpdate.Enabled = false;
                txtName.Enabled = false;
                mskNIF.Enabled = false;
                mskTel.Enabled = false;
                txtEmail.Enabled = false;
                txtAddress.Enabled = false;
                txtLogin.Enabled = false;
                txtPassword.Enabled = false;
            }
        }

        private void frmUpdateClient_Load(object sender, EventArgs e)
        {
            btnReturn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReturn.Width, btnReturn.Height, 40, 40));
            btnUpdate.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnUpdate.Width, btnUpdate.Height, 40, 40));
            btnEdit.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnEdit.Width, btnEdit.Height, 40, 40));
        }
    }
}
