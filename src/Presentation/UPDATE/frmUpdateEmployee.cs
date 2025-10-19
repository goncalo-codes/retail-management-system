using projetoLoja.Control;
using projetoLoja.Presentation.CONSULT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projetoLoja.Presentation
{
    public partial class frmUpdateEmployee : Form
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
        public frmUpdateEmployee(string updUserID, string updEmpID, string updFullName, string updTel, string updAdd, string updJob, string updSalary, string updDate, string updUsername, string updEmail, string updPassword, string updStatus)
        {
            InitializeComponent();
            LoadJobDepartments();
            SetComboBoxSelection(updJob);

            txtUserID.Text = updUserID;
            txtEmpID.Text = updEmpID;
            txtName.Text = updFullName;
            mskTel.Text = updTel;
            txtAddress.Text = updAdd;
            txtSalary.Text = updSalary;
            txtDate.Text = updDate;
            txtLogin.Text = updUsername;
            txtEmail.Text = updEmail;
            txtPassword.Text = updPassword;
            if (updStatus == "Ativo")
            {
                rdbAtivo.Checked = true;
            }
            else
            {
                rdbInativo.Checked = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            edit = true;
            btnEdit.Enabled = false;
            btnUpdate.Enabled = true;
            txtName.Enabled = true;
            mskTel.Enabled = true;
            txtEmail.Enabled = true;
            txtAddress.Enabled = true;
            cbxJob.Enabled = true;
            txtSalary.Enabled = true;
            txtLogin.Enabled = true;
            txtPassword.Enabled = true;
            gbxStatus.Enabled = true;
        }
        private void LoadJobDepartments()
        {
            try
            {
                Ctrl ctrl = new Ctrl();
                List<string[]> jobDepartments = ctrl.loadJobDepartments();

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
        private void SetComboBoxSelection(string updJob)
        {
            // Itera pelos itens do ComboBox para encontrar o item que contém o cargo (updJob)
            foreach (var item in cbxJob.Items)
            {
                if (item.ToString().Contains(updJob))  // Verifica se o nome do cargo está na string
                {
                    cbxJob.SelectedItem = item;  // Seleciona o item correspondente
                    break;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string selectedItem = cbxJob.SelectedItem.ToString();
            string[] parts = selectedItem.Split('-');
            

            bool operSuccess = false;

            edit = false;
            int userID = int.Parse(txtUserID.Text);
            int empID = int.Parse(txtEmpID.Text);
            string fullName = txtName.Text;
            string tel = mskTel.Text;
            string add = txtAddress.Text;
            int jobID = int.Parse(parts[0].Trim());
            float salary = float.Parse(txtSalary.Text);
            string loginName = txtLogin.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string status = "";
            btnEdit.Enabled = true;

            if (rdbAtivo.Checked)
            {
                status = "Active";
            }
            else
            {
                status = "Inactive";
            }

            if (!email.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email containing '@'.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Ctrl ctrl = new Ctrl();


                operSuccess = ctrl.updateEmployee(userID, empID, fullName, tel, add, jobID, salary, loginName, email, password, status);
                string msg = ctrl.ctrlMessage;
                MessageBox.Show(msg);
                btnUpdate.Enabled = false;
                txtName.Enabled = false;
                mskTel.Enabled = false;
                txtEmail.Enabled = false;
                txtAddress.Enabled = false;
                cbxJob.Enabled = false;
                txtSalary.Enabled = false;
                txtLogin.Enabled = false;
                txtPassword.Enabled = false;
                gbxStatus.Enabled = false;
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && edit == true)
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
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && edit == true)
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
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && edit == true)
            {
                btnUpdate.Enabled = true;
            }
            else
            {
                btnUpdate.Enabled = false;
            }
        }

        private void txtSalary_TextChanged(object sender, EventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && edit == true)
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
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && edit == true)
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
            if (txtLogin.Text != "" && txtPassword.Text != "" && txtEmail.Text != "" && txtName.Text != "" && mskTel.Text != "" && txtAddress.Text != "" && cbxJob.Text != "" && txtSalary.Text != "" && edit == true)
            {
                btnUpdate.Enabled = true;
            }
            else
            {
                btnUpdate.Enabled = false;
            }
        }

        private void frmUpdateEmployee_Load(object sender, EventArgs e)
        {
            btnReturn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReturn.Width, btnReturn.Height, 40, 40));
            btnUpdate.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnUpdate.Width, btnUpdate.Height, 40, 40));
            btnEdit.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnEdit.Width, btnEdit.Height, 40, 40));
        }
    }
}
