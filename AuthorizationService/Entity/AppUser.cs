using AuthorizationService.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizationService.Entity;
    public class AppUser : IdentityUser
    {

        public bool IsActive { get; set; } = false;
        public string Role { get; set; }
        public int? PhoneNumber { get; set; }
        public int? NIP { get; set; }


        [NotMapped]
        public UserRole UserRoleEnum
        {
            get
        {
            if (Enum.TryParse<UserRole>(Role, out UserRole roleValue))
            {
                return roleValue;
            }
            // Można zdefiniować domyślną wartość lub logikę błędu
            return UserRole.EndUser; // Przykładowo, domyślny typ roli
        }
        set => Role = value.ToString();
        }
    }
