using API.Data;
using API.DTOs.Product;
using API.DTOs.Token;
using API.DTOs.UserRole;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Security.Claims;
using YirmibesYazilim.Framework.Models.Responses;

namespace API.Repositories
{
    public class UserRoleService : IUserRoleService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
 
        public UserRoleService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<Response<NoContent>> AddUserRoleAsync(UserRoleRequest userRoleRequest)
        {
            var userRole = _mapper.Map<UserRoleRequest, UserRole>(userRoleRequest);
            await _appDbContext.UserRoles.AddAsync(userRole);
            await _appDbContext.SaveChangesAsync();
            return Response<NoContent>.Success(HttpStatusCode.OK, "Rol Ekleme Başarılı!");
        }

        public async Task<Response<NoContent>> DeleteUserRoleAsync(int userId)
        {
            var userRole = await _appDbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId);
            if (userRole == null)
            {
                return Response<NoContent>.Fail("Silme Başarısız", HttpStatusCode.BadRequest);
            }
            else
            {
                _appDbContext.UserRoles.Remove(userRole);
                _appDbContext.SaveChanges();
                return Response<NoContent>.Success(HttpStatusCode.OK, "Silme Başarılı!");
            }
        }

        public async Task<Response<List<UserRoleResponse>>> GetUserRoleAllAsync()
        {
            var userRoles = await _appDbContext.UserRoles.OrderBy(p => p.Id).ToListAsync();

            if (userRoles.Count == 0)
            {
                return Response<List<UserRoleResponse>>.Fail("Ürün tablosu boş.", HttpStatusCode.BadRequest);
            }

            var response = _mapper.Map<List<UserRole>, List<UserRoleResponse>>(userRoles);
            return Response<List<UserRoleResponse>>.Success(response, HttpStatusCode.OK, "Başarılı!");
        }

        public async Task<Response<UserRoleResponse>> GetUserRoleAsync(int userId)
        {
            var userRole = await _appDbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId);
            if (userRole == null)
            {
                return Response<UserRoleResponse>.Fail("Ürün Yok.", HttpStatusCode.BadRequest);
            }
            else
            {
                var response = _mapper.Map<UserRole, UserRoleResponse>(userRole);
                return Response<UserRoleResponse>.Success(response, HttpStatusCode.OK, "Başarılı!");
            }
        }

        public async Task<Response<NoContent>> UpdateUserRoleAsync(UserRoleRequest userRoleRequest)
        {
            var userRole = await _appDbContext.UserRoles
                                    .Include(ur => ur.User)
                                        .ThenInclude(u => u.RefreshTokens)
                                    .FirstOrDefaultAsync(x => x.UserId == userRoleRequest.UserId);
            if (userRole == null)
            {
                return Response<NoContent>.Fail("Güncelleme Başarısız", HttpStatusCode.BadRequest);
            }
            else
            {
                userRole.Role = userRoleRequest.Role;

                _appDbContext.UserRoles.Update(userRole);
                await _appDbContext.SaveChangesAsync();
                return Response<NoContent>.Success(HttpStatusCode.OK, "Güncelleme Başarılı!");
            }
        }
    }
}
