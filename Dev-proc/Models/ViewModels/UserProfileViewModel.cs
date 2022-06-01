namespace Dev_proc.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public Guid StudentId { get; set; }
        public string? Surname { get; set; }//Фамилия
        public string? Firstname { get; set; }//Имя
        public string? Secondname { get; set; }//Отчество
        public string Email { get; set; }
        public string Fullname { get; set; }
    }
}
