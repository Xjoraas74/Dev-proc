using Dev_proc.Models.Data;
using Dev_proc.Models.Enums;

namespace Dev_proc.Models.ViewModels.CandidacyViews
{
    public class CandidacyViewModel
    {
        public string Email { get; set; }
        public CandidateStatus CandidateStatus { get; set; }
        public Guid CandidatureId { get; set; }
        public Guid UserId { get; set; }
        public string? Comment { get; set; }
        public string PositionName { get; set; }
        public UploadedFile? Resume { get; set; }
        public DateTime? InterviewDate { get; set; }
    }
}
