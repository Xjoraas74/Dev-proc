using Dev_proc.Models.Enums;
using Dev_proc.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dev_proc.Models.Data
{
    public class PracticeDiary
    {
        public Guid Id { get; set; }
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
        public PracticeDiaryStatus PracticeDiaryStatus { get; set; } = PracticeDiaryStatus.New;
    }
}
