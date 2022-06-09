using Microsoft.AspNetCore.Mvc;

namespace Dev_proc.Controllers
{
    public class CandidacyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
