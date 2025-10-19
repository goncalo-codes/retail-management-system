using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projetoLoja.Data
{
    internal class Connection
    {
        SqlConnection conn = new SqlConnection(); // communication token attribute

        public Connection()
        {
            // connection string            
            conn.ConnectionString = "Data Source=localhost;Initial Catalog=lojaDB;Integrated Security=True;TrustServerCertificate=True;"; //colocar a sua devida connection string
        }

        public SqlConnection conectar()
        {
            // database connecting
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();

            return conn;
        }

        public void desconectar()
        {
            // database disconecting
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
    }
}
