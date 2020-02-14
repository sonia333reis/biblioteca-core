using System;
using System.Collections.Generic;
using System.Data;
using biblioteca.Models;
using MySql.Data.MySqlClient;

namespace biblioteca.Repositories
{
    public class adoCrudUser
    {
        /*
        public bool updateUser(User user)
        {
            Connection();

            int i;

            using (MySqlCommand command = new MySqlCommand("updateUser", _con))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@uName", user.Nome);
                command.Parameters.AddWithValue("@uCpf", user.Cpf);
                command.Parameters.AddWithValue("@uEmail", user.Email);
                command.Parameters.AddWithValue("@uId", user.UserID);
                _con.Open();
                i = command.ExecuteNonQuery();
            }
            _con.Close();

            return i >= 1;
        }

        public bool deleteUser(int id)
        {
            Connection();

            int i;

            using (MySqlCommand command = new MySqlCommand("deleteUser", _con))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@uId",id);
                _con.Open();
                i = command.ExecuteNonQuery();
            }
            _con.Close();

            return i >= 1;
        }*/
    }

}
