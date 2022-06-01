using Dev_proc.Models.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dev_proc.Models.Identity
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<UserRole> Roles { get; set; }
        public string? Surname { get; set; }//Фамилия
        public string? Firstname { get; set; }//Имя
        public string? Secondname { get; set; }//Отчество
        public UploadedFile? Resume { get; set; }
        public Guid? ResumeId { get; set; }
        [NotMapped]
        public string FullName => $"{Surname} {Firstname} {Secondname}";
    }
}
