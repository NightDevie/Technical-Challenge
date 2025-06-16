using LetShareAuthChallenge.Models;
using Npgsql;
using System.Data;

namespace LetShareAuthChallenge.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            User? user = null;

            await using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                await using (var cmd = new NpgsqlCommand("SELECT id, username, password, role, tenant_id, language_id, first_name, last_name FROM tbl_user WHERE username = @username LIMIT 1", conn))
                {
                    cmd.Parameters.AddWithValue("username", username);

                    await using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                UserId = reader.GetString(0),
                                Username = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                Role = !reader.IsDBNull(3) ? reader.GetString(3) : null,
                                TenantId = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                                LanguageId = !reader.IsDBNull(5) ? reader.GetString(5) : null,
                                FirstName = !reader.IsDBNull(6) ? reader.GetString(6) : null,
                                LastName = !reader.IsDBNull(7) ? reader.GetString(7) : null,
                            };
                        }
                    }
                }
            }

            return user;
        }
    }
}
