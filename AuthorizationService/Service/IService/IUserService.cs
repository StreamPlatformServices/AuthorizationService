using System.Security.Claims;
using AuthorizationService.Models.Dto;

namespace AuthorizationService.Service.IService;
public interface IUserService
{
    Task<UsersResponseDto> GetUsers();
    Task<UserResponseDto> GetUser(ClaimsPrincipal userPrincipal);
    Task<string> RemoveUser(string password, ClaimsPrincipal userPrincipal);
    Task<string> RegisterEndUser(BaseRegistrationRequestDto registrationRequestDto);
    Task<string> RegisterContentCreator(RegistrationContentCreatorRequestDto registrationRequestDto);
    Task<string> RegisterAdminUser(BaseRegistrationRequestDto registrationRequestDto);
    Task<string> UpdateEndUser(BaseUpdateUserRequestDto user, ClaimsPrincipal userPrincipal);
    Task<string> UpdateContentCreator(UpdateContentCreatorRequestDto user, ClaimsPrincipal userPrincipal);
    Task<string> UpdateAdmin(BaseUpdateUserRequestDto user, ClaimsPrincipal userPrincipal);
    Task<string> UpdateStatus(UpdateUserStatusRequestDto isActive, string username);
}
