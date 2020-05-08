using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using biblioteca.Models;
using biblioteca.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace biblioteca.Controllers
{
    public class BookController : Controller
    {
        const string objectValue = "Book";
        private Connection connection { get; set; }

        public BookController(Connection connection)
        {
            this.connection = connection;
        }

        public IActionResult SelectAllBooks()
        {
            List<Book> books = new List<Book>();
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

                    Book book = new Book() {
                        BookID = Convert.ToInt32(connection.rowReaderForSimpleObjects(resultRow, "BookId", false)),
                        Name = connection.rowReaderForSimpleObjects(resultRow, "BookName", false),
                        Writter = connection.rowReaderForSimpleObjects(resultRow, "BookWritter", false),
                        Release = Convert.ToDateTime(connection.rowReaderForSimpleObjects(resultRow, "BookRelease", true))
                    };
                    books.Add(book);
                }
            }
            return View(books);
        }

        public IActionResult CreateBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBook(Book book)
        {
            var cmd = connection._con.CreateCommand() as MySqlCommand;
            try
            {
                if (ModelState.IsValid)
                {
                    cmd.CommandText = "call createBook(@bName, @bWritter, @bRelease)";
                    cmd.Parameters.AddWithValue("@bName", book.Name);
                    cmd.Parameters.AddWithValue("@bWritter", book.Writter);
                    cmd.Parameters.AddWithValue("@bRelease", book.Release);

                    int i = cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (i >= 1)
                    {
                        ViewBag.Mensagem = "Livro cadastrado com sucesso!";
                    }
                    else
                    {
                        ViewBag.Mensagem = "Não foi possível completar a operação!";
                    }
                }
                return RedirectToAction("selectAllBooks");
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult UpdateBook(int id)
        {
            Book book = new Book();
            var cmd = connection._con.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"call selectBook(@bId)";
            cmd.Parameters.AddWithValue("@bId", id);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                book = new Book()
                {
                    BookID = Convert.ToInt32(reader["BookId"]),
                    Name = Convert.ToString(reader["BookName"]),
                    Writter = Convert.ToString(reader["BookWritter"]),
                    Release = Convert.ToDateTime(reader["BookRelease"])
                };
            }

            connection._con.Dispose();
            ModelState.Clear();

            return View(book);
        }

        [HttpPost]
        public IActionResult UpdateBook(int id, Book book)
        {
            var cmd = connection._con.CreateCommand() as MySqlCommand;

            try
            {
                cmd.CommandText = "call updateBook(@bName, @bWritter, @bRelease, @bId)";
                cmd.Parameters.AddWithValue("@bName", book.Name);
                cmd.Parameters.AddWithValue("@bWritter", book.Writter);
                cmd.Parameters.AddWithValue("@bRelease", book.Release);
                cmd.Parameters.AddWithValue("@bId", id);
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
                return RedirectToAction("SelectAllBooks");
            }
            catch
            {
                throw;
            }
        }

        public IActionResult DeleteBook(int id)
        {
            var cmd = connection._con.CreateCommand() as MySqlCommand;

            try
            {
                cmd.CommandText = "call deleteBook(@bId)";
                cmd.Parameters.AddWithValue("@bId", id);
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
                return RedirectToAction("SelectAllBooks");
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

                List<Book> books = new List<Book>();

                var cmd = connection._con.CreateCommand() as MySqlCommand;
                cmd.CommandText = "select * from books where BookName = @name";
                cmd.Parameters.AddWithValue("@name", searchString);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Book book = new Book()
                    {
                        BookID = Convert.ToInt32(reader["BookId"]),
                        Name = Convert.ToString(reader["BookName"]),
                        Writter = Convert.ToString(reader["BookWritter"]),
                        Release = Convert.ToDateTime(reader["BookRelease"])
                    };

                    books.Add(book);
                }

                connection._con.Dispose();
                ModelState.Clear();
                return View("SelectAllBooks", books);
            }
            else
            {
                ViewBag.Mensagem = "Não existem livros com este nome";
                return RedirectToAction("SelectAllBooks");
            }
        }
    }
}