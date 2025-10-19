using projetoLoja.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace projetoLoja.Presentation.REGISTER
{
    public partial class frmRegisterSale : Form
    {
        bool leave;
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

        Ctrl ctrl = new Ctrl();
        public frmRegisterSale(int userID, bool lv)
        {
            InitializeComponent();
            loadClients();
            int empID = userID;

            txtEmpID.Text = empID.ToString();
            leave = lv;
        }

        private void loadClients()
        {
            try
            {
                // Obtém os dados como uma lista de arrays

                List<string[]> clientsData = ctrl.loadClients();

                // Adiciona os itens ao ComboBox no formato desejado
                foreach (string[] clientData in clientsData)
                {
                    string displayText = $"{clientData[0]} - {clientData[1]} - {clientData[2]}";
                    cbxClient.Items.Add(displayText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar departamentos: " + ex.Message);

            }
        }

        private void cbxClient_SelectedIndexChaged(object sender, EventArgs e)
        {
            // Verifica se o índice selecionado não é -1 (significa que um item foi selecionado)
            if (cbxClient.SelectedIndex != -1)
            {
                btnOpenStore.Enabled = true;  // Habilita o botão
            }
            else
            {
                btnOpenStore.Enabled = false;  // Desabilita o botão, se necessário
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOpenStore_Click(object sender, EventArgs e)
        {
            string selectedItem = cbxClient.SelectedItem.ToString();
            string[] parts = selectedItem.Split('-');
            int clientID = int.Parse(parts[0].Trim());
            int employeeID = int.Parse(txtEmpID.Text);

            frmClient frmClient = new frmClient(leave);
            frmClient.UserID = clientID;  // Passando o UserID para o frmClient
            frmClient.EmpID = employeeID;

            this.Hide();
            frmClient.ShowDialog();
        }

        private void frmRegisterSale_Load(object sender, EventArgs e)
        {
            btnReturn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReturn.Width, btnReturn.Height, 40, 40));
            btnOpenStore.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnOpenStore.Width, btnOpenStore.Height, 40, 40));
        }
    }
}
