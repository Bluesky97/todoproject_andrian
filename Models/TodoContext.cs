using Microsoft.EntityFrameworkCore;

namespace todoproject_andrian.Models
{
    public class TodoContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ToDoItemModel> ToDoItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=C:\Temp\TodoDB.db");
        }
    }
}
