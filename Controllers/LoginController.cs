using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using todoproject_andrian.Models;

namespace todoproject_andrian.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly SqliteConnection _connection;

        public LoginController(ILogger<LoginController> logger, SqliteConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }
        public IActionResult Index()
        {
            return View("Views/User/Login.cshtml");
        }
        [HttpPost]
        public IActionResult Login(UserModel model)
        {
            using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
            {
                con.Open();

                string query = $"SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";

                using (var command = new SqliteCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Username", model.Username);
                    command.Parameters.AddWithValue("@Password", model.Password);

                    long userCount = (long)command.ExecuteScalar();

                    if (userCount > 0)
                    {
                        // Authentication successful, redirect to home page
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect("https://localhost:7048");
                    }
                }
            }
        }
        [HttpPost]
        public IActionResult Register()
        {
            return View("Views/User/Register.cshtml");
        }
    }
}
