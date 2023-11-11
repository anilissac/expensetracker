using ExpenseTracker.DAL.Models;
using ExpenseTracker.DAL.Repositories;
using ExpenseTracker.DAL.Utilities;
using ExpenseTracker.Filters;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Controllers
{
    public class HomeController : Controller
    {
        private PurchaseRepository R_Purchase;
        private SaleRepository R_Sale;
        private AccountRepository R_Account;
        private SettingsRepository R_Settings;
        private EmployeeRepository R_Employee;
        private readonly ILogger<HomeController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            R_Purchase = new PurchaseRepository(_httpContextAccessor);
            R_Sale = new SaleRepository(_httpContextAccessor);
            R_Account = new AccountRepository(_httpContextAccessor);
            R_Settings = new SettingsRepository(httpContextAccessor);
            R_Employee = new EmployeeRepository(httpContextAccessor);
        }
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult Index()
        {
            var oTodaysPurchases = R_Purchase.GetTodaysPurchases();
            var oTodaysSales = R_Sale.GetTodaysSales();
            ViewBag.TotalPurchaseAmount = oTodaysPurchases.Sum(c=>c.TotalAmount);
            ViewBag.TotalCashAmount = oTodaysSales.Sum(c=>c.CashAmount);
            ViewBag.TotalBankAmount = oTodaysSales.Sum(c => c.BankAmount);
            ViewBag.TotalCreditAmount = oTodaysSales.Sum(c => c.CreditAmount);

            ViewData["CalendarEvents"] = R_Employee.GetTaskAssignments((int)HttpContext.Session.GetInt32("CurrentOrgUnitID"), (int)HttpContext.Session.GetInt32("CurrentUserID"));
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
