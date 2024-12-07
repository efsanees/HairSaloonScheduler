using HairSaloonScheduler.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairSaloonScheduler.Controllers
{
    public class SaloonController : Controller
    {
        private readonly MyDbContext _context;
        public SaloonController(MyDbContext context) 
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        
    }
}
