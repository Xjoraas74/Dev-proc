using Dev_proc.Data;
using Dev_proc.Models.ViewModels.CandidacyViews;
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
        [HttpGet]
        public async Task<IActionResult> GetCandidature(Guid candidatureId)
        {
            var candidature = await _context.Candidatures
                .Include(c=>c.User).ThenInclude(u=>u.Resume)
                .Include(c=>c.Position)
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
        [HttpPost]
        public async Task<IActionResult> CheckCandidature(CandidacyViewModel model)
        {
            var candidature = await _context.Candidatures
                .Where(c=>c.Id == model.CandidatureId).FirstOrDefaultAsync();
            if (candidature == null)
            {
                return NotFound("Candidature not found");
            }
            candidature.Status = model.CandidateStatus;
            candidature.Comment = model.Comment;
            _context.Candidatures.Update(candidature);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Candidature updated";
            return RedirectToAction("GetCandidature",new { candidatureId = candidature.Id});
        }
    }
}
