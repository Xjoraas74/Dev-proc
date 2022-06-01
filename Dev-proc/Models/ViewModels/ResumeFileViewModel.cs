namespace Dev_proc.Models.ViewModels
{
    public class ResumeFileViewModel
    {
        public Guid StudentId { get; set; }
        public IFormFile Resume { get; set; }
    }
}
