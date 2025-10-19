using projetoLoja.Control;
using projetoLoja.Presentation.REGISTER;
using projetoLoja.Presentation.UPDATE;
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
using static projetoLoja.Presentation.CONSULT.frmStock;

namespace projetoLoja.Presentation.CONSULT
{
    public partial class frmStock : Form
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

        public frmStock(bool aut)
        {
            InitializeComponent();
            if (!aut)
            {
                btnRefresh.Visible = false;
                btnAdd.Visible = false;
                btnDelete.Visible = false;
                btnUpdate.Visible = false;
            }
            else
            {
                btnRefresh.Visible = true;
                btnAdd.Visible = true;
                btnDelete.Visible = true;
                btnUpdate.Visible = true;
            }
        }

        public class Stock
        {
            public int prdID;
            public int sizeID;
            public string sizeName;
            public string prdName;
            public int qtd;
            public DateTime dateUpdt;
        }

        public void getStock()
        {
            Stock[] stocks = new Stock[0];
            Ctrl ctrl = new Ctrl();

            bool operSucess = ctrl.getStockList(ref stocks);

            lvStockData.Clear();
            
            lvStockData.Columns.Add("sizeID", 70);
            lvStockData.Columns.Add("productID", 50);
            lvStockData.Columns.Add("product", 150);
            lvStockData.Columns.Add("size", 50);
            lvStockData.Columns.Add("qtd", 50);
            lvStockData.Columns.Add("date last update", 150);
            lvStockData.View = View.Details;

            if (operSucess)
            {
                if (ctrl.ctrlExist)
                {
                    // fill the lv
                    for (int i = 0; i < stocks.Length; i++)
                    {
                        string[] record = {stocks[i].sizeID.ToString(), stocks[i].prdID.ToString(),  stocks[i].prdName, stocks[i].sizeName, stocks[i].qtd.ToString(), stocks[i].dateUpdt.ToString()};

                        var listViewItem = new ListViewItem(record);
                        lvStockData.Items.Add(listViewItem);
                    }
                    MessageBox.Show(ctrl.ctrlMessage, "List Credentials", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show(ctrl.ctrlMessage, "List Credentials", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
                MessageBox.Show(ctrl.ctrlMessage, "List Credentials", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmRegisterStock frmRegisterStock = new frmRegisterStock();
            frmRegisterStock.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            getStock();
        }

        private void frmStock_Load(object sender, EventArgs e)
        {
            btnRefresh_Click(sender, e);

            btnRefresh.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnRefresh.Width, btnRefresh.Height, 40, 40));
            btnUpdate.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnUpdate.Width, btnUpdate.Height, 40, 40));
            btnAdd.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnAdd.Width, btnAdd.Height, 40, 40));
            btnDelete.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDelete.Width, btnDelete.Height, 40, 40));
            btnReturn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReturn.Width, btnReturn.Height, 40, 40));
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvStockData.SelectedItems.Count > 0)
            {
                string prdID = lvStockData.SelectedItems[0].SubItems[0].Text;
                string sizeID = lvStockData.SelectedItems[0].SubItems[1].Text;
                string sizeName = lvStockData.SelectedItems[0].SubItems[2].Text;
                string prdName = lvStockData.SelectedItems[0].SubItems[3].Text;
                string qtd = lvStockData.SelectedItems[0].SubItems[4].Text;

                frmUpdateStock frmUpdateStock = new frmUpdateStock(prdID, sizeID, sizeName, prdName, qtd);
                frmUpdateStock.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select a stock row to update!");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool operSucess = false;

            // identify the username in a LVrecord
            if (lvStockData.SelectedItems.Count > 0)
            {
                int delPrdID = int.Parse(lvStockData.SelectedItems[0].SubItems[1].Text);
                int delSizeID = int.Parse(lvStockData.SelectedItems[0].SubItems[0].Text);

                DialogResult result = MessageBox.Show("Do you want to delete this record?", "Record delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Ctrl ctrl = new Ctrl();
                    operSucess = ctrl.stockDelete(delPrdID, delSizeID);

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
                MessageBox.Show("Select a stock row to delete!");
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
