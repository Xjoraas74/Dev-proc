using Dev_proc.Models.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Dev_proc.Models.CompanyModels;
using Dev_proc.Models.DeanModels;

namespace Dev_proc.Models.Identity
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<UserRole> Roles { get; set; }
        public ICollection<Candidature> Candidatures { get; set; }
        public string? Surname { get; set; }//Фамилия
        public string? Firstname { get; set; }//Имя
        public string? Secondname { get; set; }//Отчество
        public Guid? ResumeId { get; set; }
        public UploadedFile? Resume { get; set; }
        public Guid? PracticeDiaryId { get; set; }
        public PracticeDiary? PracticeDiary { get; set; }
        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }
        public Guid? DeanId { get; set; }
        public Dean? Dean { get; set; }
        [NotMapped]
        public string FullName => $"{Surname} {Firstname} {Secondname}";
    }
}
