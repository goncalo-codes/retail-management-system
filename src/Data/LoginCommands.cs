using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using projetoLoja.Presentation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static projetoLoja.Presentation.CONSULT.frmConsultClient;
using static projetoLoja.Presentation.CONSULT.frmConsultEmployee;
using static projetoLoja.Presentation.CONSULT.frmConsultProduct;
using static projetoLoja.Presentation.CONSULT.frmConsultSale;
using static projetoLoja.Presentation.CONSULT.frmConsultSaleDate;
using static projetoLoja.Presentation.CONSULT.frmStock;

namespace projetoLoja.Data
{

    internal class LoginCommands
    {
        public int userId;
        public string UserName = "";
        public string UserType = "";
        public int ClientID;
        public int EmpID;
        public string EmpStatus = "";
        // conexão com a bd
        Connection conn = new Connection();
        // SqlCmd supports the sql statement
        SqlCommand SqlCmd = new SqlCommand();
        // pointer to read table records
        SqlDataReader dr;

        // control attributes
        public bool exist = false;
        public string message = "";

        public void verifyCredentials(string ln, string pw)
        {
            SqlCmd.Parameters.Clear(); // Limpar parâmetros antigos
            SqlCmd.Parameters.AddWithValue("@log", ln);
            SqlCmd.Parameters.AddWithValue("@pwd", pw);
            SqlCmd.CommandText = "SELECT * FROM Users WHERE LoginName = @log AND Passwd = @pwd";

            try
            {
                SqlCmd.Connection = conn.conectar();
                dr = SqlCmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read(); // Posicionar no primeiro resultado

                    exist = true;
                    message = "Credenciais válidas!";

                    // Obter valores do leitor
                    UserName = dr["LoginName"].ToString(); // Nome do usuário
                    UserType = dr["UserType"].ToString();  // Tipo do usuário

                    // Adicionando lógica para pegar ClientID ou EmpID
                    int userId = Convert.ToInt32(dr["UserId"]); // UserId vem da tabela Users

                    // Caso seja Client, pega o ClientId
                    if (UserType == "Client")
                    {
                        SqlCmd.Parameters.Clear();
                        SqlCmd.CommandText = "SELECT ClientId FROM Clients WHERE UserId = @UserId";
                        SqlCmd.Parameters.AddWithValue("@UserId", userId);

                        // Executar a consulta para obter o ClientId
                        dr.Close(); // Fechar o leitor atual
                        dr = SqlCmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            dr.Read(); // Posicionar no primeiro resultado
                            ClientID = Convert.ToInt32(dr["ClientId"]);
                        }
                        else
                        {
                            throw new Exception("Cliente não encontrado para o UserId fornecido.");
                        }
                    }
                    // Caso não seja Client, pega o EmpId
                    else if (UserType == "Employee" || UserType == "Manager" || UserType == "Adm")
                    {
                        SqlCmd.Parameters.Clear();
                        SqlCmd.CommandText = "SELECT EmpId, Status FROM Employees WHERE UserId = @UserId";
                        SqlCmd.Parameters.AddWithValue("@UserId", userId);

                        // Executar a consulta para obter o EmpId
                        dr.Close(); // Fechar o leitor atual
                        dr = SqlCmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            dr.Read();
                            EmpID = Convert.ToInt32(dr["EmpId"]);
                            EmpStatus = dr["Status"].ToString();
                        }
                        else
                        {
                            throw new Exception("Funcionário não encontrado para o UserId fornecido.");
                        }
                    }

                    dr.Close();
                }
                else
                {
                    exist = false;
                    message = "Credenciais inválidas!";
                }

