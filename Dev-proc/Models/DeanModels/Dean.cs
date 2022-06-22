using Dev_proc.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dev_proc.Models.DeanModels
{
    public class Dean
    {
        public Guid Id { get; set; }
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public string Name { get; set; }
    }
}
