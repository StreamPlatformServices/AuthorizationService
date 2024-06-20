using Microsoft.AspNetCore.Identity;
using AuthorizationService.Models.Dto;
using AuthorizationService.Models;
using AuthorizationService.Service.IService;
using AuthorizationService.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AuthorizationService.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IJwtTokenGenerator jwtTokenGenerator,
            UserManager<AppUser> userManager)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userManager = userManager;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.NormalizedEmail == loginRequestDto.Email.ToUpper());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false || !user.IsActive)
            {
                return null;
            } 
            
            return new LoginResponseDto {
                Token = _jwtTokenGenerator.GenerateToken(user, user.UserRoleEnum)
            };
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

        public string GetPublicKey()
        {
            return _jwtTokenGenerator.GetPublicKey();
        }

        private async Task<bool> IsUserExist(string Email)
        {
            var normalizedEmail = Email.ToUpper();
            return await _userManager.Users
                .AsNoTracking()
                .AnyAsync(u => u.NormalizedEmail == normalizedEmail);
        }
    }
}
