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

namespace projetoLoja.Presentation.UPDATE
{
    public partial class frmUpdateProduct : Form
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
        public frmUpdateProduct(string updProdID, string updProdName, string updProdDesc, string updProdCat, string updProdPrice, string updProdDate)
        {
            InitializeComponent();
            LoadCat();
            SetComboBoxSelection(updProdCat);

            txtProdID.Text = updProdID;
            txtName.Text = updProdName;
            cbxCategory.SelectedItem = updProdCat;
            txtDesc.Text = updProdDesc;
            txtPrice.Text = updProdPrice;
            txtDate.Text = updProdDate;
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

        private void SetComboBoxSelection(string updJob)
        {
            // Itera pelos itens do ComboBox para encontrar o item que contém o cargo (updJob)
            foreach (var item in cbxCategory.Items)
            {
                if (item.ToString().Contains(updJob))  // Verifica se o nome do cargo está na string
                {
                    cbxCategory.SelectedItem = item;  // Seleciona o item correspondente
                    break;
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            edit = true;
            btnEdit.Enabled = false;
            txtName.Enabled = true;
            cbxCategory.Enabled = true;
            txtDesc.Enabled = true;
            txtPrice.Enabled = true;
            btnUpdate.Enabled = true;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtDesc.Text != "" && txtPrice.Text != "" && edit == true)
            {
                btnUpdate.Enabled = true;
            }
            else
            {
                btnUpdate.Enabled = false;
            }
        }

        private void txtDesc_TextChanged(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtDesc.Text != "" && txtPrice.Text != "" && edit == true)
            {
                btnUpdate.Enabled = true;
            }
            else
            {
                btnUpdate.Enabled = false;
            }
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtDesc.Text != "" && txtPrice.Text != "" && edit == true)
            {
                btnUpdate.Enabled = true;
            }
            else
            {
                btnUpdate.Enabled = false;
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool operSuccess = false;
            string selectedItem = cbxCategory.SelectedItem.ToString();
            string[] parts = selectedItem.Split('-');

            int prodID = int.Parse(txtProdID.Text);
            string prodName = txtName.Text;
            int category = int.Parse(parts[0].Trim());
            string desc = txtDesc.Text;
            decimal price = decimal.Parse(txtPrice.Text);
            Ctrl ctrl = new Ctrl();


            operSuccess = ctrl.updateProduct(prodID, prodName, category, desc, price);
            string msg = ctrl.ctrlMessage;
            MessageBox.Show(msg);
            btnEdit.Enabled = true;
            btnUpdate.Enabled = false;
            txtName.Enabled = false;
            cbxCategory.Enabled = false;
            txtDesc.Enabled = false;
            txtPrice.Enabled = false;
        }

        private void frmUpdateProduct_Load(object sender, EventArgs e)
        {
            btnReturn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReturn.Width, btnReturn.Height, 40, 40));
            btnUpdate.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnUpdate.Width, btnUpdate.Height, 40, 40));
            btnEdit.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnEdit.Width, btnEdit.Height, 40, 40));
        }
    }
}
