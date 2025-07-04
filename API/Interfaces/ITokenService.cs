using API.DTOs;
using API.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace API.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(IEnumerable<Claim> claim, int minutes);
        public string GenerateRefreshToken();
    }
}
