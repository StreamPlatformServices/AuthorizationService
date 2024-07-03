using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models.Dto;
public class LoginRequestDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail.")]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
