using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ToDoAppAPI.Data;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
        {
        // Dependency Injection for DbContext to interact with the database
        private readonly ToDoContext _context;

        public ToDoController(ToDoContext context)
            {
            _context = context;
            }

        // GET: api/todo
        // Retrieves all ToDo items from the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetToDoItems()
            {
            try
                {
                // Fetch all ToDo items asynchronously
                return await _context.ToDoItems.ToListAsync();
                }
            catch (Exception ex)
                {
                // Optionally log the exception for future troubleshooting
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching the ToDo items." });
                }
            }

        // GET: api/todo/{id}
        // Retrieves a specific ToDo item by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetToDoItem(int id)
            {
            try
                {
                // Find the ToDo item by its ID asynchronously
                var todoItem = await _context.ToDoItems.FindAsync(id);

                // Return 404 if item not found
                if (todoItem == null)
                    {
                    return NotFound(new { Message = "ToDo item not found." });
                    }

                return todoItem;
                }
            catch (Exception ex)
                {
                // Optionally log the exception for future troubleshooting
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching the ToDo item." });
                }
            }

        // POST: api/todo
        // Creates a new ToDo item in the database
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> PostToDoItem(ToDoItem todoItem)
            {
            try
                {
                // Add the new ToDo item to the database context
                _context.ToDoItems.Add(todoItem);

                // Save changes asynchronously
                await _context.SaveChangesAsync();

                // Return the created item and a 201 status code
                return CreatedAtAction(nameof(GetToDoItem), new { id = todoItem.Id }, todoItem);
                }
            catch (Exception ex)
                {
                // Optionally log the exception for future troubleshooting
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = "An error occurred while creating the ToDo item." });
                }
            }

        // PUT: api/todo/{id}
        // Updates an existing ToDo item by its ID
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoItem(int id, ToDoItem todoItem)
            {
            // Validate that the provided ID matches the ToDo item's ID
            if (id != todoItem.Id)
                {
                return BadRequest(new { Message = "The ID in the URL does not match the item ID." });
                }

            // Mark the item as modified in the database context
            _context.Entry(todoItem).State = EntityState.Modified;

            try
                {
                // Attempt to save the changes asynchronously
                await _context.SaveChangesAsync();
                }
            catch (DbUpdateConcurrencyException)
                {
                // Check if the item exists before trying to update
                if (!_context.ToDoItems.Any(e => e.Id == id))
                    {
                    return NotFound(new { Message = "ToDo item not found." });
                    }

                // If it's a concurrency issue, re-throw the exception
                throw;
                }
            catch (Exception ex)
                {
                // Optionally log the exception for future troubleshooting
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = "An error occurred while updating the ToDo item." });
                }

            // Return 204 No Content on successful update
            return NoContent();
            }

        // DELETE: api/todo/{id}
        // Deletes an existing ToDo item by its ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(int id)
            {
            try
                {
                // Find the ToDo item by its ID
                var todoItem = await _context.ToDoItems.FindAsync(id);

                // If the item does not exist, return 404
                if (todoItem == null)
                    {
                    return NotFound(new { Message = "ToDo item not found." });
                    }

                // Remove the item from the database context
                _context.ToDoItems.Remove(todoItem);

                // Save changes asynchronously
                await _context.SaveChangesAsync();

                // Return 204 No Content on successful deletion
                return NoContent();
                }
            catch (Exception ex)
                {
                // Optionally log the exception for future troubleshooting
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = "An error occurred while deleting the ToDo item." });
                }
            }

        // GET: api/todo/search/{query}
        // Searches for ToDo items based on a query (matches Title, Description, Priority, or Category)
        [HttpGet("search/{query}")]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> SearchToDoItems(string query)
            {
            try
                {
                // Filter ToDo items based on whether the query matches any of the given fields
                return await _context.ToDoItems
                    .Where(t => t.Title.Contains(query) || t.Description.Contains(query) || t.Priority.Contains(query) || t.Category.Contains(query))
                    .ToListAsync();
                }
            catch (Exception ex)
                {
                // Optionally log the exception for future troubleshooting
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = "An error occurred while searching for ToDo items." });
                }
            }
        }
    }
