namespace LetShareAuthChallenge.Models
{
    public class User
    {
        public string UserId { get; set; } = null!;         // maps to 'id'
        public string Username { get; set; } = null!;        // maps to 'username'
        public string PasswordHash { get; set; } = null!;    // maps to 'password'
        public string? Role { get; set; }                     // maps to 'role'
        public string? TenantId { get; set; }                 // maps to 'tenant_id'
        public string? LanguageId { get; set; }               // maps to 'language_id'
        public string? FirstName { get; set; }                 // maps to 'first_name'
        public string? LastName { get; set; }                   // maps to 'last_name'
    }
}
