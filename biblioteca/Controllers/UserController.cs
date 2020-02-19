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
        private Connection connection { get; set; }

        public UserController(Connection connection)
        {
            this.connection = connection;
        }

        public IActionResult SelectAllUsers()
        {
            List<User> users = new List<User>();

            var cmd = connection._con.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"call selectAllUsers()";
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                User user = new User()
                {
                    UserID = Convert.ToInt32(reader["UserId"]),
                    Nome = Convert.ToString(reader["UserName"]),
                    Cpf = Convert.ToString(reader["UserCpf"]),
                    Email = Convert.ToString(reader["UserEmail"])
                };

                users.Add(user);
            }

            connection._con.Dispose();
            ModelState.Clear();
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
                    cmd.CommandText = "call createUser(@uName, @uCpf, @uEmail)";
                    cmd.Parameters.AddWithValue("@uName", user.Nome);
                    cmd.Parameters.AddWithValue("@uCpf", user.Cpf);
                    cmd.Parameters.AddWithValue("@uEmail", user.Email);

                    int i = cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (i >= 1) 
                    {
                        ViewBag.Mensagem = "Usuário cadastrado com sucesso!";
                    }
                    else 
                    {
                        ViewBag.Mensagem = "Não foi possível completar a operação!";
                    }
                }
                return RedirectToAction("selectAllUsers");
            }
            catch 
            {
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
                    Nome = Convert.ToString(reader["UserName"]),
                    Cpf = Convert.ToString(reader["UserCpf"]),
                    Email = Convert.ToString(reader["UserEmail"])
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
                cmd.CommandText = "call updateUser(@uName, @uCpf, @uEmail, @uId)";
                cmd.Parameters.AddWithValue("@uName", user.Nome);
                cmd.Parameters.AddWithValue("@uCpf", user.Cpf);
                cmd.Parameters.AddWithValue("@uEmail", user.Email);
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

    }
}