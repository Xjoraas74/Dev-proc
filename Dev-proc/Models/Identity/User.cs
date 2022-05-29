using Microsoft.AspNetCore.Identity;

namespace Dev_proc.Models.Identity
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<UserRole> Roles { get; set; }
    }
}
