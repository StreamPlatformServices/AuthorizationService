namespace AuthorizationService.Models.Dto
{
    public class BaseUpdateUserRequestDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }

    }
}
