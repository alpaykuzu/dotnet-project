using API.DTOs.UserRole;
using YirmibesYazilim.Framework.Models.Responses;

namespace API.Interfaces
{
    public interface IUserRoleService
    {
        Task<Response<UserRoleResponse>> GetUserRoleAsync(int userId);
        Task<Response<List<UserRoleResponse>>> GetUserRoleAllAsync();
        Task<Response<NoContent>> AddUserRoleAsync(UserRoleRequest userRoleRequest);
        Task<Response<NoContent>> UpdateUserRoleAsync(UserRoleRequest userRoleRequest);
        Task<Response<NoContent>> DeleteUserRoleAsync(int userId);
    }
}
