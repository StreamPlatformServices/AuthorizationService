using AuthorizationService.Models.Dto;
using System.Security.Claims;

namespace AuthorizationService.Service.IService
{
    public interface IUserService
    {
        Task<UsersResponseDto> GetUsers();
        Task<UserResponseDto> GetUser(ClaimsPrincipal userPrincipal);

        Task<string> UpdateEndUser(BaseUpdateUserRequestDto user, ClaimsPrincipal userPrincipal);
        Task<string> UpdateContentCreator(UpdateContentCreatorRequestDto user, ClaimsPrincipal userPrincipal);
        Task<string> UpdateStatus(string username);
    }
}
