using AuthorizationService.Models.Dto;

namespace AuthorizationService.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        string GetPublicKey();
    }
}
