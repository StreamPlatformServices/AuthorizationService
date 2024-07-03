using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models.Dto
{
    public class DeleteAccountRequestDto
    {
        [Required]
        public string Password { get; set; }

    }
}
