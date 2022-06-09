using Dev_proc.Models.Enums;
using Dev_proc.Models.Identity;

namespace Dev_proc.Models.CompanyModels
{
    public class Candidature
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public Position Position { get; set; }
        public Guid PositionId { get; set; }
        public CandidateStatus Status { get; set; } = CandidateStatus.New;
    }
}
