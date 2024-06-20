using AuthorizationService.Models.Dto;

namespace AuthorizationService.Service.IService
{
    public interface IAuthService
    {
        Task<string> RegisterEndUser(BaseRegistrationRequestDto registrationRequestDto);
        Task<string> RegisterContentCreator(RegistrationContentCreatorRequestDto registrationRequestDto);
        Task<string> RegisterAdminUser(BaseRegistrationRequestDto registrationRequestDto);

        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

        string GetPublicKey();
    }
}
