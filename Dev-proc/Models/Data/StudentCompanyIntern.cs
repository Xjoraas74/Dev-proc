using Dev_proc.Models.CompanyModels;
using Dev_proc.Models.Identity;

namespace Dev_proc.Models.Data
{
    public class StudentCompanyIntern
    {
        public Guid Id { get; set; }
        public User Student { get; set; }
        public Guid StudentId { get; set; }
        public Company CompanyIntern { get; set; }
        public Guid CompanyId { get; set; }
    }
}
