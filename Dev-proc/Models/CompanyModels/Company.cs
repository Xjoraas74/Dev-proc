using Dev_proc.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dev_proc.Models.CompanyModels
{
    public class Company
    {
        public Guid Id { get; set; }    
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public string? Comment { get; set; }
        public string Name { get; set; }
        public ICollection<Position> Positions { get; set; } = new List<Position>();
    }
}
