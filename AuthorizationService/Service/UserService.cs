using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuthorizationService.Models;
using AuthorizationService.Models.Dto;
using AuthorizationService.Service.IService;
using AuthorizationService.Entity;

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

            var normalizedRoleName = roleName.ToUpper();

            var roleExists = await _roleManager.RoleExistsAsync(normalizedRoleName);
            if (!roleExists) return "Role does not exist";

            var userHasRole = await _userManager.IsInRoleAsync(user, normalizedRoleName);
            if (userHasRole) return "User already has this role";

            var result = await _userManager.AddToRoleAsync(user, normalizedRoleName);
            if (!result.Succeeded) return "Failed to add to role";

            return "";
        }

        public async Task<string> Update(string id, UpdateUserRequestDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return "Could not find user";

            if (user == null)
            {
                return "User not found"; 
            }

            if (!string.IsNullOrEmpty(updateDto.Email))
            {
                user.Email = updateDto.Email;
                user.UserName = updateDto.Email; 
            }

            if (!string.IsNullOrEmpty(updateDto.Password))
            {
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded)
                {
                    return "Password removal failed"; 
                }

                var addPasswordResult = await _userManager.AddPasswordAsync(user, updateDto.Password);
                if (!addPasswordResult.Succeeded)
                {
                    return "Adding new password failed"; 
                }
            }

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded ? "" : "Failed to update user.";
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

        public async Task<string> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return "user not found.";

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded ? "" : "Failed to delete user.";

        }

    }
}
