using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuthorizationService.Models.Dto;
using AuthorizationService.Service.IService;
using AuthorizationService.Entity;
using System.Security.Claims;
using System.Data;

namespace AuthorizationService.Service
{
    public class UserService: IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> UpdateEndUser(BaseUpdateUserRequestDto updateDto, ClaimsPrincipal userPrincipal)
        {
            var user = await validateUser(userPrincipal);

            if (user == null)
            {
                return "Użytkownik nie został znaleziony.";
            }

            var errorMessage = await updateBaseUserData(updateDto, user);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return errorMessage;
            }

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded ? "" : "Nie udało się zaktualizować użytkownika.";
        }

        public async Task<string> UpdateContentCreator(UpdateContentCreatorRequestDto updateDto, ClaimsPrincipal userPrincipal)
        {

            var user = await validateUser(userPrincipal);

            if (user == null)
            {
                return "Użytkownik nie został znaleziony.";
            }

            var errorMessage = await updateBaseUserData(updateDto, user);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return errorMessage;
            }

            if (updateDto.NIP != null)
            {
                user.NIP = updateDto.NIP;
            }

            if (updateDto.PhoneNumber != null)
            {
                user.PhoneNumber = updateDto.PhoneNumber;
            }

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded ? "" : "Nie udało się zaktuazliować użytkownika o roli Content Creator.";
        }

        public async Task<UsersResponseDto> GetUsers()
        {
            var users = await _userManager.Users
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    IsActive = user.IsActive,
                    Role = user.Role
                }).ToListAsync();

            return new UsersResponseDto
            {
                Users = users
            }; ;
        }

        public async Task<UserResponseDto> GetUser(ClaimsPrincipal userPrincipal)
        {
            var user = await validateUser(userPrincipal);

            if (user == null)
            {
                return null;
            }

            return new UserResponseDto
            {
                Email = user.Email,
                UserName = user.UserName,
                Role = user.Role,
                NIP = user.NIP,
                PhoneNumber = user.PhoneNumber,
            };
        }

        public  async Task<string> UpdateStatus(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return "Użytkownik nie został znaleziony.";
            }

            user.IsActive = !user.IsActive;

            await _userManager.UpdateAsync(user);

            return "";

        }


        private async Task<string> updateBaseUserData(BaseUpdateUserRequestDto updateDto, AppUser user)
        {

            if (!string.IsNullOrEmpty(updateDto.Email))
            {
                var userWithEmail = await _userManager.FindByEmailAsync(user.Email);

                if (userWithEmail != null)
                {
                    return "Nieprawidłowy e-mail.";
                }

                user.Email = updateDto.Email;
            }

            if (!string.IsNullOrEmpty(updateDto.UserName))
            {
                var userWithUserName = await _userManager.FindByEmailAsync(user.Email);

                if (userWithUserName != null)
                {
                    return "Nieprawidłowa nazwa użytkownika.";
                }

                user.UserName = updateDto.UserName;
            }

            if (!string.IsNullOrEmpty(updateDto.Password))
            {
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded)
                {
                    return "Usunięcie hasła nie powiodło się.";
                }

                var addPasswordResult = await _userManager.AddPasswordAsync(user, updateDto.Password);
                if (!addPasswordResult.Succeeded)
                {
                    return "Dodanie nowego hasła nie powiodło się.";
                }
            }
            return "";
        }

        private async Task<AppUser> validateUser(ClaimsPrincipal userPrincipal)
        {
            var userIdClaim = userPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                throw new Exception("Nieprawidłowy token.");
            }

            var userId = userIdClaim.Value;

            return await _userManager.FindByIdAsync(userId);
        }
    }
}
