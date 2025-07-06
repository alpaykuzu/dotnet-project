using System.Text.Json.Serialization;

namespace API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<UserRole> Roles { get; set; } 
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
