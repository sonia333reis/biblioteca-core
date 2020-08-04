using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using biblioteca.Models;
using biblioteca.Repositories;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace biblioteca.Controllers
{
    public class EmployeeController : Controller
    {
        const string objectValue = "Employee";

        private Connection connection { get; set; }

        public EmployeeController(Connection connection)
        {
            this.connection = connection;
        }

        public IActionResult SelectAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
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

                    Employee emp = new Employee()
                    {
                        EmployeeID = Convert.ToInt32(connection.rowReaderForSimpleObjects(resultRow, "EmployeeId", false)),
                        Name = connection.rowReaderForSimpleObjects(resultRow, "EmployeeName", false),
                        Cpf = connection.rowReaderForSimpleObjects(resultRow, "EmployeeCpf", false),
                        Email = connection.rowReaderForSimpleObjects(resultRow, "EmployeeEmail", false),
                        Age = Convert.ToInt32(connection.rowReaderForSimpleObjects(resultRow, "EmployeeAge", true))
                    };
                    employees.Add(emp);
                }
            }
            return View(employees);
        }

        public IActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateEmployee(Employee emp)
        {
            var cmd = connection._con.CreateCommand() as MySqlCommand;
            try
            {
                if (ModelState.IsValid)
                {
                    emp.Cpf = emp.Cpf.Replace(".", "");
                    emp.Cpf = emp.Cpf.Replace("-", "");

                    cmd.CommandText = "INSERT INTO " + objectValue + "s values(0, @uName, @uCpf, @uEmail, @uAge); ";
                    cmd.Parameters.AddWithValue("@uName", emp.Name);
                    cmd.Parameters.AddWithValue("@uCpf", emp.Cpf);
                    cmd.Parameters.AddWithValue("@uEmail", emp.Email);
                    cmd.Parameters.AddWithValue("@uAge", emp.Age);

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return RedirectToAction("SelectAllEmployees");
            }
            catch
            {
                throw;
            }
        }

        public IActionResult UpdateEmployee(int id)
        {
            Employee emp = new Employee();
            var cmd = connection._con.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"call selectEmployee(@eId)";
            cmd.Parameters.AddWithValue("@eId", id);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                emp = new Employee()
                {
                    EmployeeID = Convert.ToInt32(reader["EmployeeId"]),
                    Name = Convert.ToString(reader["EmployeeName"]),
                    Cpf = Convert.ToString(reader["EmployeeCpf"]),
                    Email = Convert.ToString(reader["EmployeeEmail"]),
                    Age = Convert.ToInt32(reader["EmployeeAge"])
                };
            }

            connection._con.Dispose();
            ModelState.Clear();

            return View(emp);
        }

        [HttpPost]
        public IActionResult UpdateEmployee(int id, Employee emp)
        {
            var cmd = connection._con.CreateCommand() as MySqlCommand;
            try
            {
                cmd.CommandText = "call updateEmployee(@eName, @eCpf, @eEmail, @eAge, @eId)";
                cmd.Parameters.AddWithValue("@eName", emp.Name);
                cmd.Parameters.AddWithValue("@eCpf", emp.Cpf);
                cmd.Parameters.AddWithValue("@eEmail", emp.Email);
                cmd.Parameters.AddWithValue("@eAge", emp.Age);
                cmd.Parameters.AddWithValue("@eId", id);
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
                return RedirectToAction("SelectAllEmployees");
            }
            catch
            {
                throw;
            }
        }

        public IActionResult DeleteEmployee(int id)
        {
            var cmd = connection._con.CreateCommand() as MySqlCommand;

            try
            {
                cmd.CommandText = "call deleteEmployee(@eId)";
                cmd.Parameters.AddWithValue("@eId", id);
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
                return RedirectToAction("SelectAllEmployees");
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

                List<Employee> employees = new List<Employee>();

                var cmd = connection._con.CreateCommand() as MySqlCommand;
                cmd.CommandText = "select * from employees where EmployeeName = @name";
                cmd.Parameters.AddWithValue("@name", searchString);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Employee emp = new Employee()
                    {
                        EmployeeID = Convert.ToInt32(reader["EmployeeId"]),
                        Name = Convert.ToString(reader["EmployeeName"]),
                        Cpf = Convert.ToString(reader["EmployeeCpf"]),
                        Email = Convert.ToString(reader["EmployeeEmail"]),
                        Age = Convert.ToInt32(reader["EmployeeAge"]),
                    };

                    employees.Add(emp);
                }

                connection._con.Dispose();
                ModelState.Clear();
                return View("SelectAllEmployees", employees);
            }
            else
            {
                ViewBag.Mensagem = "Não existem usuários com este nome";
                return RedirectToAction("SelectAllEmployees");
            }
        }
    }

}