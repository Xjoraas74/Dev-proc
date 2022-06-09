namespace Dev_proc.Models.CompanyModels
{
    public class Position
    {
        public Guid Id { get; set; }
        public string Name { get; set; }  
        public string? Description { get; set; }
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }
        public int? AvailablePlaces { get; set; }
     
        public ICollection<Candidature> Applications { get; set; }
    }
}
