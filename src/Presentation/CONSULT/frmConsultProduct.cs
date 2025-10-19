using projetoLoja.Control;
using projetoLoja.Presentation.UPDATE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static projetoLoja.Presentation.CONSULT.frmConsultClient;
using static projetoLoja.Presentation.CONSULT.frmConsultEmployee;

namespace projetoLoja.Presentation.CONSULT
{
    public partial class frmConsultProduct : Form
    {
        public frmConsultProduct()
        {
            InitializeComponent();
        }
        public class Product
        {
            public int productID;
            public string productName;
            public string productDescription;
            public string productCategory;
            public decimal productPrice;
            public DateTime dateAdded;
        }

        public void getProduct(string filter = "")
        {
            Product[] products = new Product[0];
            Ctrl ctrl = new Ctrl();

            bool operSucess = ctrl.getProductList(ref products);

            lvProductData.Clear();

            lvProductData.Columns.Add("ID", 50);
            lvProductData.Columns.Add("Name", 150);
            lvProductData.Columns.Add("Description", 250);
            lvProductData.Columns.Add("Category", 100);
            lvProductData.Columns.Add("Price", 50);
            lvProductData.Columns.Add("Date", 80);
            lvProductData.View = View.Details;

            if (operSucess)
            {
                if (ctrl.ctrlExist)
                {
                    // Aplica o filtro
                    Product[] filteredProducts;
                    if (string.IsNullOrEmpty(filter))
                    {
                        filteredProducts = products;
                    }
                    else
                    {
                        filteredProducts = products.Where(c =>
                            (c.productID.ToString() != null && c.productID.ToString().IndexOf(filter) >= 0) ||
                            (c.productName != null && c.productName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                        ).ToArray();
                    }

                    // Preenche o ListView
                    foreach (var product in filteredProducts)
                    {
                        string[] record =
                        {
                            product.productID.ToString(),
                            product.productName,
                            product.productDescription,
                            product.productCategory,
                            product.productPrice.ToString(),
                            product.dateAdded.ToString()
                    };

                        var listViewItem = new ListViewItem(record);
                        lvProductData.Items.Add(listViewItem);
                    }
                }
                else
                    MessageBox.Show(ctrl.ctrlMessage, "List Credentials", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
                MessageBox.Show(ctrl.ctrlMessage, "List Credentials", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void frmConsultProduct_Load(object sender, EventArgs e)
        {
            getProduct();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Refreshed Data!");
            getProduct();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvProductData.SelectedItems.Count > 0)
            {
                string updProdID = lvProductData.SelectedItems[0].SubItems[0].Text;
                string updProdName = lvProductData.SelectedItems[0].SubItems[1].Text;
                string updProdDesc = lvProductData.SelectedItems[0].SubItems[2].Text;
                string updProdCat = lvProductData.SelectedItems[0].SubItems[3].Text;
                string updProdPrice = lvProductData.SelectedItems[0].SubItems[4].Text;
                string updProdDate = lvProductData.SelectedItems[0].SubItems[5].Text;


                frmUpdateProduct frmUpdateProduct = new frmUpdateProduct(updProdID, updProdName, updProdDesc, updProdCat, updProdPrice, updProdDate);
                frmUpdateProduct.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select a product to update!");
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool operSucess = false;

            // identify the username in a LVrecord
            if (lvProductData.SelectedItems.Count > 0)
            {
                int delProdID = int.Parse(lvProductData.SelectedItems[0].SubItems[0].Text);

                DialogResult result = MessageBox.Show("Do you want to delete this record?", "Record delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Ctrl ctrl = new Ctrl();
                    operSucess = ctrl.productDelete(delProdID);

                    if (operSucess)
                    {
                        MessageBox.Show(ctrl.ctrlMessage, "Record Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnRefresh_Click(sender, e);
                    }
                    else
                        MessageBox.Show("The product is registered in stock, so it cannot be deleted", "Record Delete", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                MessageBox.Show("Select a product to delete!");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            getProduct(txtSearch.Text);
        }
    }
}
