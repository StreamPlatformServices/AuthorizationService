using AuthorizationService.Dto.Requests;
using AuthorizationService.Dto.Responses;

namespace AuthorizationService.Service.IService;
public interface IAuthService
{

    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

    string GetPublicKey();
}
