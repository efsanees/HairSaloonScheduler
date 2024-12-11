using Microsoft.AspNetCore.Mvc;

namespace HairSaloonScheduler.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
