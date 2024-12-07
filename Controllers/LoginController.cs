using HairSaloonScheduler.Context;
using HairSaloonScheduler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairSaloonScheduler.Controllers
{
	public class LoginController : Controller
	{
		private readonly MyDbContext _context;

		public LoginController(MyDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Login(User user)
		{
			var infos= await _context.users.FirstOrDefaultAsync(x => x.UserMail == user.UserMail && x.Password == user.Password);
			if (infos != null)
			{
				return RedirectToAction("Index", "Saloon");
			}
			else
			{
				ViewBag.Error = "Böyle bir kullanıcı bulunamadı.Önce üyelik oluşturunuz.";
				return View();
			}
		}

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(User user)
		{
			if(user!=null)
			{
                await _context.AddAsync(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));

            }
				
			return View(user);
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(Admin admin)
        {
            var infos = await _context.admins.FirstOrDefaultAsync(x => x.AdminMail == admin.AdminMail && x.Password == admin.Password);
            if (infos != null)
            {
                return RedirectToAction("Index", "Saloon");
            }
            else
            {
                ViewBag.Error = "Email veya şifre yanlış.";
                return View();
            }
        }

    }
}
