using API.Data;
using API.DTOs.Token;
using API.DTOs.User;
using API.DTOs.UserRole;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using YirmibesYazilim.Framework.Models.Responses;

namespace API.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUserRoleService _roleService;
        private int AccessTokenMinutes = 1;
        private int RefreshTokenDays = 1;
        public AuthService(AppDbContext appDbContext, ITokenService tokenService, IMapper mapper, IUserRoleService userRoleService) 
        {
            _appDbContext = appDbContext;
            _tokenService = tokenService;
            _mapper = mapper;
            _roleService = userRoleService;
        }
        public async Task<Response<NoContent>> RegisterAsync(RegisterRequest request)
        {
            if (await _appDbContext.Users.AnyAsync(u => u.Email == request.Email))
                return Response<NoContent>.Fail("Zaten kayıtlı hesap!", HttpStatusCode.BadRequest); 

            if (request.Email == "admin@admin.com")
            {
                var admin = _mapper.Map<RegisterRequest, User>(request, opt =>
                {
                    opt.Items["PasswordHash"] = BCrypt.Net.BCrypt.HashPassword(request.Password);
                });

                await _appDbContext.Users.AddAsync(admin);
                await _appDbContext.SaveChangesAsync();

                await _roleService.AddUserRoleAsync(new UserRoleRequest { UserId = admin.Id, Role = "Admin" });
                return Response<NoContent>.Success(HttpStatusCode.OK, "Kayıt başarılı");
            }

            var user = _mapper.Map<RegisterRequest, User>(request, opt =>
            {
                opt.Items["PasswordHash"] = BCrypt.Net.BCrypt.HashPassword(request.Password);
            });

            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            await _roleService.AddUserRoleAsync(new UserRoleRequest { UserId = user.Id, Role = "User" });
            return Response<NoContent>.Success(HttpStatusCode.OK, "Kayıt başarılı");
        }

        public async Task<Response<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Response<LoginResponse>.Fail("Giriş başarısız", HttpStatusCode.Unauthorized); ;

            var role = await _roleService.GetUserRoleAsync(user.Id);
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, role.Data.Role),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = _tokenService.GenerateToken(claims, AccessTokenMinutes);

            var generatedRefreshToken = _tokenService.GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                Token = generatedRefreshToken,
                UserId = user.Id,
                ExpirationDate = DateTime.Now.AddDays(RefreshTokenDays)
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
                opt.Items["AccessTokenExpTime"] = DateTime.Now.AddMinutes(AccessTokenMinutes).ToString("o");
                opt.Items["RefreshTokenExpTime"] = DateTime.Now.AddDays(RefreshTokenDays).ToString("o");
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
            var role = await _roleService.GetUserRoleAsync(existingToken.User.Id);
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, role.Data.Role),
                new Claim(ClaimTypes.Email, existingToken.User.Email)
            };
            var newGeneratedToken = _tokenService.GenerateToken(claims, AccessTokenMinutes);
            var newGeneratedRefreshToken = _tokenService.GenerateRefreshToken();

            var newRefreshToken = new RefreshToken
            {
                Token = newGeneratedRefreshToken,
                ExpirationDate = DateTime.UtcNow.AddDays(RefreshTokenDays)
            };

            existingToken.Token = newRefreshToken.Token;
            existingToken.ExpirationDate = newRefreshToken.ExpirationDate;
            _appDbContext.RefreshTokens.Update(existingToken);
            _appDbContext.SaveChanges();
            var tokenResponse = new TokenResponse { AccessToken = newGeneratedToken, RefreshToken = newGeneratedRefreshToken , AccessTokenExpTime = DateTime.Now.AddMinutes(AccessTokenMinutes).ToString("o"), RefreshTokenExpTime = DateTime.Now.AddDays(RefreshTokenDays).ToString("o") };

            return Response<TokenResponse>.Success(tokenResponse,HttpStatusCode.OK, "Token güncellendi");
        }
    }
}
public class NoContent { }