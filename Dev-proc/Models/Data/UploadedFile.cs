using Dev_proc.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dev_proc.Models.Data
{
    public class UploadedFile
    {
        public Guid Id { get; set; }
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }

    }
}
