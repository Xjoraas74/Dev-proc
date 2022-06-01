using Dev_proc.Constants.Configuration;
using Dev_proc.Data;
using Dev_proc.Models.Identity;
using Dev_proc.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dev_proc.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        public UserController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context= context;
        }

        [Route("")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return NotFound();
            }
            User currentUser = await _userManager.GetUserAsync(User);
            return View("Profile", new UserProfileViewModel
            {
                StudentId = currentUser.Id,
                Firstname = currentUser.Firstname,
                Secondname = currentUser.Secondname,
                Surname = currentUser.Surname,
                Fullname = currentUser.FullName,
                Email = currentUser.Email
            });

        }
        [Route("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Profile(Guid id)
        {
            User currentUser = await _userManager.GetUserAsync(User);
            if (!User.IsInRole(ApplicationRoleNames.AdminAndDean) && currentUser.Id != id)
            {
                return NotFound();
            }
            var user = await _context.Users.Where(x=>x.Id==id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            return View(new UserProfileViewModel
            {
                StudentId=user.Id,
                Firstname = user.Firstname,
                Secondname = user.Secondname,
                Surname = user.Surname,
                Fullname = user.FullName,
                Email = user.Email
            });
        }
        [Route("upload_resume/{studentId:guid}")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UploadResumeForStudent(Guid studentId)
        {
            var model = new ResumeFileViewModel {StudentId = studentId};
            return View(model);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadResumeForStudentPost(Guid studentId, ResumeFileViewModel model)
        {
            return View(model);
        }
    }
}
