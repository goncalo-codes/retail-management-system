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

namespace projetoLoja.Presentation
{
    public partial class frmCart : Form
    {
        // Lista para armazenar os produtos adicionados ao carrinho
        public static List<(int? empID, int clientID, string productName, decimal price, string size, int quantity)> cartProducts = new List<(int? empID,int clientID, string productName, decimal price, string size, int quantity)>();

        Ctrl ctrl = new Ctrl();

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

        public frmCart()
        {
            InitializeComponent();

        }

        private void LoadListView()
        {
            // Adicionando colunas à ListView
            lvCart.Columns.Add("Product", 200);   // Nome do produto
            lvCart.Columns.Add("ClientID", 100);   // Nome do produto
            lvCart.Columns.Add("Size", 100);    // Tamanho do produto
            lvCart.Columns.Add("Quantity", 100); // Quantidade do produto
            lvCart.Columns.Add("Price", 80);      // Preço unitário do produto

            Controls.Add(lvCart);  // Adiciona a ListView ao formulário
        }

        // Método para exibir os produtos armazenados na ListView
        public void DisplayProducts()
        {
            lvCart.Items.Clear();  // Limpa a ListView

            // Adiciona os itens da lista interna à ListView
            foreach (var product in cartProducts)
            {
                decimal totalPrice = product.price * product.quantity;  // Calcula o preço total (quantidade * preço unitário)

                ListViewItem item = new ListViewItem(product.productName);  // Nome do produto
                item.SubItems.Add(product.clientID.ToString());
                item.SubItems.Add(product.size);  // Tamanho do produto
                item.SubItems.Add(product.quantity.ToString());  // Quantidade do produto
                item.SubItems.Add(product.price.ToString() + "€");  // Preço unitário formatado como moeda
                item.SubItems.Add(totalPrice.ToString() + "€");  // Preço total formatado como moeda

                lvCart.Items.Add(item);  // Adiciona o item à ListView
            }
        }

        // Método para atualizar o preço total
        public void UpdateTotalPrice()
        {
            decimal total = 0;

            // Soma os preços totais dos produtos no carrinho
            foreach (var product in cartProducts)
            {
                total += product.price * product.quantity;  // Preço total por produto (quantidade * preço)
            }

            // Atualiza o texto da Label com o preço total
            lblTotal.Text = $"Total: {total + "€"}";  // Formata como moeda
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                // Enviar dados para o controlador
                ctrl.FinalizarCompra(cartProducts);

                // Limpar o carrinho
                cartProducts.Clear();
                lvCart.Items.Clear();
                UpdateTotalPrice();

                MessageBox.Show("Success Sale!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error to finish the sale: {ex.Message}");
            }
        }

        private void frmCart_Load(object sender, EventArgs e)
        {
            LoadListView();
            btnFinish.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnFinish.Width, btnFinish.Height, 40, 40));
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
