using Dev_proc.Models.CompanyModels;
using Dev_proc.Models.Data;
using Dev_proc.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dev_proc.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole,
        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override DbSet<User> Users { get; set; }
        public override DbSet<Role> Roles { get; set; }
        public override DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UploadedFile> Files { get; set; }
        public DbSet<Company> Companies { get; set; }   
        public DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasOne(p => p.User)
                .WithMany(b => b.Roles)
                .HasForeignKey(p => p.UserId);


            modelBuilder.Entity<UserRole>()
                .HasOne(p => p.Role)
                .WithMany(b => b.Users)
                .HasForeignKey(p => p.RoleId);
                

        }
    }
}