using AuthorizationService.Models;
using AuthorizationService.Models.Dto;

namespace AuthorizationService.Service.IService
{
    public interface IUserService
    {
        Task<string> AssignRole(string id, string roleName);
        Task<UsersResponseDto> GetUsersWithRoles();

        Task<string> Update(string id, UpdateUserRequestDto user);
        Task<string> Delete(string id);
    }
}
