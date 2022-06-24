using Dev_proc.Constants.Configuration;
using Dev_proc.Data;
using Dev_proc.Models.Enums;
using Dev_proc.Models.Identity;
using Dev_proc.Models.ViewModels.CandidacyViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dev_proc.Controllers
{
    [Route("candidacy")]
    public class CandidacyController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CandidacyController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Route("{candidatureId:guid}/application")]
        [Authorize(Roles = ApplicationRoleNames.AdminAndCompany)]
        [HttpGet]
        public async Task<IActionResult> GetCandidature(Guid candidatureId)
        {
            var candidature = await _context.Candidatures
                .Include(c => c.User).ThenInclude(u => u.Resume)
                .Include(c => c.Position)
                .Where(c => c.Id == candidatureId)
                .FirstOrDefaultAsync();
            if (candidature == null)
            {
                return NotFound("Candidature not found");
            }
            return View("Candidature", new CandidacyViewModel
            {
                CandidatureId = candidature.Id,
                CandidateStatus = candidature.Status,
                Comment = candidature.Comment,
                Email = candidature.User.Email,
                Resume = candidature.User.Resume,
                PositionName = candidature.Position.Name,
                UserId = candidature.User.Id
            });
        }
        [Authorize(Roles = ApplicationRoleNames.AdminAndCompany)]
        [HttpPost]
        public async Task<IActionResult> CheckCandidature(CandidacyViewModel model)
        {
            var candidature = await _context.Candidatures
                .Where(c => c.Id == model.CandidatureId).FirstOrDefaultAsync();
            if (candidature == null)
            {
                return NotFound("Candidature not found");
            }
            candidature.Status = model.CandidateStatus;
            candidature.Comment = model.Comment;
            _context.Candidatures.Update(candidature);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Candidature updated";
            return RedirectToAction("GetCandidature", new { candidatureId = candidature.Id });
        }
        [Route("{userId:guid}/{positionId:guid}/your_candidacy")]
        [Authorize]
        public async Task<IActionResult> CheckYourCandidacy(Guid userId, Guid positionId)
        {
            var candidature = await _context.Candidatures
                .Include(c => c.User).ThenInclude(u => u.Resume)
                .Include(c => c.Position)
                .Where(c => c.PositionId == positionId && c.UserId == userId)
                .FirstOrDefaultAsync();
            if (candidature == null)
            {
                return NotFound("Candidature not found");
            }
            return View("StudentCandidature", new CandidacyViewModel
            {
                CandidatureId = candidature.Id,
                CandidateStatus = candidature.Status,
                Comment = candidature.Comment,
                Email = candidature.User.Email,
                Resume = candidature.User.Resume,
                PositionName = candidature.Position.Name,
                UserId = candidature.User.Id
            });
        }
        [Route("{candidatureId:guid}/application_delete")]
        [Authorize]
        public async Task<IActionResult> DeleteYourCandidacy(Guid candidatureId)
        {
            var candidature = await _context.Candidatures
                .Include(c => c.User).ThenInclude(u => u.Resume)
                .Include(c => c.Position)
                .Where(c => c.Id == candidatureId)
                .FirstOrDefaultAsync();
            if (candidature == null)
            {
                return NotFound("Candidature not found");
            }
            _context.Remove(candidature);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Application deleted";
            return RedirectToAction("CompaniesList", "Company");
        }
        [Route("{userId:guid}/user_candidatures")]
        [Authorize(Roles = ApplicationRoleNames.Student)]
        public async Task<IActionResult> CandidaturesForThisUser(Guid userId, CandidateStatus? candidateStatus = null)
        {
            var user = await _context.Users
                    .Include(x => x.Candidatures).ThenInclude(c => c.Position)
                    .Where(x => x.Id == userId).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found");
            }


            var candidatures = candidateStatus == null? 
                 await _context.Candidatures
                .Where(c => c.UserId == userId)
                .ToListAsync() 
                : await _context.Candidatures
                .Where(c => c.UserId == userId && c.Status == candidateStatus)
                .ToListAsync();
            StudentCandidacyView studentCandidacyView = new StudentCandidacyView
			{
                UserId = userId,
                Candidatures = candidatures
			};
            return View(studentCandidacyView);
        }
    }
}
