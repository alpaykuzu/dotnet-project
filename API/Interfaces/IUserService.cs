using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using YirmibesYazilim.Framework.Models.Responses;

namespace API.Interfaces
{
    public interface IUserService
    {
        Task<Response<NoContent>> RegisterAsync(RegisterRequest request);
        Task<Response<LoginResponse>> LoginAsync(LoginRequest request);
        Task<Response<TokenResponse>> RefreshTokenAsync(TokenRequest tokenRequest);

    }
}
