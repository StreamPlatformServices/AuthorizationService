
namespace AuthorizationService.Models.Dto
{
    public class UserResponseDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

        public int? PhoneNumber { get; set; }

        public int? NIP { get; set; }
    }
}
