using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Dto.Requests;
public class UpdateUserStatusRequestDto
{
    [Required]
    public bool isActive { get; set; }
}
