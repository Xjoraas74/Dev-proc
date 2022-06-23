using Dev_proc.Constants.Configuration;
using Dev_proc.Data;
using Dev_proc.Models;
using Dev_proc.Models.CompanyModels;
using Dev_proc.Models.DeanModels;
using Dev_proc.Models.Identity;
using Dev_proc.Models.ViewModels.DeanViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;

namespace Dev_proc.Controllers
{
    [Route("dean")]
    public class DeanController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public DeanController(UserManager<User> userManager,
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
        }
        [Route("create_company")]
        [Authorize(Roles = ApplicationRoleNames.AdminAndDean)]
        [HttpGet]
        public async Task<IActionResult> CreateCompany()
        {
            return View("CreateCompany", new CreateCompanyViewModel { });
        }

        [Route("create_company")]
        [Authorize(Roles = ApplicationRoleNames.AdminAndDean)]
        [HttpPost]
        public async Task<IActionResult> CreateCompanyPost(CreateCompanyViewModel model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                EmailConfirmed = true,
            };

            Company company = new Company
            {
                Name = model.Name,
                User = user
            };
            Role role = await _context.Roles
                .Where(r => r.Name == ApplicationRoleNames.Company).FirstAsync();
            user.Company = company;
            user.Roles = new List<UserRole>() { new UserRole { Role = role, User = user } };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _context.SaveChangesAsync();
            TempData["Success"] = "Company profile created";
            return RedirectToAction("Index", "User");
        }

        [Route("create_student")]
        [Authorize(Roles = ApplicationRoleNames.AdminAndDean)]
        [HttpGet]
        public async Task<IActionResult> CreateStudent()
        {
            return View("CreateStudent", new CreateStudentViewModel { });
        }
        [Route("create_student")]
        [Authorize(Roles = ApplicationRoleNames.AdminAndDean)]
        [HttpPost]
        public async Task<IActionResult> CreateStudentPost(CreateStudentViewModel model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                Surname = model.Surname,
                Secondname = model.Secondname,
                Firstname = model.Firstname,
                EmailConfirmed = true,
            };

            Role role = await _context.Roles
                .Where(r => r.Name == ApplicationRoleNames.Student).FirstAsync();
            user.Roles = new List<UserRole>() { new UserRole { Role = role, User = user } };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _context.SaveChangesAsync();
            TempData["Success"] = "Student profile created";
            return RedirectToAction("Index", "User");
        }

        [Route("create_dean")]
        [Authorize(Roles = ApplicationRoleNames.AdminAndDean)]
        [HttpGet]
        public async Task<IActionResult> CreateDean()
        {
            return View("CreateDean", new CreateDeanViewModel { });
        }

        [Route("create_dean")]
        [Authorize(Roles = ApplicationRoleNames.AdminAndDean)]
        [HttpPost]
        public async Task<IActionResult> CreateDeanPost(CreateDeanViewModel model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                EmailConfirmed = true,
            };

            Dean dean = new Dean
            {
                Name = model.Name,
                User = user
            };
            Role role = await _context.Roles
                .Where(r => r.Name == ApplicationRoleNames.Dean).FirstAsync();
            user.Dean = dean;
            user.Roles = new List<UserRole>() { new UserRole { Role = role, User = user } };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _context.SaveChangesAsync();
            TempData["Success"] = "Dean profile created";
            return RedirectToAction("Index", "User");
        }
        [Route("student_list")]
        [Authorize(Roles = ApplicationRoleNames.AdminAndDean)]
        public async Task<IActionResult> StudentsList()
        {
            var students = await _context.Users
                .Include(u => u.Roles).ThenInclude(r => r.Role)
                .Include(u => u.PracticeDiary)
                .Where(u => u.Roles.Any(x => x.Role.Name == ApplicationRoleNames.Student))
                .ToListAsync();
            return View(students);

        }
        [Route("{userId:guid}/practice_diary")]
        [Authorize(Roles = ApplicationRoleNames.AdminAndDean)]
        [HttpGet]
        public async Task<IActionResult> UserPracticeDiary(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.PracticeDiary)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound("User not found");
            }
            if (user.PracticeDiary == null)
            {
                return NotFound("User doesn't have a practice diary");
            }
            return View("StudentPracticeDiary", new PracticeDiaryViewModel
            {
                UserId = user.Id,
                Surname = user.Surname,
                Firstname = user.Firstname,
                Secondname = user.Secondname,
                Comment = user.PracticeDiary.Comment,
                PracticeDiary = user.PracticeDiary,
                PracticeDiaryStatus = user.PracticeDiary.PracticeDiaryStatus
            });
        }
        [Route("update_practice_diary")]
        [Authorize(Roles = ApplicationRoleNames.AdminAndDean)]
        [HttpPost]
        public async Task<IActionResult> CheckUserPracticeDiary(PracticeDiaryViewModel model)
        {
            var user = await _context.Users
               .Include(u => u.PracticeDiary)
               .Where(u => u.Id == model.UserId)
               .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound("User not found");
            }
            if (user.PracticeDiary == null)
            {
                return NotFound("User doesn't have a practice diary");
            }
            user.PracticeDiary.PracticeDiaryStatus = model.PracticeDiaryStatus;
            user.PracticeDiary.Comment = model.Comment;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Practice diary updated";
            return RedirectToAction("StudentsList");
        }

        [Route("{userId:guid}/your_practice_diary")]
        [Authorize]
        public async Task<IActionResult> StudentCheckPracticeDiary(Guid userId)
        {
            var user = await _context.Users
                .Include(c => c.PracticeDiary)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound("User not found");
            }
            if (user.PracticeDiary == null)
            {
                return NotFound("User doesn't have a practice diary");
            }
            return View("StudentCheckPracticeDiary", new PracticeDiaryViewModel
            {
                UserId = user.Id,
                Surname = user.Surname,
                Firstname = user.Firstname,
                Secondname = user.Secondname,
                Comment = user.PracticeDiary.Comment,
                PracticeDiary = user.PracticeDiary,
                PracticeDiaryStatus = user.PracticeDiary.PracticeDiaryStatus
            });
        }
        [Route("create_group_of_students")]
        [Authorize(Roles = ApplicationRoleNames.AdminAndDean)]
        [HttpGet]
        public async Task<IActionResult> CreateStudentsGroup()
        {
            return View(new CreateStudentsGroupViewModel { });
        }

        [Route("create_group_of_students")]
        [Authorize(Roles = ApplicationRoleNames.AdminAndDean)]
        [HttpPost]
        public async Task<IActionResult> CreateAccountsForGroupStudents(CreateStudentsGroupViewModel model)
        {
            HttpClient client = new HttpClient();
            var lk = _configuration.GetSection("LKStudent");
            string lk_username = lk.GetSection("Loggin").Value;
            string lk_password = lk.GetSection("Password").Value;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Host", "api.lk.student.tsu.ru");
            string basicAuth = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                           .GetBytes(lk_username + ":" + lk_password));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", basicAuth);
            var uri = "https://api.lk.student.tsu.ru/" + "students/group/" + model.GroupNumber;
            var response = await client.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            var students = await response.Content.ReadFromJsonAsync<List<TSUStudent>>();
            if (students == null || students.Count == 0)
            {
                return NotFound("Students not found");
            }
            Role role = await _context.Roles
                .Where(r => r.Name == ApplicationRoleNames.Student).FirstAsync();
            var localUsers = await _context.Users.ToListAsync();

            foreach (var student in students)
			{
                var localUser = localUsers.Where(u => u.Email == student.Email).FirstOrDefault();
                if(localUser != null)
				{
                    continue;
				}
				var user = new User
				{
					Email = student.Email,
					UserName = student.Email,
					Surname = student.LastName,
					Secondname = student.Patronymic,
					Firstname = student.FirstName,
					EmailConfirmed = true,
				};
                user.Roles = new List<UserRole>() { new UserRole { Role = role, User = user } };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    continue;
                }
            }
			await _context.SaveChangesAsync();
			TempData["Success"] = "Student accounts successfully created";
            return RedirectToAction("Index", "User");
        }
    }
}
