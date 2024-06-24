using AuthorizationService.Entity;
using AuthorizationService.Models.Dto;
using AuthorizationService.Service.IService;
using Microsoft.AspNetCore.Identity;

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

            return new LoginResponseDto
            {
                Token = _jwtTokenGenerator.GenerateToken(user, user.UserRoleEnum)
            };
        }

        public string GetPublicKey()
        {
            return _jwtTokenGenerator.GetPublicKey();
        }
    }
}
