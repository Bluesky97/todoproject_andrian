using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using todoproject_andrian.Models;
using todoproject_andrian.Models.ViewModel;

namespace todoproject_andrian.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            var todoListViewModel = GetAllTodos();
            return View(todoListViewModel);
        }
        [HttpGet]
        public JsonResult PopulateForm(int id)
        {
            var todo = GetById(id);
            return Json(todo);
        }
        internal TodoViewModel GetAllTodos()
        {
            List<TodoItem> todoList = new();

            using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = "SELECT * FROM todo";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                TodoItem todo = new TodoItem
                                {
                                    Id = reader.GetInt32(0),
                                    Subject = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    ActivitiesNo = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                };

                                // Check if Status column is NULL
                                if (!reader.IsDBNull(4))
                                {
                                    // Parse status only if it's not NULL
                                    todo.Status = Enum.Parse<TodoStatus>(reader.GetString(4));
                                }

                                todoList.Add(todo);
                            }
                        }
                        else
                        {
                            return new TodoViewModel
                            {
                                TodoList = todoList
                            };
                        }
                    };
                }
            }

            return new TodoViewModel
            {
                TodoList = todoList
            };
        }
        internal TodoItem GetById(int id)
        {
            TodoItem todo = null;

            using (var connection = new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM todo WHERE Id = '{id}'";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            todo = new TodoItem
                            {
                                Id = reader.GetInt32(0),
                                Subject = reader.GetString(1),
                                Description = reader.GetString(2),
                                ActivitiesNo = reader.GetString(3)
                            };

                            // Check if Status column is not null
                            if (!reader.IsDBNull(4))
                            {
                                // Parse status only if it's not null
                                if (Enum.TryParse<TodoStatus>(reader.GetString(4), out TodoStatus status))
                                {
                                    todo.Status = status;
                                }
                                else
                                {
                                    // Handle if the value from the database doesn't match any enum value
                                    // You can set a default value or throw an exception
                                    // For example, setting to a default value:
                                    todo.Status = TodoStatus.Unmarked;
                                }
                            }
                        }
                        else
                        {
                            return null; // Return null if no row found with the given Id
                        }
                    }
                }
            }

            return todo;
        }
        private string GenerateActivitiesNo()
        {
            // Generate a random number and append it to "AC"
            Random rnd = new Random();
            int randomNumber = rnd.Next(1000, 10000); // Adjust range as needed
            return $"AC{randomNumber}";
        }
        public RedirectResult Insert(TodoItem todo)
        {
            using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();

                    // Generate a unique ActivitiesNo starting from AC + random number
                    string activitiesNo = GenerateActivitiesNo();
                    tableCmd.CommandText = $"INSERT INTO todo (Subject, Description, ActivitiesNo, Status) VALUES ('{todo.Subject}', '{todo.Description}', '{activitiesNo}', '{todo.Status}')";

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
            return Redirect("https://localhost:7048/Home");
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (SqliteConnection con =
                   new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = $"DELETE from todo WHERE Id = '{id}'";
                    tableCmd.ExecuteNonQuery();
                }
            }

            return Json(new { });
        }

        public RedirectResult Update(TodoItem todo)
        {
            using (SqliteConnection con =
                   new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = $"UPDATE todo SET Subject = '{todo.Subject}', Description = '{todo.Description}', Status = '{todo.Status}' WHERE Id = '{todo.Id}'";
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

            return Redirect("https://localhost:7048/Home");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
