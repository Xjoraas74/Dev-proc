using Dev_proc.Constants.Configuration;
using Dev_proc.Data;
using Dev_proc.Models.CompanyModels;
using Dev_proc.Models.DeanModels;
using Dev_proc.Models.Identity;
using Dev_proc.Models.ViewModels.DeanViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dev_proc.Controllers
{
    [Route("dean")]
    public class DeanController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        public DeanController(UserManager<User> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
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
            return RedirectToAction("Index","User");
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
    }
}
