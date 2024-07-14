
namespace AuthorizationService.Models.Dto;
public class UserResponseDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }

    public long? PhoneNumber { get; set; }

    public long? NIP { get; set; }
}
