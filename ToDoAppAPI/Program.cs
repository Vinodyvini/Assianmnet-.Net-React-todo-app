using Microsoft.EntityFrameworkCore;
using ToDoAppAPI.Data;
using ToDoAppAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// CORS policy setup to allow requests from the React frontend at http://localhost:3000.
// This is useful for development when the API and frontend are hosted on different origins.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Add services to the container.
builder.Services.AddControllers(); // Enables support for MVC and API controllers.

// Swagger is added for API documentation and testing in development.
// Swagger allows testing API endpoints without the need for external tools like Postman.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework to use an in-memory database for storing ToDo items.
// This is useful for quick prototyping and testing, but for production, switch to a persistent database (e.g., SQL Server, PostgreSQL).
builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("ToDoDb"))); // Connection string placeholder

var app = builder.Build();

// Exception handling middleware that captures and handles exceptions globally.
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Enable CORS using the policy defined above to allow the React app to communicate with the API.
app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.

// If the application is in development mode, enable Swagger UI for easy API documentation and testing.
if (app.Environment.IsDevelopment())
    {
    app.UseSwagger(); // Serves the Swagger documentation.
    app.UseSwaggerUI(); // Provides a user interface for interacting with the Swagger docs.
    }

// Redirect all HTTP requests to HTTPS for security.
app.UseHttpsRedirection();

app.UseAuthorization(); // Enable authorization middleware. Can be expanded to handle authentication/authorization.

// Map the controllers so that API endpoints can be routed properly.
app.MapControllers();

// Run the application.
app.Run();
