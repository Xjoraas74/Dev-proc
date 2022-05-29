using Dev_proc.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Dev_proc.Models.Identity
{
    public class Role : IdentityRole<Guid>
    {
        public ICollection<UserRole> Users { get; set; }
        public RoleType Type { get; set; }
    }
}
