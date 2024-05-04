using AuthorizationService.Models.Dto;

namespace AuthorizationService.Service.IService
{
    public interface IUserService
    {
        Task<string> AssignRole(string id, string roleName);
        Task<UsersResponseDto> GetUsersWithRoles();
    }
}
