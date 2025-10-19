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

namespace projetoLoja.Presentation
{
    public partial class frmRegisterClient : Form
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

        public frmRegisterClient()
        {
            InitializeComponent();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtLogin_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && mskNIF.Text != "" && txtAddress.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && mskNIF.Text != "" && txtAddress.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && mskNIF.Text != "" && txtAddress.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void txtRetypePassword_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && mskNIF.Text != "" && txtAddress.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string loginName = txtLogin.Text;
            string email = txtEmail.Text;
            string userType = "Client";
            string nif = mskNIF.Text;
            
            if (!email.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email containing '@'.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Ctrl ctrl = new Ctrl();
                bool operSuccess = ctrl.accessingUniqueData(loginName, email);


                if (operSuccess) // if succeed
                {
                    if (!ctrl.ctrlExist)
                    {
                        // criar o login na BD
                        string password = txtPassword.Text;
                        string retypePassword = txtRetypePassword.Text;
                        string name = txtName.Text;
                        string tel = mskTel.Text;
                        string add = txtAddress.Text;


                        operSuccess = ctrl.registCredentials(loginName, email, password, retypePassword, userType);


                        if (operSuccess)
                        {
                            ctrl.registClient(name, tel, add, nif);
                            MessageBox.Show(ctrl.ctrlMessage, "Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtName.Clear();
                            mskTel.Clear();
                            mskNIF.Clear();
                            txtEmail.Clear();
                            txtAddress.Clear();
                            txtLogin.Clear();
                            txtPassword.Clear();
                            txtRetypePassword.Clear();
                        }
                            
                        else
                            MessageBox.Show(ctrl.ctrlMessage, "Application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show(ctrl.ctrlMessage, "Application", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && mskNIF.Text != "" && txtAddress.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && mskNIF.Text != "" && txtAddress.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void frmRegisterClient_Load(object sender, EventArgs e)
        {
            btnReturn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReturn.Width, btnReturn.Height, 40, 40));
            btnRegister.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnRegister.Width, btnRegister.Height, 40, 40));
        }
    }
}
