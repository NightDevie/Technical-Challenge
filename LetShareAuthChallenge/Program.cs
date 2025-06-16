using LetShareAuthChallenge.Repositories; // <- ADD THIS
using LetShareAuthChallenge.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;

namespace LetShareAuthChallenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Authentication with JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // Register UserRepository with DI
            builder.Services.AddScoped<IUserRepository>(provider =>
                new UserRepository(builder.Configuration.GetConnectionString("DefaultConnection")!));

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
                    return Results.Ok("Postgres connection is successful!");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Failed to connect to Postgres: {ex.Message}");

                }
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
