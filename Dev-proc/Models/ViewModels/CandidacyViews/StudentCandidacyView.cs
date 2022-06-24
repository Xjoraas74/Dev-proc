using Dev_proc.Models.CompanyModels;

namespace Dev_proc.Models.ViewModels.CandidacyViews
{
    public class StudentCandidacyView
    {
        public Guid UserId { get; set; }
        public List<Candidature> Candidatures { get; set; }
    }
}
