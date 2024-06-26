using System.Data;
using System.Security.Claims;
using AuthorizationService.Entity;
using AuthorizationService.Models;
using AuthorizationService.Models.Dto;
using AuthorizationService.Service.IService;
using Microsoft.AspNetCore.Authorization;
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

    public async Task<string> UpdateAdmin(BaseUpdateUserRequestDto updateDto, ClaimsPrincipal userPrincipal)
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

    public async Task<string> RemoveUser(string password, ClaimsPrincipal userPrincipal)
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

    public async Task<string> UpdateStatus(UpdateUserStatusRequestDto userStatus, string username)
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

    private async Task<string> updateBaseUserData(BaseUpdateUserRequestDto updateDto, AppUser user)
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

    public async Task<string> RegisterEndUser(BaseRegistrationRequestDto registrationRequestDto)
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

        try
        {
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

        try
        {
            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

            return result.Succeeded ? "" : result.Errors.FirstOrDefault()?.Description;

        }
        catch (Exception ex)
        {
            return $"Wystąpił błąd {ex.Message}";
        }
    }

    [Authorize(Policy = "RequireAdminRole")]
    [Authorize]
    public async Task<string> RegisterAdminUser(BaseRegistrationRequestDto registrationRequestDto)
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

        try
        {
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
        var normalizedEmail = Email.ToUpper();
        return await _userManager.Users
            .AsNoTracking()
            .AnyAsync(u => u.NormalizedEmail == normalizedEmail);
    }
}
