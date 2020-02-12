using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using biblioteca.Models;
using MySql.Data.MySqlClient;

namespace biblioteca.Repositories
{
    public class adoCrudUser
    {
        private MySqlConnection _con;

        private void Connection() 
        {
            _con = new MySqlConnection("DefaultConnection");
        }
    }
}
