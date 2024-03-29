﻿using Dev_proc.Constants.Configuration;
using Dev_proc.Data;
using Dev_proc.Models.Data;
using Dev_proc.Models.Identity;
using Dev_proc.Models.ViewModels;
using Dev_proc.Models.ViewModels.UserViews;
using Dev_proc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Dev_proc.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _appEnvironment;

        public UserController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context,
            IFileService fileService,
             IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _fileService = fileService;
            _appEnvironment = appEnvironment;
        }

        [Route("")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return NotFound("User doesn't exists");
            }
            User? currentUser = await _userManager.Users
                .Include(u => u.Resume)
                .Include(u=>u.PracticeDiary)
                .Include(u=>u.StudentCompanyIntern).ThenInclude(s=>s.CompanyIntern)
                .Include(u => u.Company).ThenInclude(c => c.Positions)
                .Include(u => u.Dean)
                .Where(u => u.Id == Guid.Parse(userId))
                .FirstOrDefaultAsync();

            return View("Profile", currentUser);

        }
        [Route("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Profile(Guid id)
        {
            User currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != id && !User.IsInRole(ApplicationRoleNames.Administrator))
            {
                return NotFound();
            }
            var user = await _context.Users
                .Include(u => u.Resume)
                .Include(u => u.StudentCompanyIntern).ThenInclude(s => s.CompanyIntern)
                .Include(u => u.PracticeDiary)
                .Include(u=>u.Company).ThenInclude(c=>c.Positions)
                .Include(u=>u.Dean)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            return View("Profile", user);
        }
        [Route("upload_resume/{userId:guid}")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UploadResumeForStudent(Guid userId)
        {
            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            var model = new ResumeFileViewModel { StudentId = userId };
            return View(model);
        }

        [Route("upload_resume")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadResumeForStudentPost(ResumeFileViewModel model)
        {
            try
            {
                await _fileService.UploadResume(model);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            TempData["Success"] = "Resume uploaded";
            return RedirectToAction("Profile", new { id = model.StudentId });
        }

        [Route("delete_resume/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteResumeForStudent(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Resume)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                await _fileService.DeleteResume(user.Resume);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            TempData["Success"] = "Resume deleted";
            return RedirectToAction("Profile", new { id = userId });
        }
        [Route("download_resume/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> DownloadResume(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Resume)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            if (user.Resume == null)
            {
                TempData["Error"] = "Resume not found";
                return RedirectToAction("Profile", new { id = userId });
            }

            string file_path_after_regex = user.Resume.Path.Remove(0, 2);
            //string file_path_after_regex = Regex.Replace(file_path_after_regex, "/", "\\");
            string file_path = Path.Combine(_appEnvironment.ContentRootPath, file_path_after_regex);

            return PhysicalFile(file_path, "application/pdf", user.Resume.Name);
        }

        [Route("upload_practice_diary/{userId:guid}")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UploadPracticeDiaryForStudent(Guid userId)
        {
            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            var model = new ResumeFileViewModel { StudentId = userId };
            return View(model);
        }

        [Route("upload_practice_diary")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadPracticeDiaryForStudentPost(ResumeFileViewModel model)
        {
            try
            {
                await _fileService.UploadPracticeDiary(model);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            TempData["Success"] = "Practice diary uploaded";
            return RedirectToAction("Profile", new { id = model.StudentId });
        }
        [Route("delete_practice_diary/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeletePracticeDiaryForStudent(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.PracticeDiary)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                await _fileService.DeletePracticeDiary(user.PracticeDiary);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            TempData["Success"] = "Practice diary deleted";
            return RedirectToAction("Profile", new { id = userId });
        }
        [Route("download_practice_diary/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> DownloadPracticeDiary(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.PracticeDiary)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            if (user.PracticeDiary == null)
            {
                TempData["Error"] = "Practice diary not found";
                return RedirectToAction("Profile", new { id = userId });
            }

            string file_path_after_regex = user.PracticeDiary.Path.Remove(0, 2);
            //string file_path_after_regex = Regex.Replace(file_path_after_regex, "/", "\\");
            string file_path = Path.Combine(_appEnvironment.ContentRootPath, file_path_after_regex);

            return PhysicalFile(file_path, "application/pdf", user.PracticeDiary.Name);
        }
        [Route("change_password")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangePassword(Guid userId)
        {
            return View(new ChangePasswordViewModel { });
        }
        [Route("change_password")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Something wrong =)";
                return this.View(model);
            }
            await _signInManager.RefreshSignInAsync(user);
            TempData["Success"] = "Password changed";
            return RedirectToAction("Index");
        }

    }
}
