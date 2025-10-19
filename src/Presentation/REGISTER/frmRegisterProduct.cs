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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace projetoLoja.Presentation.REGISTER
{
    public partial class frmRegisterProduct : Form
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

        public frmRegisterProduct()
        {
            InitializeComponent();
            LoadCat();
        }

        private void LoadCat()
        {
            try
            {
                // Obtém os dados como uma lista de arrays
                Ctrl ctrl = new Ctrl();
                List<string[]> categories = ctrl.loadCat();

                // Adiciona os itens ao ComboBox no formato desejado
                foreach (string[] category in categories)
                {
                    string displayText = $"{category[0]} - {category[1]}";
                    cbxCategory.Items.Add(displayText);
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
            string selectedItem = cbxCategory.SelectedItem.ToString();
            string[] parts = selectedItem.Split('-');
            int cat = int.Parse(parts[0].Trim());

            string name = txtName.Text;
            string desc = txtDesc.Text;
            float price = float.Parse(txtPrice.Text);
            DateTime date = DateTime.Now;
            Ctrl ctrl = new Ctrl();

            bool operSuccess = ctrl.accessingUniqueProduct(name);

            if (operSuccess)
            {
                if (!ctrl.ctrlExist)
                {
                    ctrl.registProduct(name, desc, cat, price, date);
                    MessageBox.Show(ctrl.ctrlMessage, "Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtName.Clear();
                    txtDesc.Clear();
                    cbxCategory.SelectedIndex = -1;
                    txtPrice.Clear();
                }
                else
                    MessageBox.Show(ctrl.ctrlMessage, "Application", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtDesc.Text != "" && txtPrice.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void txtDesc_TextChanged(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtDesc.Text != "" && txtPrice.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtDesc.Text != "" && txtPrice.Text != "")
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void frmRegisterProduct_Load(object sender, EventArgs e)
        {
            btnReturn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReturn.Width, btnReturn.Height, 40, 40));
            btnRegister.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnRegister.Width, btnRegister.Height, 40, 40));
        }
    }  
}

