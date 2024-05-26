using AuthorizationService.Models;
using AuthorizationService.Models.Dto;
using AuthorizationService.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Service
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<string> CreateRole(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return $"Role '{roleName}' already exists.";
            }

            AppRole role = new AppRole {
                Id = Guid.NewGuid().ToString(),
                Name = roleName 
            };
            IdentityResult result = await _roleManager.CreateAsync(role);

            return result.Succeeded ? "" : "Failed to create a role";
        }

        public async Task<string> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return "Role not found";
            }

            var result = await _roleManager.DeleteAsync(role);

            return result.Succeeded ? "" : "Failed to delete a role"; ;
        }

        public async Task<RolesResponseDto> GetAllRoles()
        {
            var roles = await _roleManager.Roles
                .Select(role => role.NormalizedName)
                .ToListAsync();

            RolesResponseDto rolesResponseDto = new RolesResponseDto
            {
                Roles = roles
            };
            return rolesResponseDto;
        }

    }
}
