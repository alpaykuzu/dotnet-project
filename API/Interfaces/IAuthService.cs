using API.DTOs.Token;
using API.DTOs.User;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using YirmibesYazilim.Framework.Models.Responses;

namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<Response<NoContent>> RegisterAsync(RegisterRequest request);
        Task<Response<LoginResponse>> LoginAsync(LoginRequest request);
        Task<Response<TokenResponse>> RefreshTokenAsync(TokenRequest tokenRequest);
    }
}
