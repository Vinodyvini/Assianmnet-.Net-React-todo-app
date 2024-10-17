using Microsoft.EntityFrameworkCore;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Data
    {
    // ToDoContext class is responsible for interacting with the database through Entity Framework Core.
    // It extends DbContext and manages the ToDoItem entity, representing the ToDoItems table in the database.
    public class ToDoContext : DbContext
        {
        // Constructor that accepts DbContextOptions to configure the context, such as the database connection string.
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
            {
            // Initialization code or additional configurations can go here if needed in the future.
            }

        // DbSet represents the collection of ToDoItem entities in the database.
        // This will map the ToDoItem model to the ToDoItems table in the database.
        public DbSet<ToDoItem> ToDoItems { get; set; }
        }
    }
