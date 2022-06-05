namespace Dev_proc.Models.ViewModels.CompanyViews
{
    public class AddPositionViewModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? AvailablePlaces { get; set; }
        public Guid CompanyId { get; set; }
    }
}
