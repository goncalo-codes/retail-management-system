using projetoLoja.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projetoLoja.Presentation
{
    public partial class frmClient : Form
    {
        bool lv;
        private FlowLayoutPanel flowLayoutPanel;
        private Panel sidePanel; // Painel lateral
        private Ctrl ctrl; // Controlador
        public int UserID { get; set; }  // Propriedade para armazenar o UserID
        public int EmpID { get; set; }  // Propriedade para armazenar o UserID

        // Adicionando uma variável para armazenar o formulário de carrinho
        frmCart frmCart = new frmCart();
        public frmClient(bool leave)
        {
            ctrl = new Ctrl(); // Inicializa o Ctrl
            InitializeComponent();
            //
            this.BackColor = Color.White; // Define o fundo branco
            //
            InitializeSidePanel(); // Inicializa o painel lateral
            InitializeFlowLayoutPanel(); // Inicializa o painel de produtos
            LoadCategories(); // Carrega as categorias no menu lateral
            LoadProducts(); // Carrega todos os produtos inicialmente
            lv = leave;
        }


        private void InitializeSidePanel()
        {
            // Painel lateral
            sidePanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Color.FromArgb(91, 62, 45),
                AutoScroll = true
            };

            this.Controls.Add(sidePanel);
        }

        private void InitializeFlowLayoutPanel()
        {
            flowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, // Ocupa o espaço restante ao lado do painel lateral
                AutoScroll = true
            };

            // Adicione o painel principal antes do painel lateral
            this.Controls.Add(flowLayoutPanel);
            this.Controls.SetChildIndex(flowLayoutPanel, 1); // Garante que o painel lateral tenha prioridade
        }

        private void LoadCategories()
        {
            // Adiciona a opção "Todos" no menu lateral
            Button btnAll = new Button
            {
                Text = "All",
                Dock = DockStyle.Top,
                Height = 40,
                //
                ForeColor = Color.White,
                BackColor = Color.FromArgb(91, 62, 45),
                FlatStyle = FlatStyle.Flat
                //
            };

            btnAll.Click += (sender, e) =>
            {
                flowLayoutPanel.Controls.Clear(); // Limpa os produtos exibidos
                LoadProducts(); // Carrega todos os produtos
            };

            // Inicializa o botão para abrir o carrinho
            Button btnOpenCart = new Button
            {
                Text = "Open Cart",
                Dock = DockStyle.Top,
                Height = 40,
                //
                ForeColor = Color.White,
                BackColor = Color.FromArgb(91, 62, 45),
                FlatStyle = FlatStyle.Flat
                //
            };

            btnOpenCart.Click += (sender, e) =>
            {
                if (frmCart.IsDisposed)
                {
                    frmCart = new frmCart(); // Reinstancia o frmCart se já tiver sido descartado

                    // Atualiza os produtos exibidos no carrinho
                    frmCart.DisplayProducts();

                    // Atualiza o preço total
                    frmCart.UpdateTotalPrice();
                }
                frmCart.Show();
            };
            


            Button btnExit = new Button
            {
                Text = "Exit",
                Dock = DockStyle.Bottom,
                Height = 40,
                //
                ForeColor = Color.White,
                BackColor = Color.FromArgb(91, 62, 45),
                FlatStyle = FlatStyle.Flat
                //
            };

            btnExit.Click += (sender, e) =>
            {
                if (lv)
                {
                    this.Close();
                    frmLogin frmLogin = new frmLogin();
                    frmLogin.Show();
                }
                else
                {
                    this.Close();
                }

            };

            sidePanel.Controls.Add(btnExit);

            // Obtém as categorias do banco de dados usando o Ctrl
            List<string> categories = ctrl.GetCategories();

            if (categories == null || categories.Count == 0)
            {
                Label lblNoCategories = new Label
                {
                    Text = "No category found",
                    AutoSize = true,
                    //
                    ForeColor = Color.White
                    //
                };
                sidePanel.Controls.Add(lblNoCategories);
            }
            else
            {
                foreach (string category in categories)
                {
                    Button btnCategory = new Button
                    {
                        Text = category,
                        Dock = DockStyle.Top,
                        Height = 40,
                        //
                        ForeColor = Color.White,
                        BackColor = Color.FromArgb(91, 62, 45),
                        FlatStyle = FlatStyle.Flat,
                        //
                        Tag = category // Armazena o nome da categoria como Tag
                    };

                    btnCategory.Click += (sender, e) =>
                    {
                        string selectedCategory = (string)((Button)sender).Tag;
                        FilterProductsByCategory(selectedCategory); // Filtra os produtos
                    };

                    sidePanel.Controls.Add(btnCategory);
                }
            }
            sidePanel.Controls.Add(btnAll);
            sidePanel.Controls.Add(btnOpenCart);
        }

        private void FilterProductsByCategory(string category)
        {
            // Limpa os produtos exibidos
            flowLayoutPanel.Controls.Clear();

            // Obtém os produtos filtrados pela categoria
            List<Dictionary<string, object>> filteredProducts = ctrl.GetProductsByCategory(category);

            if (filteredProducts == null || filteredProducts.Count == 0)
            {
                Label lblNoProducts = new Label
                {
                    Text = "No products in this category",
                    AutoSize = true
                };
                flowLayoutPanel.Controls.Add(lblNoProducts);
            }
            else
            {
                foreach (var product in filteredProducts)
                {
                    Panel productCard = CreateProductCard(product);
                    flowLayoutPanel.Controls.Add(productCard);
                }
            }
        }

        // Função para carregar os produtos do estoque
        private void LoadProducts()
        {
            // Carrega todos os produtos inicialmente
            List<Dictionary<string, object>> products = ctrl.GetProductsFromStock();

            if (products == null || products.Count == 0)
            {
                Label lblNoProducts = new Label
                {
                    Text = "Product not found",
                    AutoSize = true
                };
                flowLayoutPanel.Controls.Add(lblNoProducts);
            }
            else
            {
                foreach (var product in products)
                {
                    Panel productCard = CreateProductCard(product);
                    flowLayoutPanel.Controls.Add(productCard);
                }
            }
        }

        

        // Função para criar um card de produto
        private Panel CreateProductCard(Dictionary<string, object> product)
        {
            // Cria o painel (card) com as informações do produto
            Panel productPanel = new Panel
            {
                Width = 220,
                Height = 400,
                //BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(8),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            Label lblName = new Label
            {
                Text = product["ProductName"].ToString(),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            Label lblDescription = new Label
            {
                Text = product["Description"].ToString(),
                AutoSize = false,
                MaximumSize = new Size(productPanel.Width - 20, 0),
                Height = 60,
                Font = new Font("Arial", 9),
                TextAlign = ContentAlignment.TopLeft
            };

            Label lblPrice = new Label
            {
                Text = "Price: " + product["Price"].ToString() + "€",
                AutoSize = true,
                Font = new Font("Arial", 9)
            };

            PictureBox pictureBox = new PictureBox
            {
                Width = 180,
                Height = 180,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // Tente carregar a imagem, se não houver, defina uma imagem padrão
            string imagePath = product["ImagePath"].ToString();
            if (product["ImagePath"] != DBNull.Value)
            {
                byte[] imageBytes = (byte[])product["ImagePath"];
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    pictureBox.Image = Image.FromStream(ms);
                }
            }
            else
            {
                MessageBox.Show("Image not found!");
            }

            ComboBox cmbSize = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 100,
                Height = 30,
                Font = new Font("Arial", 8)
            };
            int productId = Convert.ToInt32(product["ProductId"]);

            List<string> sizes = ctrl.getProductSizes(productId);
            cmbSize.Items.AddRange(sizes.ToArray());
            cmbSize.SelectedIndex = -1;

            TextBox txtQuantity = new TextBox
            {
                Width = 100,
                Height = 30,
                Font = new Font("Arial", 8),
                Text = "1"
            };

            Button btnAddToCart = new Button
            {
                Text = "Add to Cart",
                Tag = product["ProductName"].ToString(),
                Width = 100,
                Height = 30,
                Font = new Font("Arial", 8)
            };

            
            btnAddToCart.Click += (sender, e) =>
                {
                    int userID = this.UserID;  // Acessando o UserID no frmClient
                    string productName = product["ProductName"].ToString();
                    decimal price = Convert.ToDecimal(product["Price"]);
                    string sizeSelected = cmbSize.SelectedItem.ToString();
                    int quantity = int.Parse(txtQuantity.Text);

                    if (EmpID == 0)
                    {
                        EmpID = 0;
                    }
                


                // Acesse o estoque disponível do produto
                int availableStock = Convert.ToInt32(product["Quantity"]);

                if (quantity > availableStock)
                {
                    MessageBox.Show($"Available Quantity: {availableStock}. Not possible to add {quantity} to the cart.");
                }
                else
                {
                    // Adiciona o produto na lista estática do carrinho
                    frmCart.cartProducts.Add((EmpID, userID, productName, price, sizeSelected, quantity));

                    // Atualiza os produtos exibidos no carrinho
                    frmCart.DisplayProducts();

                    // Atualiza o preço total
                    frmCart.UpdateTotalPrice();

                    MessageBox.Show($"Product {productName} ({sizeSelected}, Quantity: {quantity}) added to cart!");
                }
            };

            productPanel.Controls.Add(lblName);
            productPanel.Controls.Add(pictureBox);
            productPanel.Controls.Add(lblDescription);
            productPanel.Controls.Add(lblPrice);
            productPanel.Controls.Add(cmbSize);
            productPanel.Controls.Add(txtQuantity);
            productPanel.Controls.Add(btnAddToCart);
            cmbSize.SelectedIndex = 0;
            lblName.Location = new Point((productPanel.Width - lblName.Width) / 2, 10);
            pictureBox.Location = new Point((productPanel.Width - pictureBox.Width) / 2, lblName.Bottom + 10);
            lblDescription.Location = new Point((productPanel.Width - lblDescription.Width) / 2, pictureBox.Bottom + 10);
            lblPrice.Location = new Point((productPanel.Width - lblPrice.Width) / 2, lblDescription.Bottom + 10);
            cmbSize.Location = new Point((productPanel.Width - cmbSize.Width) / 2, lblPrice.Bottom + 10);
            txtQuantity.Location = new Point((productPanel.Width - txtQuantity.Width) / 2, cmbSize.Bottom + 10);
            btnAddToCart.Location = new Point((productPanel.Width - btnAddToCart.Width) / 2, txtQuantity.Bottom + 10);

            return productPanel;
        }
    }
}