using Npgsql;

namespace LetShareAuthChallenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Grab connection string from appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add a minimal endpoint to test DB connection
            app.MapGet("/test-db", () =>
            {
                try
                {
                    using var conn = new Npgsql.NpgsqlConnection(connectionString);
                    conn.Open();
                    return Results.Ok("PostgreSQL connection is successful!");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Failed to connect to PostgreSQL: {ex.Message}");
                }
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
