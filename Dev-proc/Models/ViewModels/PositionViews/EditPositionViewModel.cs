namespace Dev_proc.Models.ViewModels.PositionViews
{
    public class EditPositionViewModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? AvailablePlaces { get; set; }
        public Guid PositionId { get; set; }
    }
}
