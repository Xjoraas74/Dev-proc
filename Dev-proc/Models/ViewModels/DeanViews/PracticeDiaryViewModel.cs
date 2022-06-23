using Dev_proc.Models.Data;
using Dev_proc.Models.Enums;
using Dev_proc.Models.Identity;

namespace Dev_proc.Models.ViewModels.DeanViews
{
    public class PracticeDiaryViewModel
    {
        public Guid UserId { get; set; }
        public string? Surname { get; set; }
        public string? Firstname { get; set; }
        public string? Secondname { get; set; }
        public PracticeDiaryStatus PracticeDiaryStatus { get; set; }
        public string? Comment { get; set; }
        public PracticeDiary? PracticeDiary { get; set; }
    }
}
