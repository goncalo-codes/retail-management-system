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
using static projetoLoja.Presentation.CONSULT.frmConsultProduct;
using static projetoLoja.Presentation.CONSULT.frmConsultSale;

namespace projetoLoja.Presentation.CONSULT
{
    public partial class frmConsultSale : Form
    {
        public frmConsultSale()
        {
            InitializeComponent();
            getSale();
        }

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

        public class Sale
        {
            public int saleID;
            public int clientID;
            public int employeeID;
            public int productID;
            public int sizeID;
            public string clientName;
            public string employeeName;
            public string productName;
            public int quantity;
            public string sizeName;
            public DateTime saleDate;
            public decimal total;
        }

        public void getSale(string filter = "")
        {
            Sale[] sales = new Sale[0];
            Ctrl ctrl = new Ctrl();

            bool operSucess = ctrl.getSaleList(ref sales);

            lvProductData.Clear();

            lvProductData.Columns.Add("saleID", 50);
            lvProductData.Columns.Add("clientID", 50);
            lvProductData.Columns.Add("employeeID", 50);
            lvProductData.Columns.Add("productID", 50);
            lvProductData.Columns.Add("sizeID", 50);
            lvProductData.Columns.Add("clientName", 150);
            lvProductData.Columns.Add("employeeName", 150);
            lvProductData.Columns.Add("productName", 150);
            lvProductData.Columns.Add("quantity", 50);
            lvProductData.Columns.Add("sizeName", 50);
            lvProductData.Columns.Add("saleDate", 150);
            lvProductData.Columns.Add("total", 50);
            lvProductData.View = View.Details;

            if (operSucess)
            {
                if (ctrl.ctrlExist)
                {
                    // Aplica o filtro
                    Sale[] filteredSales;
                    if (string.IsNullOrEmpty(filter))
                    {
                        filteredSales = sales;
                    }
                    else
                    {
                        filteredSales = sales.Where(c =>
                            (c.saleID.ToString() != null && c.saleID.ToString().IndexOf(filter) >= 0) ||
                            (c.clientID.ToString() != null && c.clientID.ToString().IndexOf(filter) >= 0) ||
                            (c.employeeID.ToString() != null && c.employeeID.ToString().IndexOf(filter) >= 0) ||
                            (c.productID.ToString() != null && c.productID.ToString().IndexOf(filter) >= 0) ||
                            (c.clientName != null && c.clientName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (c.employeeName != null && c.employeeName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (c.productName != null && c.productName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                        ).ToArray();
                    }

                    // Preenche o ListView
                    foreach (var product in filteredSales)
                    {
                        string[] record =
                        {
                            product.saleID.ToString(),
                            product.clientID.ToString(),
                            product.employeeID.ToString(),
                            product.productID.ToString(),
                            product.sizeID.ToString(),
                            product.clientName,
                            product.employeeName,
                            product.productName,
                            product.quantity.ToString(),
                            product.sizeName,
                            product.saleDate.ToString(),
                            product.total.ToString() + ("€")
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
                int saleId = int.Parse(lvProductData.SelectedItems[0].SubItems[0].Text);

                DialogResult result = MessageBox.Show("Do you want to delete this record?", "Record delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Ctrl ctrl = new Ctrl();
                    operSucess = ctrl.saleDelete(saleId);

                    if (operSucess)
                    {
                        MessageBox.Show(ctrl.ctrlMessage, "Record Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        getSale();
                    }
                    else
                        MessageBox.Show(ctrl.ctrlMessage, "Record Delete", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                MessageBox.Show("Select a sale to delete!");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            getSale(txtSearch.Text);
        }

        private void frmConsultSale_Load(object sender, EventArgs e)
        {
            btnDelete.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDelete.Width, btnDelete.Height, 40, 40));
            btnReturn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReturn.Width, btnReturn.Height, 40, 40));
        }
    }
}
