using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace biblioteca.Repositories
{
    public class Connection : IDisposable
    {
        public MySqlConnection _con;

        public  Connection(string connectionString)
        {
            _con = new MySqlConnection(connectionString);
            _con.Open();
        }

        public void Open()
        {
            _con.Open();
        }

        public void Dispose()
        {
            _con.Close();
        }

    }
}
