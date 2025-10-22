using projetoLoja.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static projetoLoja.Presentation.CONSULT.frmConsultEmployee;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using projetoLoja.Presentation.CONSULT;
using static projetoLoja.Presentation.CONSULT.frmConsultClient;
using System.Security.Claims;
using static projetoLoja.Presentation.CONSULT.frmConsultProduct;
using System.Diagnostics;
using static projetoLoja.Presentation.CONSULT.frmStock;
using projetoLoja.Presentation;
using System.Windows.Forms;
using static projetoLoja.Presentation.CONSULT.frmConsultSale;
using static projetoLoja.Presentation.CONSULT.frmConsultSaleDate;

namespace projetoLoja.Control
{
    internal class Ctrl
    {
        public bool ctrlExist = false;
        public string ctrlMessage = "";
        public string ctrlUserName = ""; // Nome do usuário capturado
        public string ctrlUserType = ""; // Tipo do usuário capturado
        public string ctrlEmpStatus = "";
        public int ctrlUserIDLog;
        public int ctrlEmpIDLog;
        private LoginCommands loggObj = new LoginCommands();

        public void accessingCredentials(string lgname, string passd)
        {
            // Instanciar LoginCommands
            loggObj.verifyCredentials(lgname, passd);

            // Atualizar estado do Ctrl
            ctrlExist = loggObj.exist;
            ctrlMessage = loggObj.message;

            // Capturar valores adicionais (se credenciais válidas)
            if (ctrlExist)
            {
                ctrlUserName = loggObj.UserName;
                ctrlUserType = loggObj.UserType;
                ctrlUserIDLog = loggObj.ClientID;
                ctrlEmpIDLog = loggObj.EmpID;
                ctrlEmpStatus = loggObj.EmpStatus;
            }
        }

        public bool accessingUniqueData(string lgname, string email)
        {
            bool operSuccess = loggObj.verifyUniqueData(lgname, email);

            ctrlExist = loggObj.exist;
            ctrlMessage = loggObj.message;

            return operSuccess;
        }

        public bool accessingUniqueProduct(string name)
        {
            bool operSuccess = loggObj.verifyUniqueProduct(name);

            ctrlExist = loggObj.exist;
            ctrlMessage = loggObj.message;

            return operSuccess;
        }

        public bool registCredentials(string logg, string email, string passwd, string retypePasswd, string userType)
        {
            bool operSuccess = false;

            // testing if empties
            if (logg != "" && passwd != "" && retypePasswd != "")
            {
                if (passwd == retypePasswd)
                {
                    // object for data access
                    operSuccess = loggObj.insertCredentials(logg, email, passwd, userType);
                    ctrlMessage = loggObj.message;
                }
                else
                    ctrlMessage = "Passwords don't match";
            }
            else
                ctrlMessage = "Empty fields";

            return operSuccess;
        }

        public void registClient(string name, string numb, string add, string nif)
        {
            loggObj.insertClient(name, numb, add, nif);
        }

        public void registProduct (string name, string desc, int cat, float price, DateTime date)
        {
            loggObj.insertProduct(name, desc,cat, price, date);
            ctrlMessage = loggObj.message;
        }

        public void registStock(int prdID, int catID, int qtd, DateTime date)
        {
            loggObj.insertStock(prdID, catID, qtd, date);
            ctrlMessage = loggObj.message;
        }

        public List<string[]> loadJobDepartments()
        {
            return loggObj.getJobDepartments();
        }

        public List<string[]> loadProducts()
        {
            return loggObj.getProducts();
        }

        public List<string[]> loadClients()
        {
            return loggObj.getClients();
        }

        public List<string[]> loadCat()
        {
            return loggObj.getCategories();
        }

        public List<string[]> loadSize()
        {
            return loggObj.getSizes();
        }

        public void registEmployee(string name, string number, string add, int jobID, float salary, DateTime hiredDate, string status) 
        {
            loggObj.insertEmployee(name, number, add, jobID, salary, hiredDate, status);
        }

        public bool getEmployeeList(ref Employee[] employees)
        {
            bool operSuccess;

            operSuccess = loggObj.EmployeesList(ref employees);
            ctrlExist = loggObj.exist;
            ctrlMessage = loggObj.message;
            return operSuccess;
        }

        public bool getSaleList(ref Sale[] sales)
        {
            bool operSuccess;

            operSuccess = loggObj.salesList(ref sales);
            ctrlExist = loggObj.exist;
            ctrlMessage = loggObj.message;
            return operSuccess;
        }

        public bool getSaleList2(ref SaleDate[] salesDate)
        {
            bool operSuccess;

            operSuccess = loggObj.salesList2(ref salesDate);
            ctrlExist = loggObj.exist;
            ctrlMessage = loggObj.message;
            return operSuccess;
        }

        public bool getProductList(ref Product[] products)
        {
            bool operSuccess;

            operSuccess = loggObj.productList(ref products);
            ctrlExist = loggObj.exist;
            ctrlMessage = loggObj.message;
            return operSuccess;
        }

        public bool getStockList(ref Stock[] stocks)
        {
            bool operSuccess;

            operSuccess = loggObj.stockList(ref stocks);
            ctrlExist = loggObj.exist;
            ctrlMessage = loggObj.message;
            return operSuccess;
        }

