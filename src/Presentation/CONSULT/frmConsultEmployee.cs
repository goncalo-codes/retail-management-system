using projetoLoja.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projetoLoja.Presentation.CONSULT
{
    public partial class frmConsultEmployee : Form
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

        public frmConsultEmployee()
        {
            InitializeComponent();
        }
        
        public class Employee
        {
            public int userID;
            public int empID;
            public string username;
            public string email;
            public string passwd;
            public string fullName;
            public string tel;
            public string address;
            public string job;
            public decimal salary;
            public DateTime dateHired;
            public string status;
        }

        public void getEmployee(string filter = "")
        {
            Employee[] employees = new Employee[0];
            Ctrl ctrl = new Ctrl();

            bool operSucess = ctrl.getEmployeeList(ref employees);

            lvEmployeeData.Clear();

            lvEmployeeData.Columns.Add("userID", 50);
            lvEmployeeData.Columns.Add("empID", 50);
            lvEmployeeData.Columns.Add("FullName", 150);
            lvEmployeeData.Columns.Add("Tel", 150);
            lvEmployeeData.Columns.Add("Address", 150);
            lvEmployeeData.Columns.Add("Job", 150);
            lvEmployeeData.Columns.Add("Salary", 150);
            lvEmployeeData.Columns.Add("DateHired", 150);
            lvEmployeeData.Columns.Add("Username", 150);
            lvEmployeeData.Columns.Add("Email", 150);
            lvEmployeeData.Columns.Add("Password", 100);
            lvEmployeeData.Columns.Add("Status", 50);
            lvEmployeeData.View = View.Details;

            if (operSucess)
            {
                if (ctrl.ctrlExist)
                {
                    // Aplica o filtro
                    Employee[] filteredEmployees; 
                    if (string.IsNullOrEmpty(filter))
                    {
                        filteredEmployees = employees;
                    }
                    else
                    {
                        filteredEmployees = employees.Where(c =>
                            (c.empID.ToString() != null && c.empID.ToString().IndexOf(filter) >= 0) ||
                            (c.fullName != null && c.fullName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (c.username != null && c.username.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (c.email != null && c.email.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                        ).ToArray();
                    }

                    // Preenche o ListView
                    foreach (var employee in filteredEmployees)
                    {
                        string[] record =
                        {
                            employee.userID.ToString(),
                            employee.empID.ToString(),
                            employee.fullName,
                            employee.tel,
                            employee.address,
                            employee.job,
                            employee.salary.ToString(),
                            employee.dateHired.ToString(),
                            employee.username,
                            employee.email,
                            employee.passwd,
                            employee.status
                    };

                        var listViewItem = new ListViewItem(record);
                        lvEmployeeData.Items.Add(listViewItem);
                    }
                }
                else
                    MessageBox.Show(ctrl.ctrlMessage, "List Credentials", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
                MessageBox.Show(ctrl.ctrlMessage, "List Credentials", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvEmployeeData.SelectedItems.Count > 0)
            {
                string updUserID = lvEmployeeData.SelectedItems[0].SubItems[0].Text;
                string updEmpID = lvEmployeeData.SelectedItems[0].SubItems[1].Text;
                string updFullName = lvEmployeeData.SelectedItems[0].SubItems[2].Text;
                string updTel = lvEmployeeData.SelectedItems[0].SubItems[3].Text;
                string updAdd = lvEmployeeData.SelectedItems[0].SubItems[4].Text;
                string updJob = lvEmployeeData.SelectedItems[0].SubItems[5].Text;
                string updSalary = lvEmployeeData.SelectedItems[0].SubItems[6].Text;
                string updDate = lvEmployeeData.SelectedItems[0].SubItems[7].Text;
                string updUsername = lvEmployeeData.SelectedItems[0].SubItems[8].Text;
                string updEmail = lvEmployeeData.SelectedItems[0].SubItems[9].Text;
                string updPassword = lvEmployeeData.SelectedItems[0].SubItems[10].Text;
                string updStatus = lvEmployeeData.SelectedItems[0].SubItems[11].Text;

                frmUpdateEmployee frmUpdateEmployee = new frmUpdateEmployee(updUserID, updEmpID, updFullName, updTel, updAdd, updJob, updSalary, updDate, updUsername, updEmail, updPassword, updStatus);
                frmUpdateEmployee.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select a employee to update!");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Refreshed Data!");
            getEmployee();
        }

        private void frmConsultEmployee_Load(object sender, EventArgs e)
        {
            getEmployee();

            btnRefresh.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnRefresh.Width, btnRefresh.Height, 40, 40));
            btnUpdate.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnUpdate.Width, btnUpdate.Height, 40, 40));
            btnDelete.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDelete.Width, btnDelete.Height, 40, 40));
            btnReturn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReturn.Width, btnReturn.Height, 40, 40));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool operSucess = false;

            // identify the username in a LVrecord
            if (lvEmployeeData.SelectedItems.Count > 0)
            {
                int delUserID = int.Parse(lvEmployeeData.SelectedItems[0].SubItems[0].Text);
                int delEmpID = int.Parse(lvEmployeeData.SelectedItems[0].SubItems[1].Text);

                DialogResult result = MessageBox.Show("Do you want to delete this record?", "Record delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Ctrl ctrl = new Ctrl();
                    operSucess = ctrl.employeeDelete(delUserID, delEmpID);

                    if (operSucess)
                    {
                        MessageBox.Show(ctrl.ctrlMessage, "Record Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnRefresh_Click(sender, e);
                    }
                    else
                        MessageBox.Show(ctrl.ctrlMessage, "Record Delete", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                MessageBox.Show("Select a employee to delete!");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            getEmployee(txtSearch.Text);
        }
    }
}
