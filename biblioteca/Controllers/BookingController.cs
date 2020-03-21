﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using biblioteca.Models;
using biblioteca.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;

namespace biblioteca.Controllers
{
    public class BookingController : Controller
    {
        private Connection connection { get; set; }

        public BookingController(Connection connection)
        {
            this.connection = connection;
        }
        public IActionResult SelectAllBookings()
        {
            List<Booking> bookings = new List<Booking>();
            var cmd = connection._con.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"call selectAllBookings()";
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Booking booking = new Booking()
                {
                    BookingID = Convert.ToInt32(reader["BookingId"]),
                    BookID = Convert.ToInt32(reader["BookingId_BookId"]),
                    UserID = Convert.ToInt32(reader["BookingId_UserId"])
                };

                bookings.Add(booking);
            }
            connection._con.Dispose();
            ModelState.Clear();

            if (bookings.Count > 0) 
            {
                foreach (var item in bookings)
                {
                    item.Book = assignBook(item.BookID);
                    item.User = assignUser(item.UserID);
                }
            }
            
            return View(bookings);
        }

        private User assignUser(int userId) {

            User user = new User();
            connection._con.Open();
            var cmd = connection._con.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"call selectUser(@uId)";
            cmd.Parameters.AddWithValue("@uId", userId);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                user = new User()
                {
                    UserID = Convert.ToInt32(reader["UserId"]),
                    Name = Convert.ToString(reader["UserName"]),
                    Cpf = Convert.ToString(reader["UserCpf"]),
                    Email = Convert.ToString(reader["UserEmail"])
                };
            }

            connection._con.Dispose();
            return user;
        }

        private Book assignBook(int bookId)
        {
            Book book = new Book();
            connection._con.Open();
            var cmd = connection._con.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"call selectBook(@bId)";
            cmd.Parameters.AddWithValue("@bId", bookId);
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
            return book;
        }

        public IActionResult CreateBooking()
        {
            List<User> users = new List<User>();
            List<Book> books = new List<Book>();

            var cmd = connection._con.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"call selectAllUsers()";
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                User user = new User()
                {
                    UserID = Convert.ToInt32(reader["UserId"]),
                    Name = Convert.ToString(reader["UserName"]),
                    Cpf = Convert.ToString(reader["UserCpf"]),
                    Email = Convert.ToString(reader["UserEmail"])
                };

                users.Add(user);
            }
            cmd.Dispose();
            cmd.CommandText = @"call selectAllBooks()";
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Book book = new Book()
                {
                    BookID = Convert.ToInt32(reader["BookId"]),
                    Name = Convert.ToString(reader["BookName"]),
                    Writter = Convert.ToString(reader["BookWritter"]),
                    Release = Convert.ToDateTime(reader["BookRelease"]),
                };

                books.Add(book);
            }

            ViewData["BookID"] = new SelectList(books.ToList(), "BookID", "Name");

            ViewData["UserID"] = new SelectList(users.ToList(), "UserID", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult CreateBooking(Booking booking)
        {
            var cmd = connection._con.CreateCommand() as MySqlCommand;
            try
            {
                if (ModelState.IsValid)
                {
                    cmd.CommandText = "call createBooking (@bId, @uId)";
                    cmd.Parameters.AddWithValue("@bId", booking.Book.BookID);
                    cmd.Parameters.AddWithValue("@uId", booking.User.UserID);

                    int i = cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (i >= 1)
                    {
                        ViewBag.Mensagem = "Reserva feita!";
                    }
                    else
                    {
                        ViewBag.Mensagem = "Não foi possível completar a operação!";
                    }
                }
                return RedirectToAction("selectAllBookings");
            }
            catch
            {
                throw;
            }
        }

        public IActionResult UpdateBooking(int id)
        {
            Booking booking = new Booking();
            var cmd = connection._con.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"call selectBooking(@bId)";
            cmd.Parameters.AddWithValue("@bId", id);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                booking = new Booking()
                {
                    BookingID = Convert.ToInt32(reader["BookingId"]),
                    BookID = Convert.ToInt32(reader["BookingId_BookId"]),
                    UserID = Convert.ToInt32(reader["BookingId_UserId"])
                };
            }
            connection._con.Dispose();
            if (booking.BookingID != 0 && booking.BookingID != -1)
            {
                booking.Book = assignBook(booking.BookID);
                booking.User = assignUser(booking.UserID);
            }

            connection._con.Open();
            List<User> users = new List<User>();
            List<Book> books = new List<Book>();

            cmd.CommandText = @"call selectAllUsers()";
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                User user = new User()
                {
                    UserID = Convert.ToInt32(reader["UserId"]),
                    Name = Convert.ToString(reader["UserName"]),
                    Cpf = Convert.ToString(reader["UserCpf"]),
                    Email = Convert.ToString(reader["UserEmail"])
                };

                users.Add(user);
            }
            cmd.Dispose();
            cmd.CommandText = @"call selectAllBooks()";
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Book book = new Book()
                {
                    BookID = Convert.ToInt32(reader["BookId"]),
                    Name = Convert.ToString(reader["BookName"]),
                    Writter = Convert.ToString(reader["BookWritter"]),
                    Release = Convert.ToDateTime(reader["BookRelease"]),
                };

                books.Add(book);
            }

            ViewData["BookID"] = new SelectList(books.ToList(), "BookID", "Name");

            ViewData["UserID"] = new SelectList(users.ToList(), "UserID", "Name");

            return View(booking);
        }

        [HttpPost]
        public IActionResult UpdateBooking(Booking booking) {

            var cmd = connection._con.CreateCommand() as MySqlCommand;

            try
            {
                cmd.CommandText = "call updateBooking(@bookingId, @bId, @uId)";
                cmd.Parameters.AddWithValue("@bookingId", booking.BookingID);
                cmd.Parameters.AddWithValue("@bId", booking.Book.BookID);
                cmd.Parameters.AddWithValue("@uId", booking.User.UserID);
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
                return RedirectToAction("SelectAllBookings");
            }
            catch
            {
                throw;
            }
        }

        public IActionResult DeleteBooking(int id)
        {
            var cmd = connection._con.CreateCommand() as MySqlCommand;

            try
            {
                cmd.CommandText = "call deleteBooking(@bId)";
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
                return RedirectToAction("SelectAllBookings");
            }
            catch
            {

                throw;
            }
        }
    }
}