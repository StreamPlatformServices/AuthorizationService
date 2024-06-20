﻿namespace AuthorizationService.Models.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }

        public int? PhoneNumber { get; set; }

        public int? NIP { get; set; }
    }
}
