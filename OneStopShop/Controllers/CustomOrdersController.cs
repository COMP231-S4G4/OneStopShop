using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OneStopShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Controllers
{
	public class CustomOrdersController : Controller
	{

		private readonly ApplicationDbContext _context;
		private static int currentStore = 0;

		public CustomOrdersController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Create()
		{
			return View();
		}
	}
}
