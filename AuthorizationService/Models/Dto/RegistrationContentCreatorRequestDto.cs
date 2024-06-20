namespace AuthorizationService.Models.Dto;
public class RegistrationContentCreatorRequestDto : BaseRegistrationRequestDto
{
    public string PhoneNumber { get; set; }
    public string NIP { get; set; }
}
