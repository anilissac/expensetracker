using ExpenseTracker.DAL.Models;
using ExpenseTracker.DAL.Repositories;
using ExpenseTracker.DAL.Utilities;
using ExpenseTracker.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ExpenseTracker.Controllers
{
    public class PurchaseController : Controller
    {
        //Declarations
        private PurchaseRepository R_Purchase;
        private AccountRepository R_Account;
        private SettingsRepository R_Settings;
        private readonly ILogger<PurchaseController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        public PurchaseController(ILogger<PurchaseController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            R_Purchase = new PurchaseRepository(_httpContextAccessor);
            R_Account = new AccountRepository(_httpContextAccessor);
            R_Settings = new SettingsRepository(httpContextAccessor);
        }
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult Index()
        {
            ViewData["Purchases"] = R_Purchase.GetPurchases();
            return View();
        }
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult Create()
        {
            ViewData["Purchases"] = R_Purchase.GetTodaysPurchases();
            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(SessionTimeout))]
        public ActionResult SavePurchase(PurchaseView oPurchase)
        {
            oPurchase.OrgUnitID = (int)HttpContext.Session.GetInt32("CurrentOrgUnitID");
            oPurchase.CreatedBy = (int)HttpContext.Session.GetInt32("CurrentUserID");
            oPurchase.ModifiedBy = oPurchase.CreatedBy;
            var nPurchase = R_Purchase.SavePurchase(oPurchase);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [TypeFilter(typeof(SessionTimeout))]
        public JsonResult RemovePurchase(string sen)
        {
            int PurchasesID = 0;
            if (sen != null) { try { PurchasesID = Convert.ToInt32(Security.Decrypt(sen)); } catch (Exception ex) { _logger.LogError(ex.StackTrace); } }

            int ModifiedBy = (int)HttpContext.Session.GetInt32("CurrentUserID");
            R_Purchase.RemovePurchase(PurchasesID, ModifiedBy);
            return Json(true);
        }

        public JsonResult GetVendors()
        {
            var resultData = R_Purchase.GetVendors().Select(c => new { Value = c.VendorID, Text = c.VendorName }).OrderBy(x => x.Text).ToList();
            return Json(resultData);
        }
    }
}
