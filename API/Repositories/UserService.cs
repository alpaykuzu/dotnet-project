using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using YirmibesYazilim.Framework.Models.Responses;

namespace API.Repositories
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public UserService(AppDbContext appDbContext, ITokenService tokenService, IMapper mapper) 
        {
            _appDbContext = appDbContext;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        public async Task<Response<NoContent>> RegisterAsync(RegisterRequest request)
        {
            if (await _appDbContext.Users.AnyAsync(u => u.Email == request.Email))
                return Response<NoContent>.Fail("Zaten kayıtlı hesap!", HttpStatusCode.BadRequest); ;

            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "User"
            };
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            return Response<NoContent>.Success(HttpStatusCode.OK, "Kayıt başarılı");
        }

        public async Task<Response<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Response<LoginResponse>.Fail("Giriş başarısız", HttpStatusCode.Unauthorized); ;

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, request.Email),
            };
            var token = _tokenService.GenerateToken(claims);

            var generatedRefreshToken = _tokenService.GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                Token = generatedRefreshToken,
                UserId = user.Id,
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            var existingToken = await _appDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (existingToken != null)
            {
                existingToken.Token = refreshToken.Token;
                existingToken.ExpirationDate = refreshToken.ExpirationDate;
                _appDbContext.RefreshTokens.Update(existingToken);
            }
            else
            {
                await _appDbContext.RefreshTokens.AddAsync(refreshToken);
            }

            await _appDbContext.SaveChangesAsync();

            var loginResponse = _mapper.Map<LoginResponse>(user, opt =>
            {
                opt.Items["Token"] = token;
                opt.Items["RefreshToken"] = generatedRefreshToken;
            });

            return Response<LoginResponse>.Success(loginResponse, HttpStatusCode.OK, "Giriş başarılı");
        }

        public async Task<Response<TokenResponse>> RefreshTokenAsync(TokenRequest tokenRequest)
        {
            var existingToken = await _appDbContext.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);
            if (existingToken == null || existingToken?.ExpirationDate < DateTime.UtcNow)
            {
                return Response<TokenResponse>.Fail("Geçersiz veya tarihi geçmiş token!", HttpStatusCode.Unauthorized);
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, existingToken.User.Email)
            };
            var newGEneratedToken = _tokenService.GenerateToken(claims);
            var newGeneratedRefreshToken = _tokenService.GenerateRefreshToken();

            var newRefreshToken = new RefreshToken
            {
                Token = newGeneratedRefreshToken,
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            existingToken.Token = newRefreshToken.Token;
            existingToken.ExpirationDate = newRefreshToken.ExpirationDate;
            _appDbContext.RefreshTokens.Update(existingToken);
            _appDbContext.SaveChanges();
            var tokenResponse = new TokenResponse { AccessToken = newGEneratedToken, RefreshToken = newGeneratedRefreshToken };

            return Response<TokenResponse>.Success(tokenResponse,HttpStatusCode.OK, "Token güncellendi");
        }
    }
}
public class NoContent { }