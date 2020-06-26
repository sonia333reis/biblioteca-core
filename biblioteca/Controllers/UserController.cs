using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using biblioteca.Repositories;
using biblioteca.Models;
using MySql.Data.MySqlClient;


namespace biblioteca.Controllers
{
    public class UserController : Controller
    {
        const string objectValue = "User";
        private Connection connection { get; set; }

        public UserController(Connection connection)
        {
            this.connection = connection;
        }

        public IActionResult SelectAllUsers()
        {
            List<User> users = new List<User>();
            List<string> result = connection.SelectAllSimpleObjects(objectValue);

            if (result != null)
            {
                while (result.Count > 0)
                {
                    string resultRow = "";

                    //busca pelo fim da linha
                    int rowIndex = result.IndexOf("endrow");

                    for (int i = 0; i < rowIndex; i++)
                    {
                        // pega a primeira coluna da linha e a retira da lista inicial.
                        resultRow = resultRow + ";" + result[0].ToString();
                        result.RemoveAt(0);
                    }

                    // retira a linha
                    result.RemoveAt(0);

                    User user = new User()
                    {
                        UserID = Convert.ToInt32(connection.rowReaderForSimpleObjects(resultRow, "UserId", false)),
                        Name = connection.rowReaderForSimpleObjects(resultRow, "UserName", false),
                        Cpf = connection.rowReaderForSimpleObjects(resultRow, "UserCpf", false),
                        Email = connection.rowReaderForSimpleObjects(resultRow, "UserEmail", false),
                        Age = Convert.ToInt32(connection.rowReaderForSimpleObjects(resultRow, "UserAge", true)),
                    };
                    users.Add(user);
                }
            }
            return View(users);
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            var cmd = connection._con.CreateCommand() as MySqlCommand;
            try 
            {
                if (ModelState.IsValid) 
                {
                    user.Cpf = user.Cpf.Replace(".", "");
                    user.Cpf = user.Cpf.Replace("-", "");

                    cmd.CommandText = "INSERT INTO " + objectValue +"s values(0, @uName, @uCpf, @uEmail, @uAge, @uPass); ";
                    cmd.Parameters.AddWithValue("@uName", user.Name);
                    cmd.Parameters.AddWithValue("@uCpf", user.Cpf);
                    cmd.Parameters.AddWithValue("@uEmail", user.Email);
                    cmd.Parameters.AddWithValue("@uAge", user.Age);
                    cmd.Parameters.AddWithValue("@uPass", user.Pass);

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return RedirectToAction("selectAllUsers");
            } catch {
                throw;
            }
        }

        [HttpGet]
        public IActionResult UpdateUser(int id)
        {
            User user = new User();
            var cmd = connection._con.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"call selectUser(@uId)";
            cmd.Parameters.AddWithValue("@uId", id);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                user = new User()
                {
                    UserID = Convert.ToInt32(reader["UserId"]),
                    Name = Convert.ToString(reader["UserName"]),
                    Cpf = Convert.ToString(reader["UserCpf"]),
                    Email = Convert.ToString(reader["UserEmail"]),
                    Age = Convert.ToInt32(reader["UserAge"]),
                    Pass = Convert.ToString(reader["UserPass"])
                };
            }

            connection._con.Dispose();
            ModelState.Clear();

            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateUser(int id, User user)
        {
            var cmd = connection._con.CreateCommand() as MySqlCommand;

            try
            {
                cmd.CommandText = "call updateUser(@uName, @uCpf, @uEmail, @uAge, @uPass, @uId)";
                cmd.Parameters.AddWithValue("@uName", user.Name);
                cmd.Parameters.AddWithValue("@uCpf", user.Cpf);
                cmd.Parameters.AddWithValue("@uEmail", user.Email);
                cmd.Parameters.AddWithValue("@uAge", user.Age);
                cmd.Parameters.AddWithValue("@uPass", user.Pass);
                cmd.Parameters.AddWithValue("@uId", id);
                int i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                if (i >= 1)
                {
                    ViewBag.Mensagem = "Dados atualizados com sucesso!";
                }
                else
                {
                    ViewBag.Mensagem = "Não foi possível completar a operação!";
                }
                return RedirectToAction("SelectAllUsers");
            }
            catch
            {
                throw;
            }
        }

        public IActionResult DeleteUser(int id) 
        {
            var cmd = connection._con.CreateCommand() as MySqlCommand;

            try
            {
                cmd.CommandText = "call deleteUser(@uId)";
                cmd.Parameters.AddWithValue("@uId", id);
                int i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                if (i >= 1)
                {
                    ViewBag.Mensagem = "Dados excluídos com sucesso!";
                }
                else
                {
                    ViewBag.Mensagem = "Não foi possível completar a operação!";
                }
                return RedirectToAction("SelectAllUsers");
            }
            catch
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Search(string searchString)
        {
            ViewBag.Pesquisa = "";
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.Pesquisa = searchString;

                List<User> users = new List<User>();

                var cmd = connection._con.CreateCommand() as MySqlCommand;
                cmd.CommandText = "select * from users where UserName = @name";
                cmd.Parameters.AddWithValue("@name", searchString);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User()
                    {
                        UserID = Convert.ToInt32(reader["UserId"]),
                        Name = Convert.ToString(reader["UserName"]),
                        Cpf = Convert.ToString(reader["UserCpf"]),
                        Email = Convert.ToString(reader["UserEmail"]),
                        Age = Convert.ToInt32(reader["UserAge"]),
                    };

                    users.Add(user);
                }

                connection._con.Dispose();
                ModelState.Clear();
                return View("SelectAllUsers", users);
            }
            else
            {
                ViewBag.Mensagem = "Não existem usuários com este nome";
                return RedirectToAction("SelectAllUsers");
            }
        }
    }
}