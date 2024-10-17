namespace ToDoAppAPI.Models
    {
    // The ToDoItem class represents an individual task or item in the to-do list.
    public class ToDoItem
        {
        // Unique identifier for each ToDo item (Primary Key).
        public int Id { get; set; }

        // Title of the to-do item (required).
        // Example: "Finish project documentation".
        public required string Title { get; set; }

        // Detailed description of the to-do item (required).
        // Example: "Write the final version of the project documentation and submit it to the client."
        public required string Description { get; set; }

        // Category for organizing the task (required).
        // Example: "Work" or "Personal".
        public required string Category { get; set; }

        // Priority level of the to-do item (required).
        // Example: "High", "Medium", or "Low".
        public required string Priority { get; set; }
        }
    }
