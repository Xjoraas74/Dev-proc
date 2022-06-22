namespace Dev_proc.Models.ViewModels.DeanViews
{
    public class CreateStudentViewModel
    {
        public string Surname { get; set; }//Фамилия
        public string Firstname { get; set; }//Имя
        public string? Secondname { get; set; }//Отчество
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
