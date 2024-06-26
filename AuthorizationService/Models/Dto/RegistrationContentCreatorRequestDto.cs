using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models.Dto;
public class RegistrationContentCreatorRequestDto : BaseRegistrationRequestDto
{
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Numer NIP musi mieć 10 cyfr.")]
    public long? NIP { get; set; }

    [RegularExpression(@"^\d{9}$", ErrorMessage = "Numer telefonu musi mieć 9 cyfr.")]
    public long? PhoneNumber { get; set; }
}
