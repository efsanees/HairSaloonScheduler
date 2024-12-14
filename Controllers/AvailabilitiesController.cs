using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HairSaloonScheduler.Context;
using HairSaloonScheduler.Models;

namespace HairSaloonScheduler.Controllers
{
	public class AvailabilitiesController : Controller
	{
		private readonly MyDbContext _context;

		public AvailabilitiesController(MyDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var availabilities = _context.availabilities
			.Include(a => a.Employee)
			.ToListAsync();

			return availabilities != null
				? View(await availabilities)
				: Problem("Entity set 'MyDbContext.Availabilities' is null.");
		}
	}
}
