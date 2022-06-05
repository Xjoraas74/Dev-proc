using Dev_proc.Constants.Configuration;
using Dev_proc.Data;
using Dev_proc.Models.ViewModels.PositionViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dev_proc.Controllers
{
	[Route("position")]
	public class PositionController : Controller
	{
		private readonly ApplicationDbContext _context;
		public PositionController(ApplicationDbContext context)
		{
			_context = context;
		}
		[Route("delete/{positionId:guid}")]
		public async Task<IActionResult> DeletePosition(Guid positionId)
		{
			var position = await _context.Positions
				.Include(p=>p.Company).ThenInclude(c=>c.User)
				.Where(p => p.Id == positionId).FirstOrDefaultAsync();
			if(position == null)
			{
				return NotFound();
			}
			var profileId = position.Company.UserId;
			_context.Positions.Remove(position);
			await _context.SaveChangesAsync();
			TempData["Success"] = "Position deleted";
			return RedirectToAction("Profile", "User", new {id = profileId });
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
                .Include(c=>c.Company)
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
    }
}
