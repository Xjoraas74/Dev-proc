using Dev_proc.Models.CompanyModels;
using Dev_proc.Models.Identity;

namespace Dev_proc.Models.ViewModels.DeanViews
{
    public class StudentAndCandidaturesViewModel
    {
        public User Student { get; set; }
        public List<Candidature> Candidatures { get; set; }
    }
}
