using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuthorizationService.Models;
using AuthorizationService.Models.Dto;
using AuthorizationService.Service.IService;

namespace AuthorizationService.Service
{
    public class UserService: IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> AssignRole(string id, string roleName)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return "Could not find user";

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists) return "Role does not exist";

            var userHasRole = await _userManager.IsInRoleAsync(user, roleName);
            if (userHasRole) return "User already has this role";

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded) return "Failed to add to role";

            return "";
        }

        public async Task<UsersResponseDto> GetUsersWithRoles()
        {
            var usersWithRoles = await _userManager.Users
                .Select(user => new UserWithRolesDto
                {
                    ID = user.Id,
                    Email = user.Email,
                    Name = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Roles = user.UserRoles.Select(userRole => userRole.Role.Name).ToList()
                }).ToListAsync();

            UsersResponseDto usersResponseDto = new UsersResponseDto
            {
                Users = usersWithRoles 
            };

            return usersResponseDto; 
        }

    }
}
