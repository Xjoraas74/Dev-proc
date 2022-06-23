using Dev_proc.Models.CompanyModels;

namespace Dev_proc.Models.ViewModels.DeanViews
{
	public class AddStudentToCompanyInternViewModel
	{
		public Guid UserId { get; set; }
		public List<Company> Companies { get; set; }
		public Guid? CompanyId { get; set; }
		public string? Surname { get; set; }
		public string? Firstname { get; set; }
		public string? Secondname { get; set; }
	}
}
