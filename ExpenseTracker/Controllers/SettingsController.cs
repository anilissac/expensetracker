using ExpenseTracker.Filters;
using ExpenseTracker.DAL.Models;
using ExpenseTracker.DAL.Repositories;
using ExpenseTracker.DAL.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ExpenseTracker.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        private SettingsRepository R_Settings;
        private AccountRepository R_Account;

        public SettingsController(ILogger<SettingsController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            R_Settings = new SettingsRepository(httpContextAccessor);
            R_Account = new AccountRepository(httpContextAccessor);

        }
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult Index()
        {
            return View();
        }
        #region OrgUnits
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult ListOrgUnits()
        {
            ViewData["OrgUnits"] = R_Settings.GetOrgUnits();
            return View();
        }
        public JsonResult GetOrgUnits()
        {
            var resultData = R_Settings.GetOrgUnits().Select(c => new { Value = c.OrgUnitID, Text = c.OrgUnitName }).OrderBy(x => x.Text).ToList();
            return Json(resultData);
        }
        public JsonResult GetOrgUnit(string sen)
        {
            int OrgUnitID = 0;
            if (sen != null)
            {
                try { OrgUnitID = Convert.ToInt32(Security.Decrypt(sen)); } catch (Exception ex) { _logger.LogError(ex.StackTrace); }
            }
            var resultData = R_Settings.GetOrgUnit(OrgUnitID);
            return Json(resultData);
        }
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult ViewOrgUnit(string sen)
        {
            int OrgUnitID = 0;
            if (sen != null) { try { OrgUnitID = Convert.ToInt32(Security.Decrypt(sen)); } catch (Exception ex) { _logger.LogError(ex.StackTrace); } }

            if (OrgUnitID == 0)
            {
                OrgUnitView lOrgUnit = new OrgUnitView();
                //defaults
                lOrgUnit.OrgUnitID = OrgUnitID;

                ViewData["OrgUnit"] = lOrgUnit;
            }
            else
            {
                ViewData["OrgUnit"] = R_Settings.GetOrgUnit(OrgUnitID);
            }
            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(SessionTimeout))]
        public ActionResult SaveOrgUnit(OrgUnitView oOrgUnit)
        {
            oOrgUnit.CreatedBy = (int)HttpContext.Session.GetInt32("CurrentUserID");
            oOrgUnit.ModifiedBy = oOrgUnit.CreatedBy;
            var nOrgUnit = R_Settings.SaveOrgUnit(oOrgUnit);
            return RedirectToAction("ViewOrgUnit", new { sen = nOrgUnit.OrgUnitID.Encrypt() });
        }
        [HttpPost]
        [TypeFilter(typeof(SessionTimeout))]
        public JsonResult RemoveOrgUnit(string sen)
        {
            int OrgUnitID = 0;
            if (sen != null) { try { OrgUnitID = Convert.ToInt32(Security.Decrypt(sen)); } catch (Exception ex) { _logger.LogError(ex.StackTrace); } }

            int ModifiedBy = (int)HttpContext.Session.GetInt32("CurrentUserID");
            R_Settings.RemoveOrgUnit(OrgUnitID, ModifiedBy);
            return Json(true);
        }
        #endregion
        #region Security Roles
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult ListSecurityRoles()
        {
            return View();
        }
        public JsonResult GetSecurityRoles()
        {
            var resultData = R_Account.GetSecurityRoles().Select(c => new { Value = c.SecurityRoleID, Text = c.SecurityRoleName }).OrderBy(x => x.Text).ToList();
            return Json(resultData);
        }
        public JsonResult GetSecurityRole(int SecurityRoleID)
        {
            var resultData = R_Account.GetSecurityRole(SecurityRoleID);
            return Json(resultData);
        }
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult ViewSecurityRole(string sen)
        {
            int SecurityRoleID = 0;
            if (sen != "") { try { SecurityRoleID = Convert.ToInt32(Security.Decrypt(sen)); } catch (Exception ex) { _logger.LogError(ex.StackTrace); } }

            if (SecurityRoleID == 0)
            {
                SecurityRoleView lSecurityRole = new SecurityRoleView();
                //defaults
                lSecurityRole.SecurityRoleID = SecurityRoleID;

                ViewData["SecurityRole"] = lSecurityRole;
            }
            else
            {
                ViewData["SecurityRole"] = R_Account.GetSecurityRole(SecurityRoleID);
            }

            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(SessionTimeout))]
        public ActionResult SaveSecurityRole(SecurityRoleView oSecurityRole)
        {
            oSecurityRole.CreatedBy = (int)HttpContext.Session.GetInt32("CurrentUserID");
            oSecurityRole.ModifiedBy = oSecurityRole.CreatedBy;
            var nSecurityRole = R_Account.SaveSecurityRole(oSecurityRole);
            return RedirectToAction("ViewSecurityRole", new { sen = nSecurityRole.SecurityRoleID.Encrypt() });
        }

        [HttpPost]
        [TypeFilter(typeof(SessionTimeout))]
        public JsonResult RemoveSecurityRole(string sen)
        {
            int SecurityRoleID = 0;
            if (sen != null) { try { SecurityRoleID = Convert.ToInt32(Security.Decrypt(sen)); } catch (Exception ex) { _logger.LogError(ex.StackTrace); } }

            int ModifiedBy = (int)HttpContext.Session.GetInt32("CurrentUserID");
            R_Account.RemoveSecurityRole(SecurityRoleID, ModifiedBy);
            return Json(true);
        }
        #endregion

        #region App Settings
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult Configuration()
        {
            ViewData["AppSettings"] = R_Settings.GetAppSettings().OrderBy(c => c.Category);
            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(SessionTimeout))]
        public JsonResult SetAppSetting(int AppSettingID, string AppSettingValue)
        {
            int Action = 0;
            AppSetting lAppSetting = new AppSetting();
            lAppSetting.AppSettingID = (byte)AppSettingID;
            lAppSetting.AppSettingValue = AppSettingValue;
            Action = R_Settings.SaveAppSetting(lAppSetting);
            return Json(Action);
        }
        #endregion
        public JsonResult GetCountries()
        {
            var resultData = R_Settings.GetCountries().Select(c => new { Value = c.CountryID, Text = c.CountryName }).OrderBy(x => x.Text).ToList();
            return Json(resultData);
        }

        public JsonResult GetStates(int CountryID)
        {
            var resultData = R_Settings.GetStates(CountryID).Select(c => new { Value = c.StateID, Text = c.StateName }).OrderBy(x => x.Text).ToList();
            return Json(resultData);
        }

    }
}
