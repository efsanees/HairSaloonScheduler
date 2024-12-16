using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HairSaloonScheduler.Models;

namespace HairSaloonScheduler.Controllers
{
    public class AdminController : Controller
    {        

        [HttpGet]
        public IActionResult AdminPanel()
        {
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminEmail")))
            //{
            //    return RedirectToAction("AdminLogin","Login");
            //}

            return View();
        }
    }
}
