namespace AuthorizationService.Models.Dto
{
    public class UserWithRolesDto : UserDto
    {
        public List<string> Roles { get; set; }
    }
}
