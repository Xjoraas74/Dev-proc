using Dev_proc.Models.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Dev_proc.Models.CompanyModels;

namespace Dev_proc.Models.Identity
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<UserRole> Roles { get; set; }
        public string? Surname { get; set; }//Фамилия
        public string? Firstname { get; set; }//Имя
        public string? Secondname { get; set; }//Отчество
        public Guid? ResumeId { get; set; }
        public UploadedFile? Resume { get; set; }
        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }
        [NotMapped]
        public string FullName => $"{Surname} {Firstname} {Secondname}";
    }
}
