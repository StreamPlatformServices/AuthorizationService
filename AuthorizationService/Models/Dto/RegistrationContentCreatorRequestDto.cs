using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models.Dto;
public class RegistrationContentCreatorRequestDto : BaseRegistrationRequestDto
{
    [Required]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Numer NIP musi mieć 10 cyfr.")]
    public long? NIP { get; set; }

    [Required]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "Numer telefonu musi mieć 9 cyfr.")]
    public long? PhoneNumber { get; set; }
}
