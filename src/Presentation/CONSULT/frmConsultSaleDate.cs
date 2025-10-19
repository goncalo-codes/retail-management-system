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
using static projetoLoja.Presentation.CONSULT.frmConsultSale;

namespace projetoLoja.Presentation.CONSULT
{
    public partial class frmConsultSaleDate : Form
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

        public frmConsultSaleDate()
        {
            InitializeComponent();
        }

        public class SaleDate
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

        public void getSaleDate(DateTime? dateTimeBeg = null, DateTime? dateTimeEnd = null)
        {
            SaleDate[] salesDate = new SaleDate[0];
            Ctrl ctrl = new Ctrl();

            bool operSucess = ctrl.getSaleList2(ref salesDate);

            lvProductData.Clear();

            lvProductData.Columns.Add("saleID", 50);
            lvProductData.Columns.Add("clientID", 50);
            lvProductData.Columns.Add("employeeID", 50);
            lvProductData.Columns.Add("productID", 50);
            lvProductData.Columns.Add("sizeID", 50);
            lvProductData.Columns.Add("clientName", 150);
            lvProductData.Columns.Add("employeeName", 150);
            lvProductData.Columns.Add("quantity", 50);
            lvProductData.Columns.Add("sizeName", 50);
            lvProductData.Columns.Add("saleDate", 150);
            lvProductData.Columns.Add("total", 80);
            lvProductData.View = View.Details;

            if (operSucess)
            {
                if (ctrl.ctrlExist)
                {
                    // Filtra os dados por data, se as datas forem fornecidas
                    var filteredSales = salesDate.Where(s =>
                        (!dateTimeBeg.HasValue || s.saleDate >= dateTimeBeg.Value) &&
                        (!dateTimeEnd.HasValue || s.saleDate <= dateTimeEnd.Value))
                        .ToArray();

                    // Preenche o ListView
                    foreach (var saleDate in filteredSales)
                    {
                        string[] record = {
                    saleDate.saleID.ToString(),
                    saleDate.clientID.ToString(),
                    saleDate.employeeID.ToString(),
                    saleDate.productID.ToString(),
                    saleDate.sizeID.ToString(),
                    saleDate.clientName,
                    saleDate.employeeName,
                    saleDate.quantity.ToString(),
                    saleDate.sizeName,
                    saleDate.saleDate.ToString("yyyy-MM-dd"),
                    saleDate.total.ToString() + ("€")
                };

                        var listViewItem = new ListViewItem(record);
                        lvProductData.Items.Add(listViewItem);
                    }
                }
                else
                {
                    MessageBox.Show(ctrl.ctrlMessage, "List Credentials", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                MessageBox.Show(ctrl.ctrlMessage, "List Credentials", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime dateTimeBeg = dtpBeg.Value.Date; // Converte para apenas a data
            DateTime dateTimeEnd = dtpEnd.Value.Date;

            if (dateTimeBeg > dateTimeEnd)
            {
                MessageBox.Show("The start data cannot be greater than an end data!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chama o método get com as datas selecionadas
            getSaleDate(dateTimeBeg, dateTimeEnd);
        }

        private void frmConsultSaleDate_Load(object sender, EventArgs e)
        {
            getSaleDate();

            btnDelete.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDelete.Width, btnDelete.Height, 40, 40));
            btnReturn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReturn.Width, btnReturn.Height, 40, 40));
            btnSearch.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSearch.Width, btnSearch.Height, 40, 40));
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
                        getSaleDate();
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

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