        public bool updateEmployee(int userID, int empID, string fullName, string tel, string add, int jobID, float salary, string loginName, string email, string password, string status)
        {
            bool operSuccess = false;


            // object for data access

            operSuccess = loggObj.UpdateEmployeeCredentials(userID, empID, fullName, tel, add, jobID, salary, loginName, email, password, status);
            ctrlMessage = loggObj.message;



            return operSuccess;
        }

        public bool updateProduct(int prodID, string prodName, int category, string desc, decimal price)
        {
            bool operSuccess = false;


            // object for data access

            operSuccess = loggObj.UpdateProductCredentials(prodID, prodName, category, desc, price);
            ctrlMessage = loggObj.message;



            return operSuccess;
        }

        public bool updateStock(int prdID, int sizeID, int newQtd, DateTime dataUpdt)
        {
            bool operSuccess = false;


            // object for data access

            operSuccess = loggObj.UpdateStockCredentials(prdID, sizeID, newQtd, dataUpdt);
            ctrlMessage = loggObj.message;



            return operSuccess;
        }

        public bool employeeDelete(int userID, int empID)
        {
            bool operSuccess = false;
            operSuccess = loggObj.deleteEmployee(userID, empID);
            ctrlExist = loggObj.exist;
            ctrlMessage = loggObj.message;

            return operSuccess;
        }

        public bool productDelete(int prodID)
        {
            bool operSuccess = false;
            operSuccess = loggObj.deleteProduct(prodID);
            ctrlExist = loggObj.exist;
            ctrlMessage = loggObj.message;

            return operSuccess;
        }

        public bool saleDelete(int saleId)
        {
            bool operSuccess = false;
            operSuccess = loggObj.deleteSale(saleId);
            ctrlExist = loggObj.exist;
            ctrlMessage = loggObj.message;

            return operSuccess;
        }



        public bool stockDelete(int delPrdID,int  delSizeID)
        {
            bool operSuccess = false;
            operSuccess = loggObj.deleteStock(delPrdID, delSizeID);
            ctrlExist = loggObj.exist;
            ctrlMessage = loggObj.message;

            return operSuccess;
        }

        public bool getClientList(ref Client[] clients)
        {
            bool operSuccess;

            operSuccess = loggObj.clientList(ref clients);
            ctrlExist = loggObj.exist;
            ctrlMessage = loggObj.message;
            return operSuccess;
        }
        public bool updateClient(int userID, int cliID, string NIF, string fullName, string tel, string add, string loginName, string email, string password)
        {
            bool operSuccess = false;


            // object for data access

            operSuccess = loggObj.UpdateClientCredentials(userID, cliID, NIF, fullName, tel, add, loginName, email, password);
            ctrlMessage = loggObj.message;



            return operSuccess;
        }

        public bool clientDelete(int userID, int cliID)
        {
            bool operSuccess = false;
            operSuccess = loggObj.deleteClient(userID, cliID);
            ctrlExist = loggObj.exist;
            ctrlMessage = loggObj.message;

            return operSuccess;
        }


        public List<Dictionary<string, object>> GetProductsFromStock()
        {
            // Chama a função na classe LoginCommands para obter os produtos do estoque
            return loggObj.GetProductsFromStock(); // Assumindo que GetProductsFromStock está implementado na classe LoginCommands
        }

        public List<string> getProductSizes(int productID)
        {
            // Chama a função GetProductSizes da classe logCommands para obter os tamanhos do produto
            List<string> sizes = loggObj.GetProductSizes(productID);

            return sizes;
        }

        public List<string> GetCategories()
        {
            return loggObj.GetCategories();
        }

        public List<Dictionary<string, object>> GetProductsByCategory(string category)
        {
            return loggObj.GetProductsByCategory(category);
        }

        public void FinalizarCompra(List<(int? empID, int clientID, string productName, decimal price, string size, int quantity)> cartProducts)
        {

            if (cartProducts == null || !cartProducts.Any())
                throw new Exception("Empty cart!");

            foreach (var product in cartProducts)
            {
                try
                {
                    // Verificar se o produto existe no banco
                    if (string.IsNullOrEmpty(product.productName))
                        throw new Exception("Nome do produto não pode ser vazio.");
                    if (string.IsNullOrEmpty(product.size))
                        throw new Exception("Tamanho não pode ser vazio.");

                    // Obter o ProductId e SizeId através das funções no loggObj
                    int productId = loggObj.ObterProductIdPorNome(product.productName);
                    int sizeId = loggObj.ObterSizeIdPorNome(product.size);

                    // Verificar se os IDs foram encontrados
                    if (productId <= 0)
                        throw new Exception($"Produto {product.productName} não encontrado.");
                    if (sizeId <= 0)
                        throw new Exception($"Tamanho {product.size} não encontrado.");

                    // Criar o registro de venda
                    loggObj.InserirVenda(
                        clientId: product.clientID,
                        empID: Convert.ToInt32(product.empID),
                        productId: productId,
                        sizeId: sizeId,
                        quantity: product.quantity,
                        total: product.price * product.quantity
                    );
                }
                catch (Exception ex)
                {
                    // Capturar qualquer erro que possa ocorrer durante o processo de finalização da compra
                    throw new Exception($"Erro ao finalizar compra para o produto {product.productName} (Tamanho: {product.size}): {ex.Message}");
                }
            }
        }
    }
}
