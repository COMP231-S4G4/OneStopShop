using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneStopShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Controllers
{
    public class BaseController : Controller
    {
        public IWebHostEnvironment Environment;

        public readonly ApplicationDbContext _context;

        private IDataProtectionProvider provider;

        private IHttpContextAccessor httpContextAccessor;

        public readonly IDataProtector protector;

        public readonly IHttpContextAccessor _httpContextAccessor;

        public BaseController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            protector = provider.CreateProtector("idProtector");
            Environment = _environment;
        }
    }
}