using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Diagnostics;
using todoproject_andrian.Models;
using todoproject_andrian.Models.ViewModel;

namespace todoproject_andrian.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly SqliteConnection _connection;

        public UserController(ILogger<UserController> logger, SqliteConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }
        public IActionResult Index()
        {
            return View("Views/User/Register.cshtml");
        }
        public RedirectResult Insert(UserModel user)
        {
            using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();

                    // Execute the SQL command to insert user data into the database
                    tableCmd.CommandText = $"INSERT INTO users (Username, Password, Name) VALUES ('{user.Username}', '{user.Password}', '{user.Name}')";

                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return Redirect("https://localhost:7048/Login");
        }
    }
}