using System;
using System.Collections.Generic;
using System.Data;
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

        public bool createUser(User user) 
        {
            Connection();

            int i;

            using (MySqlCommand command = new MySqlCommand("createUser",_con)) 
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@uName", user.Nome);
                command.Parameters.AddWithValue("@uCpf", user.Cpf);
                command.Parameters.AddWithValue("@uEmail", user.Email);
                _con.Open();
                i = command.ExecuteNonQuery();
            }
            _con.Close();

            return i >= 1;
        }

        public List<User> selectAllUsers() 
        {
            Connection();
            List<User> users = new List<User>();

            using (MySqlCommand command = new MySqlCommand("selectAllUsers", _con))
            {
                command.CommandType = CommandType.StoredProcedure;
                _con.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) 
                {
                    User user = new User()
                    {
                        Nome = Convert.ToString(reader["UserName"]),
                        Cpf = Convert.ToString(reader["UserCpf"]),
                        Email = Convert.ToString(reader["UserEmail"])
                    };

                    users.Add(user);
                }
            }
            _con.Close();

            return users;
        }

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
        }
    }
}
