using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models.Dto;
public class BaseRegistrationRequestDto
{
    [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail.")]
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
