using System.Data;
using System.Security.Claims;
using AuthorizationService.Models;
using AuthorizationService.Models.Dto;
using AuthorizationService.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Service;
public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> UpdateEndUser(BaseUpdateUserRequestDto updateDto, ClaimsPrincipal userPrincipal)
    {
        try
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
        catch (Exception ex)
        {
            throw;
        }

    }

    public async Task<string> UpdateContentCreator(UpdateContentCreatorRequestDto updateDto, ClaimsPrincipal userPrincipal)
    {

        try
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
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<string> UpdateAdmin(BaseUpdateUserRequestDto updateDto, ClaimsPrincipal userPrincipal)
    {
        try
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
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<UsersResponseDto> GetUsers()
    {
        try
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
            };
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<UserResponseDto> GetUser(ClaimsPrincipal userPrincipal)
    {
        try
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
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<string> RemoveUser(string password, ClaimsPrincipal userPrincipal)
    {
        try
        {
            var user = await validateUser(userPrincipal);

            if (user == null)
            {
                return null;
            }

            var isCorrectPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!isCorrectPassword)
            {
                return "Nieprawidłowe hasło.";
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return "";
            }

            return "Nie udało się usunąć użytkownika.";
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<string> UpdateStatus(UpdateUserStatusRequestDto userStatus, string username)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return "Użytkownik nie został znaleziony.";
            }

            user.IsActive = userStatus.isActive;

            await _userManager.UpdateAsync(user);

            return "";
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    private async Task<string> updateBaseUserData(BaseUpdateUserRequestDto updateDto, AppUser user)
    {
        try
        {
            if (!string.IsNullOrEmpty(updateDto.Email))
            {
                var userWithEmail = await _userManager.FindByEmailAsync(updateDto.Email);

                if (userWithEmail != null && userWithEmail.Id != user.Id)
                {
                    return "Nieprawidłowy e-mail.";
                }

                user.Email = updateDto.Email;
            }

            if (!string.IsNullOrEmpty(updateDto.UserName))
            {
                var userWithUserName = await _userManager.FindByNameAsync(updateDto.UserName);

                if (userWithUserName != null && userWithUserName.Id != user.Id)
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
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<AppUser> validateUser(ClaimsPrincipal userPrincipal)
    {
        try
        {
            var userIdClaim = userPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                throw new Exception("Nieprawidłowy token.");
            }

            var userId = userIdClaim.Value;

            return await _userManager.FindByIdAsync(userId);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<string> RegisterEndUser(BaseRegistrationRequestDto registrationRequestDto)
    {
        try
        {
            if (await IsUserExist(registrationRequestDto.Email))
            {
                return "Użytkownik już istnieje.";
            }

            AppUser user = new()
            {
                UserName = registrationRequestDto.UserName,
                Email = registrationRequestDto.Email,
                UserRoleEnum = UserRole.EndUser,
                IsActive = true,
            };

            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

            return result.Succeeded ? "" : result.Errors.FirstOrDefault()?.Description;

        }
        catch (Exception ex)
        {
            return $"Wystąpił błąd: {ex.Message}";
        }
    }

    public async Task<string> RegisterContentCreator(RegistrationContentCreatorRequestDto registrationRequestDto)
    {
        try
        {
            if (await IsUserExist(registrationRequestDto.Email))
            {
                return "Użytkownik już istnieje.";
            }

            AppUser user = new()
            {
                UserName = registrationRequestDto.UserName,
                Email = registrationRequestDto.Email,
                NIP = registrationRequestDto.NIP,
                PhoneNumber = registrationRequestDto.PhoneNumber,
                UserRoleEnum = UserRole.ContentCreator,
                IsActive = false,
            };
            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

            return result.Succeeded ? "" : result.Errors.FirstOrDefault()?.Description;

        }
        catch (Exception ex)
        {
            return $"Wystąpił błąd {ex.Message}";
        }
    }

    public async Task<string> ChangePassword(ChangePasswordRequestDto changePasswordRequest, ClaimsPrincipal userPrincipal)
    {
        try
        {
            var user = await validateUser(userPrincipal);

            if (user == null)
            {
                return "Użytkownik nie został znaleziony.";
            }

            if (string.IsNullOrWhiteSpace(changePasswordRequest.OldPassword) || string.IsNullOrWhiteSpace(changePasswordRequest.NewPassword))
            {
                return "Wszystkie pola są wymagane.";
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);

            if (result.Succeeded)
            {
                return "";
            }

            return "Nie udało się zaktualizować hasła.";

        }

        catch (Exception ex)
        {
            // TODO: Read about throwing erros - should we throw ex.Message? Maybe we shouldn't be so verbose.
            return $"Wystąpił błąd: {ex.Message}";
        }
    }

    public async Task<string> RegisterAdminUser(BaseRegistrationRequestDto registrationRequestDto)
    {
        try
        {
            if (await IsUserExist(registrationRequestDto.Email))
            {
                return "Użytkownik już istnieje.";
            }

            AppUser user = new()
            {
                UserName = registrationRequestDto.UserName,
                Email = registrationRequestDto.Email,
                UserRoleEnum = UserRole.Admin,
                IsActive = true,
            };

            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

            return result.Succeeded ? "" : result.Errors.FirstOrDefault()?.Description;

        }
        catch (Exception ex)
        {
            return $"Wystąpił błąd: {ex.Message}";
        }
    }

    private async Task<bool> IsUserExist(string Email)
    {
        try
        {
            var normalizedEmail = Email.ToUpper();
            return await _userManager.Users
                .AsNoTracking()
                .AnyAsync(u => u.NormalizedEmail == normalizedEmail);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
