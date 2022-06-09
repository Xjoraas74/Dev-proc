using Dev_proc.Constants.Configuration;
using Dev_proc.Data;
using Dev_proc.Models.CompanyModels;
using Dev_proc.Models.ViewModels.CompanyViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dev_proc.Controllers
{
    [Route("company")]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Route("{id:guid}")]
        public async Task<IActionResult> CompanyProfile(Guid id)
        {
            var company = await _context.Companies
                .Where(c => c.Id == id)
                .Include(c=>c.Positions)
                .FirstOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }
        [Route("list")]
        [Authorize]
        public async Task<IActionResult> CompaniesList()
        {
            var companies = await _context.Companies
                .Include(c=>c.User)
                .Include(c => c.Positions).ThenInclude(p=>p.Applications)
                .ToListAsync();
            return View(companies);
        }
        [Route("add_position/{companyId:guid}")]
        [Authorize(Roles = ApplicationRoleNames.AdminAndCompany)]
        [HttpGet]
        public async Task<IActionResult> AddPositionToCompany(Guid companyId)
        {
            var company = await _context.Companies.Where(c => c.Id == companyId).FirstOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }
            return View("AddPosition", new AddPositionViewModel { CompanyId = companyId });
        }
        [Authorize(Roles = ApplicationRoleNames.AdminAndCompany)]
        [HttpPost]
        public async Task<IActionResult> AddPositionToCompanyPost(AddPositionViewModel model)
        {
            var company = await _context.Companies.Where(c => c.Id == model.CompanyId).FirstOrDefaultAsync();
            if (model.Name == null)
            {
                TempData["Error"] = "Name required";
                return View(model);
            }
            if(model.AvailablePlaces!=null && model.AvailablePlaces < 1)
            {
                TempData["Error"] = "Available places less than 1";
            }
            Position position = new Position
            {
                Company = company,
                CompanyId = model.CompanyId,
                Name = model.Name,
                Description = model.Description,
                AvailablePlaces = model.AvailablePlaces
            };
            company.Positions.Add(position);
            _context.Positions.Add(position);
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
            return RedirectToAction("Profile","User", new { id = company.UserId });
        }
    }
}
