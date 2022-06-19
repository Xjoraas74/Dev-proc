using Dev_proc.Constants.Configuration;
using Dev_proc.Data;
using Dev_proc.Models.CompanyModels;
using Dev_proc.Models.Identity;
using Dev_proc.Models.ViewModels.PositionViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dev_proc.Controllers
{
    [Route("position")]
    public class PositionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public PositionController(ApplicationDbContext context,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Route("delete/{positionId:guid}")]
        public async Task<IActionResult> DeletePosition(Guid positionId)
        {
            var position = await _context.Positions
                .Include(p => p.Company).ThenInclude(c => c.User)
                .Where(p => p.Id == positionId).FirstOrDefaultAsync();
            if (position == null)
            {
                return NotFound();
            }
            var profileId = position.Company.UserId;
            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Position deleted";
            return RedirectToAction("Profile", "User", new { id = profileId });
        }
        [Route("edit_position/{positionId:guid}")]
        [Authorize(Roles = ApplicationRoleNames.AdminAndCompany)]
        [HttpGet]
        public async Task<IActionResult> EditPosition(Guid positionId)
        {
            var position = await _context.Positions.Where(c => c.Id == positionId).FirstOrDefaultAsync();
            if (position == null)
            {
                return NotFound();
            }
            return View("EditPosition", new EditPositionViewModel
            {
                PositionId = positionId,
                AvailablePlaces = position.AvailablePlaces,
                Description = position.Description,
                Name = position.Name
            });
        }
        [Authorize(Roles = ApplicationRoleNames.AdminAndCompany)]
        [HttpPost]
        public async Task<IActionResult> EditPositionPost(EditPositionViewModel model)
        {
            var position = await _context.Positions
                .Include(c => c.Company)
                .Where(c => c.Id == model.PositionId).FirstOrDefaultAsync();
            if (position == null)
            {
                return NotFound();
            }
            position.Name = model.Name;
            position.Description = model.Description;
            position.AvailablePlaces = model.AvailablePlaces;
            if (model.AvailablePlaces != null && model.AvailablePlaces < 1)
            {
                TempData["Error"] = "Available places less than 1";
            }

            _context.Positions.Update(position);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Position edited";
            return RedirectToAction("Profile", "User", new { id = position.Company.UserId });
        }
        [Route("{positionId:guid}/submit_resume")]
        [Authorize(Roles = ApplicationRoleNames.Student)]
        public async Task<IActionResult> SubmitResumeForPosition(Guid positionId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return NotFound();
            }
            var currentUser = await _context.Users
                .Include(u => u.Resume)
                .Where(u => u.Id == Guid.Parse(userId))
                .FirstOrDefaultAsync();
            if (currentUser == null)
            {
                return NotFound();
            }
            if (currentUser.Resume == null)
            {
                TempData["Error"] = "You need to upload the resume in your profile";
                return RedirectToAction("CompaniesList", "Company");
            }
            var position = await _context.Positions
                .Include(p => p.Applications)
                .Where(p => p.Id == positionId).FirstOrDefaultAsync();
            if (position == null)
            {
                return NotFound();
            }
            var application = await _context.Candidatures
                .Where(a => a.UserId == currentUser.Id && a.PositionId == position.Id)
                .FirstOrDefaultAsync();

            if (application != null)
            {
                TempData["Error"] = "Resume already submitted";
                return RedirectToAction("CompaniesList", "Company");
            }
            Candidature candidature = new Candidature
            {
                Position = position,
                User = currentUser
            };
            _context.Candidatures.Add(candidature);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Resume submitted successfully";
            return RedirectToAction("CompaniesList", "Company");
        }
        [Route("{positionId:guid}/position_candidates")]
        [Authorize(Roles = ApplicationRoleNames.Company)]
        public async Task<IActionResult> ApplicationsForThisPosition(Guid positionId)
        {
            var position = await _context.Positions
                .Include(x => x.Company)
                .Include(x=> x.Applications).ThenInclude(a=>a.User)
                .Where(x => x.Id == positionId).FirstOrDefaultAsync();
            if (position == null)
            {
                return BadRequest("Position not found");
            }
            List<Position> positions = new List<Position>() { position };
            return View(new PositionAndCompanyViewModel
            {
                CompanyName = position.Company.Name,
                positions = positions
            });
        }
    }
}
