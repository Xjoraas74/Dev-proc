using Dev_proc.Models.CompanyModels;

namespace Dev_proc.Models.ViewModels.PositionViews
{
    public class PositionAndCompanyViewModel
    {
        public string CompanyName { get; set; }
        public List<Position> positions { get; set; } = new List<Position>();
    }
}