                conn.desconectar();
            }
            catch (SqlException error)
            {
                exist = false;
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }
        }

        public bool verifyUniqueData(string ln, string eml)
        {   // access to DB to verify if login exist
            bool operSuccess = false;

            SqlCmd.Parameters.AddWithValue("@log", ln);
            SqlCmd.Parameters.AddWithValue("@email", eml);
            SqlCmd.CommandText = "select * from users where LoginName = @log OR Email = @email";

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                // execute sql statement
                dr = SqlCmd.ExecuteReader();
                if (dr.HasRows)
                {
                    exist = true;
                    message = "Login Name or Email Invalid!";
                }
                else
                {
                    exist = false;
                    message = "Login Name Valid!";
                }

                conn.desconectar();
                dr.Close();
                operSuccess = true;
            }
            catch (SqlException error)
            {
                exist = false;
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
                operSuccess = false;
            }
            return operSuccess;
        }

        public bool verifyUniqueProduct(string name)
        {   // access to DB to verify if login exist
            bool operSuccess = false;

            SqlCmd.Parameters.AddWithValue("@nm", name);
            SqlCmd.CommandText = "select * from products where ProductName = @nm";

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                // execute sql statement
                dr = SqlCmd.ExecuteReader();
                if (dr.HasRows)
                {
                    exist = true;
                    message = "Already exist a product with this name!";
                }
                else
                {
                    exist = false;
                    message = "Product Valid!";
                }

                conn.desconectar();
                dr.Close();
                operSuccess = true;
            }
            catch (SqlException error)
            {
                exist = false;
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
                operSuccess = false;
            }
            return operSuccess;
        }

        public bool insertCredentials(string ln, string email, string pw, string usrTp)
        {
            bool operSuccess = false;

            SqlCmd.Parameters.Clear(); // Limpa os parâmetros antigos
            SqlCmd.Parameters.AddWithValue("@log", ln);
            SqlCmd.Parameters.AddWithValue("@pwd", pw);
            SqlCmd.Parameters.AddWithValue("@eml", email);
            SqlCmd.Parameters.AddWithValue("@usrType", usrTp);
            SqlCmd.CommandText = "INSERT INTO Users (LoginName, Email, Passwd, UserType) OUTPUT INSERTED.UserId VALUES (@log, @eml, @pwd, @usrType)";

            try
            {
                // Abre a conexão com o banco de dados
                SqlCmd.Connection = conn.conectar();

                // Recupera o UserId gerado pelo banco de dados
                var result = SqlCmd.ExecuteScalar();

                if (result != null)
                {
                    // Atualiza o valor global de userId
                    userId = Convert.ToInt32(result);
                    operSuccess = true;
                    message = "User inserted!";
                }
                else
                {
                    message = "Failed to get UserId.";
                }

                conn.desconectar();
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }

            return operSuccess;
        }

        public void insertClient(string name, string numb, string add, string nif)
        {
            SqlCmd.Parameters.AddWithValue("@usr", userId);
            SqlCmd.Parameters.AddWithValue("@nm", name);
            SqlCmd.Parameters.AddWithValue("@nif", nif);
            SqlCmd.Parameters.AddWithValue("@num", numb);
            SqlCmd.Parameters.AddWithValue("@add", add);

            SqlCmd.CommandText = "insert into Clients values (@usr, @nm, @nif, @num, @add)";
            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                SqlCmd.ExecuteNonQuery();
                conn.desconectar();
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }
        }

        public void insertProduct (string name, string desc, int cat, float price, DateTime date)
        {
            SqlCmd.Parameters.AddWithValue("@name", name);
            SqlCmd.Parameters.AddWithValue("@desc", desc);
            SqlCmd.Parameters.AddWithValue("@cat", cat);
            SqlCmd.Parameters.AddWithValue("@price", price);
            SqlCmd.Parameters.AddWithValue("@date", date);

            SqlCmd.CommandText = "insert into Products values (@name, @desc, @cat, @price, @date, null)";
            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                SqlCmd.ExecuteNonQuery();
                conn.desconectar();
                message = "Product Inserted";
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }
        }

        public void insertStock(int prdID, int catID, int qtd, DateTime date)
        {
            SqlCmd.Parameters.AddWithValue("@prdID", prdID);
            SqlCmd.Parameters.AddWithValue("@catID", catID);
            SqlCmd.Parameters.AddWithValue("@qtd", qtd);
            SqlCmd.Parameters.AddWithValue("@date", date);

            SqlCmd.CommandText = "insert into Stock values (@prdID, @catID, @qtd, @date)";
            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                SqlCmd.ExecuteNonQuery();
                conn.desconectar();
                message = "Product Inserted";
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }
        }

        public void insertEmployee(string name, string numb, string add, int jobID, float salary, DateTime hiredDate, string status)
        {

            SqlCmd.Parameters.AddWithValue("@usr", userId);
            SqlCmd.Parameters.AddWithValue("@nm", name);
            SqlCmd.Parameters.AddWithValue("@num", numb);
            SqlCmd.Parameters.AddWithValue("@add", add);
            SqlCmd.Parameters.AddWithValue("@jbID", jobID);
            SqlCmd.Parameters.AddWithValue("@slr", salary);
            SqlCmd.Parameters.AddWithValue("@hdDt", hiredDate);
            SqlCmd.Parameters.AddWithValue("@sta", status);

            SqlCmd.CommandText = "insert into Employees values (@usr, @nm, @num, @add, @jbID, @slr, @hdDt, @sta)";
            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                SqlCmd.ExecuteNonQuery();
                conn.desconectar();
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }
        }

        public List<string[]> getJobDepartments()
        {
            List<string[]> jobDepartments = new List<string[]>();
            SqlCmd.CommandText = "SELECT JobDeptId, Department, JobTitle FROM JobDepartments";

            try
            {
                SqlCmd.Connection = conn.conectar();
                dr = SqlCmd.ExecuteReader();

                while (dr.Read())
                {
                    // Adiciona cada registro como um array de strings
                    jobDepartments.Add(new string[]
                    {
                dr["JobDeptId"].ToString(),
                dr["Department"].ToString(),
                dr["JobTitle"].ToString()
                    });
                }

                dr.Close();
                conn.desconectar();
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }

            return jobDepartments;
        }

        public List<string[]> getProducts()
        {
            List<string[]> productsData = new List<string[]>();
            SqlCmd.CommandText = "SELECT ProductId, ProductName, Price FROM Products;";

            try
            {
                SqlCmd.Connection = conn.conectar();
                dr = SqlCmd.ExecuteReader();

                while (dr.Read())
                {
                    // Adiciona cada registro como um array de strings
                    productsData.Add(new string[]
                    {
                dr["ProductId"].ToString(),
                dr["ProductName"].ToString(),
                dr["Price"].ToString()
                    });
                }

                dr.Close();
                conn.desconectar();
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }

            return productsData;
        }

        public List<string[]> getClients()
        {
            List<string[]> clientsData = new List<string[]>();
            SqlCmd.CommandText = "SELECT ClientId, FullName, NIF FROM Clients;";

            try
            {
                SqlCmd.Connection = conn.conectar();
                dr = SqlCmd.ExecuteReader();

                while (dr.Read())
                {
                    // Adiciona cada registro como um array de strings
                    clientsData.Add(new string[]
                    {
                dr["ClientId"].ToString(),
                dr["FullName"].ToString(),
                dr["NIF"].ToString(),

                    });
                }

                dr.Close();
                conn.desconectar();
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }

            return clientsData;
        }

        public List<string[]> getCategories()
        {
            List<string[]> productsData = new List<string[]>();
            SqlCmd.CommandText = "SELECT CategoryID, CategoryName FROM Categories";

            try
            {
                SqlCmd.Connection = conn.conectar();
                dr = SqlCmd.ExecuteReader();

                while (dr.Read())
                {
                    // Adiciona cada registro como um array de strings
                    productsData.Add(new string[]
                    {
                dr["CategoryID"].ToString(),
                dr["CategoryName"].ToString()
                    });
                }

                dr.Close();
                conn.desconectar();
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }

            return productsData;
        }

        public List<string[]> getSizes()
        {
            List<string[]> sizesData = new List<string[]>();
            SqlCmd.CommandText = "select SizeID, SizeName from sizes";

            try
            {
                SqlCmd.Connection = conn.conectar();
                dr = SqlCmd.ExecuteReader();

                while (dr.Read())
                {
                    // Adiciona cada registro como um array de strings
                    sizesData.Add(new string[]
                    {
                dr["SizeID"].ToString(),
                dr["SizeName"].ToString()
                    });
                }

                dr.Close();
                conn.desconectar();
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }

            return sizesData;
        }


        public bool EmployeesList(ref Employee[] employees)
        {
            // statement sql to receive / list the credentials records
            SqlCmd.CommandText = "SELECT u.UserId,e.EmpId,e.FullName,e.PhoneNumber,e.Address, jd.JobTitle,e.Salary,e.DateHired,u.LoginName,u.Email,u.Passwd,e.Status FROM Users u INNER JOIN Employees e ON u.UserId = e.UserId INNER JOIN JobDepartments jd ON e.JobDeptId = jd.JobDeptId ORDER BY e.DateHired ASC;";
            bool operSuccess = false;

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                // execute sql statement
                dr = SqlCmd.ExecuteReader();
                if (dr.HasRows)
                {
                    exist = true;

                    int userID;
                    int empID;
                    string username;
                    string email;
                    string passwd;
                    string fullName;
                    string tel;
                    string address;
                    string job;
                    decimal salary;
                    DateTime dateHired;
                    string status;

                    int i = 0; // record counter

                    while (dr.Read()) // read while TRUE (existing records to read)
                    {
                        userID = dr.GetInt32(0);
                        empID = dr.GetInt32(1);
                        fullName = dr.GetString(2);
                        tel = dr.GetString(3);
                        address = dr.GetString(4);
                        job = dr.GetString(5);
                        salary = dr.GetDecimal(6);
                        dateHired = dr.GetDateTime(7);
                        username = dr.GetString(8);
                        email = dr.GetString(9);
                        passwd = dr.GetString(10);
                        status = dr.GetString(11);

                        // array redimention
                        Array.Resize(ref employees, employees.Length + 1);
                        // create memory
                        employees[employees.Length - 1] = new Employee();
                        // fill the array
                        employees[employees.Length - 1].userID = userID;
                        employees[employees.Length - 1].empID = empID;
                        employees[employees.Length - 1].fullName = fullName;
                        employees[employees.Length - 1].tel = tel;
                        employees[employees.Length - 1].address = address;
                        employees[employees.Length - 1].job = job;
                        employees[employees.Length - 1].salary = salary;
                        employees[employees.Length - 1].dateHired = dateHired;
                        employees[employees.Length - 1].username = username;
                        employees[employees.Length - 1].email = email;
                        employees[employees.Length - 1].passwd = passwd;
                        employees[employees.Length - 1].status = status;
                        i++;
                    }
                    message = "Retriving: " + i + " records!";
                }
                else
                {
                    message = "No records!";
                    exist = false;
                }

                conn.desconectar();
                operSuccess = true;
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }
            return operSuccess;
        }

        public bool salesList(ref Sale[] sales)
        {
            // statement sql to receive / list the credentials records
            SqlCmd.CommandText = "SELECT s.SaleId, s.ClientId, s.EmployeeId, s.ProductId, s.SizeID, c.FullName, e.FullName, p.ProductName, s.Quantity, sz.SizeName, s.SaleDate, s.Total FROM Sales s JOIN Clients c ON s.ClientId = c.ClientId LEFT JOIN Employees e ON s.EmployeeId = e.EmpId JOIN Products p ON s.ProductId = p.ProductID JOIN Sizes sz ON s.SizeID = sz.SizeID ORDER BY s.SaleDate DESC;";
            bool operSuccess = false;

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                // execute sql statement
                dr = SqlCmd.ExecuteReader();
                if (dr.HasRows)
                {
                    exist = true;

                    int saleID;
                    int clientID;
                    int employeeID;
                    int productID;
                    int sizeID;
                    string clientName;
                    string employeeName;
                    string productName;
                    int quantity;
                    string sizeName;
                    DateTime saleDate;
                    decimal total;

                    int i = 0; // record counter

                    while (dr.Read()) // read while TRUE (existing records to read)
                    {
                        saleID = dr.GetInt32(0);
                        clientID = dr.GetInt32(1);
                        employeeID = dr.IsDBNull(2) ? 0 : dr.GetInt32(2); // Verifica se EmployeeId é NULL
                        productID = dr.GetInt32(3);
                        sizeID = dr.GetInt32(4);
                        clientName = dr.GetString(5);
                        employeeName = dr.IsDBNull(6) ? null : dr.GetString(6); // Verifica se EmployeeName é NULL
                        productName = dr.GetString(7);
                        quantity = dr.GetInt32(8);
                        sizeName = dr.GetString(9);
                        saleDate = dr.GetDateTime(10);
                        total = dr.GetDecimal(11);

                        // array redimention
                        Array.Resize(ref sales, sales.Length + 1);
                        // create memory
                        sales[sales.Length - 1] = new Sale();
                        // fill the array
                        sales[sales.Length - 1].saleID = saleID;
                        sales[sales.Length - 1].clientID = clientID;
                        sales[sales.Length - 1].employeeID = employeeID;
                        sales[sales.Length - 1].productID = productID;
                        sales[sales.Length - 1].sizeID = sizeID;
                        sales[sales.Length - 1].clientName = clientName;
                        sales[sales.Length - 1].employeeName = employeeName;
                        sales[sales.Length - 1].productName = productName;
                        sales[sales.Length - 1].quantity = quantity;
                        sales[sales.Length - 1].sizeName = sizeName;
                        sales[sales.Length - 1].saleDate = saleDate;
                        sales[sales.Length - 1].total = total;

                        i++;
                    }
                    message = "Retrieving: " + i + " records!";
                }
                else
                {
                    message = "No records!";
                    exist = false;
                }

                conn.desconectar();
                operSuccess = true;
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }
            return operSuccess;
        }

        public bool salesList2(ref SaleDate[] salesDate)
        {
            // statement sql to receive / list the credentials records
            SqlCmd.CommandText = "SELECT s.SaleId, s.ClientId, s.EmployeeId, s.ProductId, s.SizeID, c.FullName, e.FullName, p.ProductName, s.Quantity, sz.SizeName, s.SaleDate, s.Total FROM Sales s JOIN Clients c ON s.ClientId = c.ClientId LEFT JOIN Employees e ON s.EmployeeId = e.EmpId JOIN Products p ON s.ProductId = p.ProductID JOIN Sizes sz ON s.SizeID = sz.SizeID ORDER BY s.SaleDate DESC;";
            bool operSuccess = false;

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                // execute sql statement
                dr = SqlCmd.ExecuteReader();
                if (dr.HasRows)
                {
                    exist = true;

                    int saleID;
                    int clientID;
                    int employeeID;
                    int productID;
                    int sizeID;
                    string clientName;
                    string employeeName;
                    string productName;
                    int quantity;
                    string sizeName;
                    DateTime saleDate;
                    decimal total;

                    int i = 0; // record counter

                    while (dr.Read()) // read while TRUE (existing records to read)
                    {
                        saleID = dr.GetInt32(0);
                        clientID = dr.GetInt32(1);
                        employeeID = dr.IsDBNull(2) ? 0 : dr.GetInt32(2); // Verifica se EmployeeId é NULL
                        productID = dr.GetInt32(3);
                        sizeID = dr.GetInt32(4);
                        clientName = dr.GetString(5);
                        employeeName = dr.IsDBNull(6) ? null : dr.GetString(6); // Verifica se EmployeeName é NULL
                        productName = dr.GetString(7);
                        quantity = dr.GetInt32(8);
                        sizeName = dr.GetString(9);
                        saleDate = dr.GetDateTime(10);
                        total = dr.GetDecimal(11);

                        // array redimention
                        Array.Resize(ref salesDate, salesDate.Length + 1);
                        // create memory
                        salesDate[salesDate.Length - 1] = new SaleDate();
                        // fill the array
                        salesDate[salesDate.Length - 1].saleID = saleID;
                        salesDate[salesDate.Length - 1].clientID = clientID;
                        salesDate[salesDate.Length - 1].employeeID = employeeID;
                        salesDate[salesDate.Length - 1].productID = productID;
                        salesDate[salesDate.Length - 1].sizeID = sizeID;
                        salesDate[salesDate.Length - 1].clientName = clientName;
                        salesDate[salesDate.Length - 1].employeeName = employeeName;
                        salesDate[salesDate.Length - 1].productName = productName;
                        salesDate[salesDate.Length - 1].quantity = quantity;
                        salesDate[salesDate.Length - 1].sizeName = sizeName;
                        salesDate[salesDate.Length - 1].saleDate = saleDate;
                        salesDate[salesDate.Length - 1].total = total;

                        i++;
                    }
                    message = "Retrieving: " + i + " records!";
                }
                else
                {
                    message = "No records!";
                    exist = false;
                }

                conn.desconectar();
                operSuccess = true;
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }
            return operSuccess;
        }

        public bool productList(ref Product[] products)
        {
            // statement sql to receive / list the credentials records
            SqlCmd.CommandText = "SELECT p.ProductID, p.ProductName, p.Description, c.CategoryName, p.Price, p.DateAdded FROM Products p LEFT JOIN Categories c ON p.CategoryID = c.CategoryID;";
            bool operSuccess = false;

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                // execute sql statement
                dr = SqlCmd.ExecuteReader();
                if (dr.HasRows)
                {
                    exist = true;

                    int prdID;
                    string prdName;
                    string prdDesc;
                    string prdCat;
                    decimal prdPrice;
                    DateTime dateAdded;

                    int i = 0; // record counter

                    while (dr.Read()) // read while TRUE (existing records to read)
                    {
                        prdID = dr.GetInt32(0);
                        prdName = dr.GetString(1);
                        prdDesc = dr.GetString(2);
                        prdCat = dr.GetString(3);
                        prdPrice = dr.GetDecimal(4);
                        dateAdded = dr.GetDateTime(5);

                        // array redimention
                        Array.Resize(ref products, products.Length + 1);
                        // create memory
                        products[products.Length - 1] = new Product();
                        // fill the array
                        products[products.Length - 1].productID = prdID;
                        products[products.Length - 1].productName = prdName;
                        products[products.Length - 1].productDescription = prdDesc;
                        products[products.Length - 1].productCategory = prdCat;
                        products[products.Length - 1].productPrice = prdPrice;
                        products[products.Length - 1].dateAdded = dateAdded;
                        i++;
                    }
                    message = "Retriving: " + i + " records!";
                }
                else
                {
                    message = "No records!";
                    exist = false;
                }

                conn.desconectar();
                operSuccess = true;
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }
            return operSuccess;
        }

        public bool stockList(ref Stock[] stocks)
        {
            // statement sql to receive / list the credentials records
            SqlCmd.CommandText = "SELECT  s.ProductId, s.SizeID, sz.SizeName, p.ProductName, s.Quantity, s.LastUpdated FROM Stock s INNER JOIN Products p ON s.ProductId = p.ProductId INNER JOIN Sizes sz ON s.SizeID = sz.SizeID;";
            bool operSuccess = false;

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                // execute sql statement
                dr = SqlCmd.ExecuteReader();
                if (dr.HasRows)
                {
                    exist = true;

                    int prdID;
                    int sizeID;
                    string sizeName;
                    string prdName;
                    int qtd;
                    DateTime dateUpdt;

                    int i = 0; // record counter

                    while (dr.Read()) // read while TRUE (existing records to read)
                    {
                        prdID = dr.GetInt32(0);
                        sizeID = dr.GetInt32(1);
                        sizeName = dr.GetString(2);
                        prdName = dr.GetString(3);
                        qtd = dr.GetInt32(4);
                        dateUpdt = dr.GetDateTime(5);

                        // array redimention
                        Array.Resize(ref stocks, stocks.Length + 1);
                        // create memory
                        stocks[stocks.Length - 1] = new Stock();
                        // fill the array
                        stocks[stocks.Length - 1].prdID = prdID;
                        stocks[stocks.Length - 1].sizeID = sizeID;
                        stocks[stocks.Length - 1].sizeName = sizeName;
                        stocks[stocks.Length - 1].prdName = prdName;
                        stocks[stocks.Length - 1].qtd = qtd;
                        stocks[stocks.Length - 1].dateUpdt = dateUpdt;
                        i++;
                    }
                    message = "Retriving: " + i + " records!";
                }
                else
                {
                    message = "No records!";
                    exist = false;
                }

                conn.desconectar();
                operSuccess = true;
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }
            return operSuccess;
        }

        public bool UpdateEmployeeCredentials(int userID, int empID, string fullName, string tel, string add, int jobID, float salary, string loginName, string email, string password, string status)
        {
            bool operSuccess = false;

            try
            {

                // open connection
                SqlCmd.Connection = conn.conectar();

                // Iniciar uma transação
                using (SqlTransaction transaction = conn.conectar().BeginTransaction())
                {
                    try
                    {
                        // Atualizar Employees
                        using (SqlCmd)
                        {
                            SqlCmd.Connection = conn.conectar();
                            SqlCmd.Transaction = transaction;
                            SqlCmd.CommandText = @"
                        UPDATE Employees
                        SET 
                            FullName = @fullName,
                            PhoneNumber = @tel,
                            Address = @add,
                            JobDeptId = @job,
                            Salary = @salary,
                            Status = @status
                        WHERE EmpId = @empID;
                    ";

                            SqlCmd.Parameters.AddWithValue("@empID", empID);
                            SqlCmd.Parameters.AddWithValue("@fullName", fullName);
                            SqlCmd.Parameters.AddWithValue("@tel", tel);
                            SqlCmd.Parameters.AddWithValue("@add", add);
                            SqlCmd.Parameters.AddWithValue("@job", jobID);
                            SqlCmd.Parameters.AddWithValue("@salary", salary);
                            SqlCmd.Parameters.AddWithValue("@status", status);

                            SqlCmd.ExecuteNonQuery();
                        }

                        // Atualizar Users
                        using (SqlCmd)
                        {
                            SqlCmd.Connection = conn.conectar();
                            SqlCmd.Transaction = transaction;
                            SqlCmd.CommandText = @"
                        UPDATE Users
                        SET 
                            LoginName = @loginName,
                            Email = @email,
                            Passwd = @password
                        WHERE UserId = @usrID;
                    ";

                            SqlCmd.Parameters.AddWithValue("@usrID", userID);
                            SqlCmd.Parameters.AddWithValue("@loginName", loginName);
                            SqlCmd.Parameters.AddWithValue("@email", email);
                            SqlCmd.Parameters.AddWithValue("@password", password);

                            SqlCmd.ExecuteNonQuery();
                        }

                        // Confirmar transação
                        transaction.Commit();
                        operSuccess = true;
                        exist = true;
                        message = "Record updated!";
                    }
                    catch (SqlException error)
                    {
                        // Reverter transação em caso de erro
                        transaction.Rollback();
                        message = "Transaction failed: " + error.Message;
                    }
                }

                conn.desconectar();
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }

            return operSuccess;
        }
        public bool UpdateClientCredentials(int userID, int cliID, string NIF, string fullName, string tel, string add, string loginName, string email, string password)
        {
            bool operSuccess = false;

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();

                // Iniciar uma transação
                using (SqlTransaction transaction = conn.conectar().BeginTransaction())
                {
                    try
                    {
                        // Atualizar Clients
                        using (SqlCmd)
                        {
                            SqlCmd.Connection = conn.conectar();
                            SqlCmd.Transaction = transaction;
                            SqlCmd.CommandText = @"
                        UPDATE Clients
                        SET 
                            FullName = @fullName,
                            NIF = @NIF,
                            PhoneNumber = @tel,
                            Address = @add
                        WHERE ClientId = @cliID;
                    ";

                            SqlCmd.Parameters.AddWithValue("@cliID", cliID);
                            SqlCmd.Parameters.AddWithValue("@fullName", fullName);
                            SqlCmd.Parameters.AddWithValue("@NIF", NIF);
                            SqlCmd.Parameters.AddWithValue("@tel", tel);
                            SqlCmd.Parameters.AddWithValue("@add", add);
                            SqlCmd.ExecuteNonQuery();
                        }

                        // Atualizar Users
                        using (SqlCmd)
                        {
                            SqlCmd.Connection = conn.conectar();
                            SqlCmd.Transaction = transaction;
                            SqlCmd.CommandText = @"
                        UPDATE Users
                        SET 
                            LoginName = @loginName,
                            Email = @email,
                            Passwd = @password
                        WHERE UserId = @usrID;
                    ";

                            SqlCmd.Parameters.AddWithValue("@usrID", userID);
                            SqlCmd.Parameters.AddWithValue("@loginName", loginName);
                            SqlCmd.Parameters.AddWithValue("@email", email);
                            SqlCmd.Parameters.AddWithValue("@password", password);

                            SqlCmd.ExecuteNonQuery();
                        }

                        // Confirmar transação
                        transaction.Commit();
                        operSuccess = true;
                        exist = true;
                        message = "Record updated!";
                    }
                    catch (SqlException error)
                    {
                        // Reverter transação em caso de erro
                        transaction.Rollback();
                        message = "Transaction failed: " + error.Message;
                    }
                }

                conn.desconectar();
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }

            return operSuccess;
        }


        public bool UpdateProductCredentials(int prodID, string prodName, int category, string desc, decimal price)
        {
            bool operSuccess = false;

            SqlCmd.Parameters.AddWithValue("@prdID", prodID);
            SqlCmd.Parameters.AddWithValue("@prdName", prodName);
            SqlCmd.Parameters.AddWithValue("@prdCat", category);
            SqlCmd.Parameters.AddWithValue("@prdDesc", desc);
            SqlCmd.Parameters.AddWithValue("@prdPrice", price);

            SqlCmd.CommandText = "update Products set ProductName = @prdName, Description = @prdDesc, CategoryID = @prdCat, Price = @prdPrice where ProductId = @prdID";

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                // execute sql statement
                SqlCmd.ExecuteNonQuery();
                conn.desconectar();
                operSuccess = true;
                exist = true;
                message = "Record updated!";
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }

            return operSuccess;
        }

        public bool UpdateStockCredentials(int prdID, int sizeID, int newQtd, DateTime dataUpdt)
        {
            bool operSuccess = false;

            SqlCmd.Parameters.AddWithValue("@prdID", prdID);
            SqlCmd.Parameters.AddWithValue("@sizeID", sizeID);
            SqlCmd.Parameters.AddWithValue("@newQTD", newQtd);
            SqlCmd.Parameters.AddWithValue("@dtUpdt", dataUpdt);

            SqlCmd.CommandText = "update Stock set Quantity = @newQTD, LastUpdated = @dtUpdt where ProductID = @prdID and SizeID = sizeID";

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                // execute sql statement
                SqlCmd.ExecuteNonQuery();
                conn.desconectar();
                operSuccess = true;
                exist = true;
                message = "Record updated!";
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }

            return operSuccess;
        }

        public bool deleteProduct(int prodID)
        {
            bool operSuccess = false;

            SqlCmd.Parameters.AddWithValue("@prdID", prodID);
            SqlCmd.CommandText = "delete from Products where ProductId = @prdID";

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                // execute sql statement
                SqlCmd.ExecuteNonQuery();
                conn.desconectar();
                operSuccess = true;
                exist = true;
                message = "Record deleted!";
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }
            return operSuccess;
        }

        public bool deleteStock(int delPrdID, int delSizeID)
        {
            bool operSuccess = false;

            SqlCmd.Parameters.AddWithValue("@prdID", delPrdID);
            SqlCmd.Parameters.AddWithValue("@sizeID", delSizeID);
            SqlCmd.CommandText = "delete from Stock where ProductID = @prdID and SizeID = @sizeID";

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                // execute sql statement
                SqlCmd.ExecuteNonQuery();
                conn.desconectar();
                operSuccess = true;
                exist = true;
                message = "Record deleted!";
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }
            return operSuccess;
        }


        public bool deleteEmployee(int usrID, int empID)
        {
            bool operSuccess = false;

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();

                // Iniciar uma transação
                using (SqlTransaction transaction = conn.conectar().BeginTransaction())
                {
                    try
                    {
                        // Excluir de Employees
                        using (SqlCmd)
                        {
                            SqlCmd.Connection = conn.conectar();
                            SqlCmd.Transaction = transaction;
                            SqlCmd.CommandText = "DELETE FROM Employees WHERE EmpId = @empID;";
                            SqlCmd.Parameters.AddWithValue("@empID", empID);
                            SqlCmd.ExecuteNonQuery();
                        }

                        // Excluir de Users
                        using (SqlCmd)
                        {
                            SqlCmd.Connection = conn.conectar();
                            SqlCmd.Transaction = transaction;
                            SqlCmd.CommandText = "DELETE FROM Users WHERE UserId = @usrID;";
                            SqlCmd.Parameters.AddWithValue("@usrID", usrID);
                            SqlCmd.ExecuteNonQuery();
                        }

                        // Confirmar transação
                        transaction.Commit();
                        operSuccess = true;
                        exist = true;
                        message = "Record deleted!";
                    }
                    catch (SqlException error)
                    {
                        // Reverter transação em caso de erro
                        transaction.Rollback();
                        message = "Transaction failed: " + error.Message;
                    }
                }

                conn.desconectar();
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }

            return operSuccess;
        }

        public bool deleteSale(int saleId)
        {
            bool operSuccess = false;

            SqlCmd.Parameters.AddWithValue("@saleId", saleId);
            SqlCmd.CommandText = "delete from Sales where SaleId = @saleId";

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                // execute sql statement
                SqlCmd.ExecuteNonQuery();
                conn.desconectar();
                operSuccess = true;
                exist = true;
                message = "Record deleted!";
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }
            return operSuccess;
        }




        public bool clientList(ref Client[] clients)
        {
            // statement sql to receive / list the credentials records
            SqlCmd.CommandText = "SELECT u.UserId, c.ClientId, c.FullName, c.NIF, c.PhoneNumber , c.Address, u.LoginName, u.Email, u.Passwd FROM Users u INNER JOIN Clients c ON u.UserId = c.UserId ORDER BY c.ClientId ASC;";
            bool operSuccess = false;

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();
                // execute sql statement
                dr = SqlCmd.ExecuteReader();
                if (dr.HasRows)
                {
                    exist = true;

                    int userID;
                    int clientID;
                    string fullName;
                    string NIF;
                    string tel;
                    string address;
                    string username;
                    string email;
                    string passwd;

                    int i = 0; // record counter

                    while (dr.Read()) // read while TRUE (existing records to read)
                    {
                        userID = dr.GetInt32(0);
                        clientID = dr.GetInt32(1);
                        fullName = dr.GetString(2);
                        NIF = dr.GetString(3);
                        tel = dr.GetString(4);
                        address = dr.GetString(5);
                        username = dr.GetString(6);
                        email = dr.GetString(7);
                        passwd = dr.GetString(8);

                        // array redimention
                        Array.Resize(ref clients, clients.Length + 1);
                        // create memory
                        clients[clients.Length - 1] = new Client();
                        // fill the array
                        clients[clients.Length - 1].userID = userID;
                        clients[clients.Length - 1].clientID = clientID;
                        clients[clients.Length - 1].fullName = fullName;
                        clients[clients.Length - 1].NIF = NIF;
                        clients[clients.Length - 1].tel = tel;
                        clients[clients.Length - 1].address = address;
                        clients[clients.Length - 1].username = username;
                        clients[clients.Length - 1].email = email;
                        clients[clients.Length - 1].passwd = passwd;
                        i++;
                    }
                    message = "Retriving: " + i + " records!";
                }
                else
                {
                    message = "No records!";
                    exist = false;
                }

                conn.desconectar();
                operSuccess = true;
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }
            return operSuccess;
        }

        

        public bool deleteClient(int usrID, int cliID)
        {
            bool operSuccess = false;

            try
            {
                // open connection
                SqlCmd.Connection = conn.conectar();

                // Iniciar uma transação
                using (SqlTransaction transaction = conn.conectar().BeginTransaction())
                {
                    try
                    {
                        // Excluir de Clients
                        using (SqlCmd)
                        {
                            SqlCmd.Connection = conn.conectar();
                            SqlCmd.Transaction = transaction;
                            SqlCmd.CommandText = "DELETE FROM Clients WHERE ClientId = @cliID;";
                            SqlCmd.Parameters.AddWithValue("@cliID", cliID);
                            SqlCmd.ExecuteNonQuery();
                        }

                        // Excluir de Users
                        using (SqlCmd)
                        {
                            SqlCmd.Connection = conn.conectar();
                            SqlCmd.Transaction = transaction;
                            SqlCmd.CommandText = "DELETE FROM Users WHERE UserId = @usrID;";
                            SqlCmd.Parameters.AddWithValue("@usrID", usrID);
                            SqlCmd.ExecuteNonQuery();
                        }

                        // Confirmar transação
                        transaction.Commit();
                        operSuccess = true;
                        exist = true;
                        message = "Record deleted!";
                    }
                    catch (SqlException error)
                    {
                        // Reverter transação em caso de erro
                        transaction.Rollback();
                        message = "Transaction failed: " + error.Message;
                    }
                }

                conn.desconectar();
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }

            return operSuccess;
        }

        //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA

        public List<Dictionary<string, object>> GetProductsFromStock()
        {
            List<Dictionary<string, object>> productData = new List<Dictionary<string, object>>();
            Dictionary<int, Dictionary<string, object>> groupedProducts = new Dictionary<int, Dictionary<string, object>>();

            try
            {
                SqlConnection connection = conn.conectar();

                string query = @"
                SELECT s.ProductId, s.SizeID, sz.SizeName, p.ProductName, p.Description, p.Price, s.Quantity, p.Image 
                FROM Stock s 
                INNER JOIN Products p ON s.ProductId = p.ProductId 
                INNER JOIN Sizes sz ON s.SizeID = sz.SizeID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        int productId = Convert.ToInt32(dr["ProductId"]);

                        if (!groupedProducts.ContainsKey(productId))
                        {
                            groupedProducts[productId] = new Dictionary<string, object>
                    {
                        {"ProductId", dr["ProductId"] },
                        {"ProductName", dr["ProductName"] },
                        {"Description", dr["Description"] },
                        {"Price", dr["Price"] },
                        {"Quantity", dr["Quantity"] },
                        {"ImagePath", dr["Image"] },
                        {"Sizes", new List<string>() }
                    };
                        }

                        // Adiciona o tamanho à lista de tamanhos
                        ((List<string>)groupedProducts[productId]["Sizes"]).Add(dr["SizeName"].ToString());
                    }

                    dr.Close();
                }

                conn.desconectar();

                // Converte os produtos agrupados para a lista final
                productData = groupedProducts.Values.ToList();
            }
            catch (SqlException error)
            {
                message = "Database Error: " + error.ErrorCode + " " + error.Message;
            }

            return productData;
        }

        public List<string> GetProductSizes(int productID)
        {
            List<string> sizes = new List<string>();

            // Inicia a conexão
            SqlConnection connection = conn.conectar(); // Supondo que conn seja sua classe de conexão

            string sizeQuery = @"
            SELECT SizeName 
            FROM Sizes s
            JOIN Stock st ON s.SizeID = st.SizeID
            WHERE st.ProductID = @ProductID";

            try
            {
                // Iniciar uma transação (caso precise em um futuro uso)
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Executar a consulta
                        using (SqlCommand sizeCommand = new SqlCommand(sizeQuery, connection))
                        {
                            sizeCommand.Transaction = transaction;
                            sizeCommand.Parameters.Add(new SqlParameter("@ProductID", productID));

                            // Executar a leitura dos dados
                            using (SqlDataReader sizeReader = sizeCommand.ExecuteReader())
                            {
                                while (sizeReader.Read())
                                {
                                    sizes.Add(sizeReader["SizeName"].ToString());
                                }
                            }
                        }

                        // Confirmar a transação
                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        // Reverter a transação em caso de erro
                        transaction.Rollback();
                        MessageBox.Show("Erro ao carregar os tamanhos: " + ex.Message);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message);
            }
            finally
            {
                // Fechar a conexão
                conn.desconectar();
            }

            return sizes;
        }

        public List<string> GetCategories()
        {
            List<string> categories = new List<string>();
            try
            {
                SqlConnection connection = conn.conectar();
                string query = "SELECT DISTINCT CategoryID,CategoryName FROM Categories";
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(reader["CategoryName"].ToString());
                }

                reader.Close();
                conn.desconectar();
            }
            catch (SqlException ex)
            {
                message = "Erro ao carregar categorias: " + ex.Message;
            }

            return categories;
        }

        public List<Dictionary<string, object>> GetProductsByCategory(string category)
        {
            List<Dictionary<string, object>> products = new List<Dictionary<string, object>>();
            try
            {
                SqlConnection connection = conn.conectar();
                string query = @"
                SELECT s.ProductId, p.ProductName, p.Description, p.Price, p.Image
                FROM Stock s
                INNER JOIN Products p ON s.ProductId = p.ProductId
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                WHERE c.CategoryName = @Category
                GROUP BY s.ProductId, p.ProductName, p.Description, p.Price, p.Image
                HAVING COUNT(s.SizeID) > 0";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Category", category);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Dictionary<string, object>
            {
                {"ProductId", reader["ProductId"] },
                {"ProductName", reader["ProductName"] },
                {"Description", reader["Description"] },
                {"Price", reader["Price"] },
                {"ImagePath", reader["Image"] }
            });
                }

                reader.Close();
                conn.desconectar();
            }
            catch (SqlException ex)
            {
                message = "Erro ao filtrar produtos por categoria: " + ex.Message;
            }

            return products;
        }

        public int ObterProductIdPorNome(string productName)
        {
            int productId; // Inicializar com um valor inválido
            try
            {
                // Usar a mesma abordagem de conexão que foi utilizada na função GetProductsByCategory
                SqlConnection connection = new Connection().conectar();

                string query = "SELECT ProductId FROM Products WHERE ProductName = @ProductName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductName", productName);

                object result = command.ExecuteScalar();

                // Verificar se o resultado é nulo e retornar o ID do produto
                if (result != null)
                {
                    productId = Convert.ToInt32(result);
                }
                else
                {
                    throw new Exception("Produto não encontrado.");
                }

                connection.Close(); // Fechar a conexão com o banco
            }
            catch (SqlException ex)
            {
                // Caso ocorra erro na execução da consulta, lançar uma exceção
                throw new Exception("Erro ao obter ProductId: " + ex.Message);
            }

            return productId;
        }

        // Obter o SizeId pelo nome do tamanho
        public int ObterSizeIdPorNome(string size)
        {
            int sizeId;
            try
            {
                // Usar a mesma abordagem de conexão que foi utilizada nas outras funções
                SqlConnection connection = new Connection().conectar();

                string query = "SELECT SizeId FROM Sizes WHERE SizeName = @SizeName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SizeName", size);

                object result = command.ExecuteScalar();

                // Verificar se o resultado é nulo e retornar o ID do tamanho
                if (result != null)
                {
                    sizeId = Convert.ToInt32(result);
                }
                else
                {
                    throw new Exception("Tamanho não encontrado.");
                }

                connection.Close(); // Fechar a conexão com o banco
            }
            catch (SqlException ex)
            {
                // Caso ocorra erro na execução da consulta, lançar uma exceção
                throw new Exception("Erro ao obter SizeId: " + ex.Message);
            }

            return sizeId;
        }

        // Inserir a venda na tabela Sales
        public void InserirVenda(int empID, int clientId, int productId, int sizeId, int quantity, decimal total)
        {

            try
            {
                // Iniciar uma transação para garantir consistência dos dados
                SqlTransaction transaction = conn.conectar().BeginTransaction();

                // Inserir venda na tabela Sales
                string querySale = @"
                 INSERT INTO Sales (ClientId, EmployeeId, ProductId, SizeId, Quantity, SaleDate, Total)
                 VALUES (@ClientId, @EmpID, @ProductId, @SizeId, @Quantity, @SaleDate, @Total)";

                SqlCommand commandSale = new SqlCommand(querySale, conn.conectar(), transaction);
                commandSale.Parameters.AddWithValue("@ClientId", clientId);
                commandSale.Parameters.AddWithValue("@EmpID", empID);
                commandSale.Parameters.AddWithValue("@ProductId", productId);
                commandSale.Parameters.AddWithValue("@SizeId", sizeId);
                commandSale.Parameters.AddWithValue("@Quantity", quantity);
                commandSale.Parameters.AddWithValue("@SaleDate", DateTime.Now);
                commandSale.Parameters.AddWithValue("@Total", total);

                commandSale.ExecuteNonQuery(); // Executar a inserção

                // Atualizar a quantidade do produto na tabela Stock
                string queryStock = @"
                     UPDATE Stock
                     SET Quantity = Quantity - @Quantity, LastUpdated = @LastUpdated
                     WHERE ProductID = @ProductId AND SizeID = @SizeId";

                    SqlCommand commandStock = new SqlCommand(queryStock, conn.conectar(), transaction);
                    commandStock.Parameters.AddWithValue("@Quantity", quantity);
                    commandStock.Parameters.AddWithValue("@ProductId", productId);
                    commandStock.Parameters.AddWithValue("@SizeId", sizeId);
                    commandStock.Parameters.AddWithValue("@LastUpdated", DateTime.Now);

                int rowsAffected = commandStock.ExecuteNonQuery(); // Executar a atualização

                // Verificar se a quantidade no estoque foi suficiente
                if (rowsAffected == 0)
                {
                    // Se não afetou nenhuma linha, significa que não havia estoque suficiente ou produto não encontrado
                    throw new Exception("Estoque insuficiente ou produto não encontrado.");
                }

                // Commit da transação se tudo ocorreu corretamente
                transaction.Commit();

                conn.conectar().Close(); // Fechar a conexão com o banco
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao inserir venda: " + ex.Message);
            }
        }
    }
}
