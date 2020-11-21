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
        //public readonly IMapper _mapper;
        public IWebHostEnvironment Environment;

        public readonly ApplicationDbContext _context;

        // private  ApplicationDbContext context;
        private IDataProtectionProvider provider;

        private IHttpContextAccessor httpContextAccessor;

        //public readonly ApplicationDbContext _context;
        public readonly IDataProtector protector;

        public readonly IHttpContextAccessor _httpContextAccessor;

        //private IDataProtectionProvider provider;
        //private IHttpContextAccessor httpContextAccessor;

        public BaseController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment)
        {
            _context = context;
            // _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            protector = provider.CreateProtector("idProtector");
            Environment = _environment;
        }

        //public BaseController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment)
        //{
        //    this.context = context;
        //    this.provider = provider;
        //    this.httpContextAccessor = httpContextAccessor;
        //    Environment = environment;
        //}

        //public BaseController(AppContext context, IMapper mapper, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment)
        //{
        //    this.context = context;
        //    this.mapper = mapper;
        //    this.provider = provider;
        //    this.httpContextAccessor = httpContextAccessor;
        //    Environment = environment;
        //}
    }
}