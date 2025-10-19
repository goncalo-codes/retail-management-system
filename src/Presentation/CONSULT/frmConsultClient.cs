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
using static projetoLoja.Presentation.CONSULT.frmConsultEmployee;


namespace projetoLoja.Presentation.CONSULT
{
    public partial class frmConsultClient : Form
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

        public frmConsultClient(bool aut)
        {
            InitializeComponent();
                
            if (!aut)
            {
                btnRefresh.Visible = false;
                btnDelete.Visible = false;
                btnUpdate.Visible = false;
            }
            else
            {
                btnRefresh.Visible = true;
                btnDelete.Visible = true;
                btnUpdate.Visible = true;
            }
        }

        public class Client
        {
            public int userID;
            public int clientID;
            public string fullName;
            public string NIF;
            public string tel;
            public string address;
            public string username;
            public string email;
            public string passwd;
        }

        public void getClient(string filter = "")
        {
            Client[] clients = new Client[0];
            Ctrl ctrl = new Ctrl();

            bool operSucess = ctrl.getClientList(ref clients);

            lvClientData.Clear();

            lvClientData.Columns.Add("UserID", 50);
            lvClientData.Columns.Add("ClientID", 50);
            lvClientData.Columns.Add("Name", 150);
            lvClientData.Columns.Add("NIF", 150);
            lvClientData.Columns.Add("Tel", 150);
            lvClientData.Columns.Add("Address", 150);
            lvClientData.Columns.Add("Username", 150);
            lvClientData.Columns.Add("Email", 150);
            lvClientData.Columns.Add("Password", 100);
            lvClientData.View = View.Details;

            if (operSucess)
            {
                if (ctrl.ctrlExist)
                {
                    // Aplica o filtro
                    Client[] filteredClients; 
                    if (string.IsNullOrEmpty(filter))
                    {
                        filteredClients = clients;
                    }
                    else
                    {
                        filteredClients = clients.Where(c =>
                            (c.clientID.ToString() != null && c.clientID.ToString().IndexOf(filter) >=0) ||
                            (c.fullName != null && c.fullName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (c.NIF != null && c.NIF.IndexOf(filter) >= 0) ||
                            (c.username != null && c.username.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (c.email != null && c.email.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                        ).ToArray();
                    }

                    // Preenche o ListView
                    foreach (var client in filteredClients)
                    {
                        string[] record =
                        {
                    client.userID.ToString(),
                    client.clientID.ToString(),
                    client.fullName,
                    client.NIF,
                    client.tel,
                    client.address,
                    client.username,
                    client.email,
                    client.passwd
                };

                        var listViewItem = new ListViewItem(record);
                        lvClientData.Items.Add(listViewItem);
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Refreshed Data!");
            getClient();
        }

        private void frmConsultClient_Load(object sender, EventArgs e)
        {
            getClient();

            btnRefresh.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnRefresh.Width, btnRefresh.Height, 40, 40));
            btnUpdate.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnUpdate.Width, btnUpdate.Height, 40, 40));
            btnDelete.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDelete.Width, btnDelete.Height, 40, 40));
            btnReturn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReturn.Width, btnReturn.Height, 40, 40));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvClientData.SelectedItems.Count > 0)
            {
                string updUserID = lvClientData.SelectedItems[0].SubItems[0].Text;
                string updCliID = lvClientData.SelectedItems[0].SubItems[1].Text;
                string updFullName = lvClientData.SelectedItems[0].SubItems[2].Text;
                string updNIF = lvClientData.SelectedItems[0].SubItems[3].Text;
                string updTel = lvClientData.SelectedItems[0].SubItems[4].Text;
                string updAdd = lvClientData.SelectedItems[0].SubItems[5].Text;
                string updUsername = lvClientData.SelectedItems[0].SubItems[6].Text;
                string updEmail = lvClientData.SelectedItems[0].SubItems[7].Text;
                string updPassword = lvClientData.SelectedItems[0].SubItems[8].Text;

                frmUpdateClient frmUpdateClient = new frmUpdateClient(updUserID, updCliID, updNIF, updFullName, updTel, updAdd, updUsername, updEmail, updPassword);
                frmUpdateClient.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select a client to update!");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool operSucess = false;

            // identify the username in a LVrecord
            if (lvClientData.SelectedItems.Count > 0)
            {
                int delUserID = int.Parse(lvClientData.SelectedItems[0].SubItems[0].Text);
                int delCliID = int.Parse(lvClientData.SelectedItems[0].SubItems[1].Text);

                DialogResult result = MessageBox.Show("Do you want to delete this record?", "Record delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Ctrl ctrl = new Ctrl();
                    operSucess = ctrl.clientDelete(delUserID, delCliID);

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
                MessageBox.Show("Select a client to delete!");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text;
            getClient(searchQuery); // Atualiza a lista com base no filtro
        }
    }
}
