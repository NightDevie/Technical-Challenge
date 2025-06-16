using Npgsql;
using LetShareAuthChallenge.Models;

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

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (var cmd = new NpgsqlCommand("SELECT userid, username, password_hash, role, tenantid, languageid, name FROM tbl_user WHERE username = @username LIMIT 1", conn))
                {
                    cmd.Parameters.AddWithValue("username", username);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                Role = !reader.IsDBNull(3) ? reader.GetString(3) : null,
                                TenantId = !reader.IsDBNull(4) ? reader.GetInt32(4) : (int?)null,
                                LanguageId = !reader.IsDBNull(5) ? reader.GetInt32(5) : (int?)null,
                                Name = !reader.IsDBNull(6) ? reader.GetString(6) : null,
                            };
                        }
                    }
                }
            }

            return user;
        }
    }
}
