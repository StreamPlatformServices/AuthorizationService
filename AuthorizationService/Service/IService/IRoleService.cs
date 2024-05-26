using AuthorizationService.Models.Dto;

namespace AuthorizationService.Service.IService
{
    public interface IRoleService
    {
        Task<string> CreateRole(string roleName);
        Task<string> DeleteRole(string roleName);
        Task<RolesResponseDto> GetAllRoles();


    }
}
