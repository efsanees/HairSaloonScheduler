using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HairSaloonScheduler.Models;
using Microsoft.AspNetCore.Authorization;

namespace HairSaloonScheduler.Controllers
{
    public class AdminController : Controller
    {        

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}
