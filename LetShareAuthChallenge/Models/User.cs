namespace LetShareAuthChallenge.Models
{
    public class User
    {
        public int UserId { get; set; }          // matches table column 'userid'
        public string Username { get; set; } = null!;   // non-nullable string, initialized with null-forgiving operator
        public string PasswordHash { get; set; } = null!; // same here for password hash
        public string? Role { get; set; }        // matches 'role' claim, nullable if optional
        public int? TenantId { get; set; }               // matches 'tenantId' claim
        public int? LanguageId { get; set; }             // matches 'languageId' claim
        public string? Name { get; set; }                 // matches 'name' claim
    }
}
