using AuthorizationService.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AuthorizationService.Data
{
    public class DataSeed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/users.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            
            if (users == null) return;

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "P4ssword$");
            }
        }
    }
}
