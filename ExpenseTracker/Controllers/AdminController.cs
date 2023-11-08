using ExpenseTracker.Filters;
using ExpenseTracker.DAL.Models;
using ExpenseTracker.DAL.Repositories;
using ExpenseTracker.DAL.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace ExpenseTracker.Controllers
{
    public class AdminController : Controller
    {
        //Declarations
        private AccountRepository R_Account;
        private SettingsRepository R_Settings;
        private readonly ILogger<AdminController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        public AdminController(ILogger<AdminController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            R_Account = new AccountRepository(_httpContextAccessor);
            R_Settings = new SettingsRepository(httpContextAccessor);
        }
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult Index()
        {
            ViewData["OrgUnits"] = R_Settings.GetOrgUnits();
            return View();
        }
    }
}
