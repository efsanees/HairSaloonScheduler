﻿using HairSaloonScheduler.Context;
using HairSaloonScheduler.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            var infos = await _context.users.FirstOrDefaultAsync(x => x.UserMail == user.UserMail && x.Password == user.Password);
            if (infos != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, infos.UserId.ToString()),
            new Claim(ClaimTypes.Name, infos.Username),
            new Claim(ClaimTypes.Email, infos.UserMail)
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);


                return RedirectToAction("Index", "Saloon");
            }
            else
            {
                ViewBag.Error = "Böyle bir kullanıcı bulunamadı. Önce üyelik oluşturunuz.";
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
            if (user != null)
            {
                await _context.AddAsync(user);
                await _context.SaveChangesAsync();

                // Cookie'ye kullanıcı bilgilerini ekle
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // UserId
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.UserMail)
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Kalıcı oturum
                    ExpiresUtc = DateTime.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

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
                // Cookie'ye admin bilgilerini ekle
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, infos.AdminId.ToString()), // AdminId
            new Claim(ClaimTypes.Email, infos.AdminMail),
            new Claim("Role", "Admin") // Rol bilgisi eklenebilir
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Kalıcı oturum
                    ExpiresUtc = DateTime.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

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
