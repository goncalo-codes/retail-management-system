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
    public partial class frmRegisterEmployee : Form
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

        public frmRegisterEmployee()
        {
            InitializeComponent();
            LoadJobDepartments();
        }

        private void LoadJobDepartments()
        {
            try
            {
                // Obtém os dados como uma lista de arrays
                Ctrl ctrl = new Ctrl();
                List<string[]> jobDepartments = ctrl.loadJobDepartments();

                // Adiciona os itens ao ComboBox no formato desejado
                foreach (string[] jobDepartment in jobDepartments)
                {
                    string displayText = $"{jobDepartment[0]} - {jobDepartment[1]} - {jobDepartment[2]}";
                    cbxJob.Items.Add(displayText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar departamentos: " + ex.Message);
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string selectedItem = cbxJob.SelectedItem.ToString();
            string[] parts = selectedItem.Split('-');
            int jobID = int.Parse(parts[0].Trim());
            string status = "";

            if (rdbAtivo.Checked)
            {
                status = "Active";
            }
            else
            {
                status = "Inactive";
            }

            string loginName = txtLogin.Text;
            string email = txtEmail.Text;
            string userType = cbxLgnType.Text;

            if (!email.Contains("@"))
            {
                MessageBox.Show("Por favor, insira um e-mail válido contendo '@'.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Ctrl ctrl = new Ctrl();
                bool operSuccess = ctrl.accessingUniqueData(loginName, email);


                if (operSuccess)
                {
                    if (!ctrl.ctrlExist)
                    {
                        string password = txtPassword.Text;
                        string retypePassword = txtRetypePassword.Text;
                        string name = txtName.Text;
                        string number = mskTel.Text;
                        string add = txtAddress.Text;
                        float salary = float.Parse(txtSalary.Text);
                        DateTime hiredDate = dtpHideredDate.Value;
                        

                        operSuccess = ctrl.registCredentials(loginName, email, password, retypePassword, userType);

                        if (operSuccess)
                        {
                            ctrl.registEmployee(name, number, add, jobID, salary, hiredDate, status);
                            MessageBox.Show(ctrl.ctrlMessage, "Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtName.Clear();
                            mskTel.Clear();
                            txtEmail.Clear();
                            txtAddress.Clear();
                            cbxJob.SelectedIndex = -1;
                            txtSalary.Clear();
                            txtLogin.Clear();
                            txtPassword.Clear();
                            txtRetypePassword.Clear();
                            cbxLgnType.SelectedIndex = -1;
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
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && dtpHideredDate.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void mskTel_TextChanged(object sender, MaskInputRejectedEventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && dtpHideredDate.Text != "")
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
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && dtpHideredDate.Text != "")
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
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && dtpHideredDate.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void cbxJob_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && dtpHideredDate.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void txtSalary_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && dtpHideredDate.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void dtpHideredDate_ValueChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && dtpHideredDate.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void txtLogin_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && dtpHideredDate.Text != "")
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
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && dtpHideredDate.Text != "")
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
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtRetypePassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && dtpHideredDate.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void frmRegisterEmployee_Load(object sender, EventArgs e)
        {
            btnReturn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReturn.Width, btnReturn.Height, 40, 40));
            btnRegister.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnRegister.Width, btnRegister.Height, 40, 40));
        }
    }
}
