using ExpenseTracker.DAL.Models;
using ExpenseTracker.DAL.Repositories;
using ExpenseTracker.DAL.Utilities;
using ExpenseTracker.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace ExpenseTracker.Controllers
{
    public class SalesController : Controller
    {
        //Declarations
        private SaleRepository R_Sale;
        private AccountRepository R_Account;
        private SettingsRepository R_Settings;
        private readonly ILogger<SalesController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        public SalesController(ILogger<SalesController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            R_Sale = new SaleRepository(_httpContextAccessor);
            R_Account = new AccountRepository(_httpContextAccessor);
            R_Settings = new SettingsRepository(httpContextAccessor);
        }
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult Index()
        {
            ViewData["Sales"] = R_Sale.GetSales();
            return View();
        }
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult Create()
        {
            ViewData["Sales"] = R_Sale.GetTodaysSales();
            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(SessionTimeout))]
        public ActionResult SaveSales(SaleView oSale)
        {
            oSale.OrgUnitID = (int)HttpContext.Session.GetInt32("CurrentOrgUnitID");
            oSale.CreatedBy = (int)HttpContext.Session.GetInt32("CurrentUserID");
            oSale.ModifiedBy = oSale.CreatedBy;
            var nSale = R_Sale.SaveSale(oSale);
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        [TypeFilter(typeof(SessionTimeout))]
        public JsonResult RemoveSales(string sen)
        {
            int SalesID = 0;
            if (sen != null) { try { SalesID = Convert.ToInt32(Security.Decrypt(sen)); } catch (Exception ex) { _logger.LogError(ex.StackTrace); } }

            int ModifiedBy = (int)HttpContext.Session.GetInt32("CurrentUserID");
            R_Sale.RemoveSale(SalesID, ModifiedBy);
            return Json(true);
        }
    }
}
