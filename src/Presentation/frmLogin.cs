using projetoLoja.Control;
using projetoLoja.Presentation;
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

namespace projetoLoja
{
    public partial class frmLogin : Form
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

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            btnLogin.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnLogin.Width, btnLogin.Height, 40, 40));
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            frmRegisterClient frmReg = new frmRegisterClient();
            frmReg.Show();
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string loginName = txtLogin.Text;
            string password = txtPassword.Text;

            Ctrl ctrl = new Ctrl();
            ctrl.accessingCredentials(loginName, password);

            if (ctrl.ctrlEmpStatus == "Inactive")
            {
                MessageBox.Show("This employee is inactive!");
            }
            else
            {
                MessageBox.Show(ctrl.ctrlMessage, "Projeto Aplicação", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (ctrl.ctrlExist)
                {
                    if (ctrl.ctrlUserType == "Client")
                    {
                        // login into the client application
                        MessageBox.Show("Welcome " + ctrl.ctrlUserName + "!", "Projeto Aplicação", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Passar o UserID para o frmClient
                        frmClient frmCliente = new frmClient(true);
                        frmCliente.UserID = ctrl.ctrlUserIDLog;  // Passando o UserID para o frmClient
                        this.Hide();
                        frmCliente.Show();
                    }
                    else if (ctrl.ctrlUserType == "Employee")
                    {
                        frmEmployee frmEmployee = new frmEmployee();
                        frmEmployee.UserID = ctrl.ctrlEmpIDLog;  // Passando o UserID para o frmEmp


                        MessageBox.Show("Welcome employee!");
                        this.Hide();
                        frmEmployee.Show();

                    }
                    else if (ctrl.ctrlUserType == "Adm")
                    {
                        frmAdm frmAdm = new frmAdm();
                        frmAdm.UserID = ctrl.ctrlEmpIDLog;// Passando o UserID para o frmAdm

                        MessageBox.Show("Welcome ADM!");
                        this.Hide();
                        frmAdm.Show();
                    }
                    else if (ctrl.ctrlUserType == "Manager")
                    {
                        frmManager frmManager = new frmManager();
                        frmManager.UserID = ctrl.ctrlEmpIDLog;  // Passando o UserID para o frmManager
                        MessageBox.Show("Welcome manager!");
                        this.Hide();
                        frmManager.Show();
                    }
                }
                else
                {
                    // do not enter the application
                    txtLogin.Clear();
                    txtPassword.Text = "";
                }
            }
        }

        private void txtLogin_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "")
            {
                btnLogin.Enabled = true;
            }
            else 
            {
                btnLogin.Enabled = false;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text != "" && txtLogin.Text != "")
            {
                btnLogin.Enabled = true;
            }
            else 
            { 
                btnLogin.Enabled = false; 
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmRegisterClient frmReg = new frmRegisterClient();
            frmReg.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    
    }
}
