using Microsoft.AspNetCore.Identity;

namespace TamweelyHr.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
