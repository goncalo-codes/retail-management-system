using projetoLoja.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System.Runtime.InteropServices;

namespace projetoLoja.Presentation.REGISTER
{
    public partial class frmRegisterStock : Form
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

        Ctrl ctrl = new Ctrl();
        public frmRegisterStock()
        {
            InitializeComponent();
            loadProducts();
            loadSizes();
        }

        private void loadProducts ()
        {
            try
            {
                // Obtém os dados como uma lista de arrays
                
                List<string[]> productsData = ctrl.loadProducts();

                // Adiciona os itens ao ComboBox no formato desejado
                foreach (string[] productData in productsData)
                {
                    string displayText = $"{productData[0]} - {productData[1]} - {productData[2]}€";
                    cbxProduct.Items.Add(displayText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar departamentos: " + ex.Message);
            }
        }

        private void loadSizes()
        {
            try
            {
                // Obtém os dados como uma lista de arrays

                List<string[]> sizesData = ctrl.loadSize();

                // Adiciona os itens ao ComboBox no formato desejado
                foreach (string[] sizeData in sizesData)
                {
                    string displayText = $"{sizeData[0]} - {sizeData[1]}";
                    cbxSize.Items.Add(displayText);
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
            string selectedItem = cbxProduct.SelectedItem.ToString();
            string[] parts = selectedItem.Split('-');

            string selectedItem2 = cbxSize.SelectedItem.ToString();
            string[] parts2 = selectedItem2.Split('-');

            int catID = int.Parse(parts2[0].Trim());
            int prodID = int.Parse(parts[0].Trim());
            int quant = int.Parse(txtQtd.Text);
            DateTime dateAdded = DateTime.Now;

            ctrl.registStock(prodID, catID,quant, dateAdded);
            MessageBox.Show(ctrl.ctrlMessage, "Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnRegister.Enabled = false;
            cbxProduct.SelectedIndex = -1;
            cbxSize.SelectedIndex = -1;
            txtQtd.Clear();
        }

        private void txtQtd_TextChanged(object sender, EventArgs e)
        {
            if (txtQtd.Text != "" && cbxProduct.SelectedIndex != -1)
            {
                btnRegister.Enabled = true;
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void frmRegisterStock_Load(object sender, EventArgs e)
        {
            btnReturn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReturn.Width, btnReturn.Height, 40, 40));
            btnRegister.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnRegister.Width, btnRegister.Height, 40, 40));
        }
    }
}
