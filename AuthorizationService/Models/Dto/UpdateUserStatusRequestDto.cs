using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models.Dto;
public class UpdateUserStatusRequestDto
{
    [Required]
    public bool isActive { get; set; }
}
