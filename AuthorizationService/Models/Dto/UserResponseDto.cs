
namespace AuthorizationService.Models.Dto;
public class UserResponseDto
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }

    public string? PhoneNumber { get; set; }

    public string? NIP { get; set; }
}
