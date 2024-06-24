using AuthorizationService.Models.Dto;

namespace AuthorizationService.Service.IService;
public interface IAuthService
{

    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

    string GetPublicKey();
}
