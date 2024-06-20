namespace AuthorizationService.Models.Dto
{
    public class RegistrationContentCreatorRequestDto: BaseRegistrationRequestDto
    {
        public int PhoneNumber { get; set; }
        public int NIP { get; set; }
    }
}
