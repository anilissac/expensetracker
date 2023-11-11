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
    public class AccountController : Controller
    {
        //Declarations
        private AccountRepository R_Account;
        private SettingsRepository R_Settings;
        private readonly ILogger<AccountController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        public AccountController(ILogger<AccountController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            R_Account = new AccountRepository(_httpContextAccessor);
            R_Settings = new SettingsRepository(httpContextAccessor);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(AppUserView oUser)
        {
            ViewBag.AlertTitle = "Login Failed";
            ViewBag.AlertType = "error";

            var user = R_Account.ValidateUser(oUser.Username, oUser.Password);
            if (user != null)
            {
                //Cookies
                if (oUser.RememberMe)
                {
                    HttpContext.Response.Cookies.Append("SPINE_Username", oUser.Username);
                    HttpContext.Response.Cookies.Append("SPINE_Password", oUser.Password);
                }
                //Sessions
                HttpContext.Session.SetInt32("CurrentUserID", user.UserID);
                HttpContext.Session.SetString("CurrentUser", user.ContactName);
                HttpContext.Session.SetInt32("CurrentOrgUnitID", (Int32)user.DefaultOrgUnitID);
                HttpContext.Session.SetString("CurrentOrgUnitName", user.DefaultOrgUnitName);
                HttpContext.Session.SetInt32("CurrentSecurityRoleID", (Int32)user.DefaultSecurityRoleID);
                HttpContext.Session.SetString("CurrentSecurityRoleName", user.DefaultSecurityRoleName);
                
                string sControllerName = "Home";
                if ((Int32)user.DefaultSecurityRoleID == 1) sControllerName = "Admin";


                 R_Account.SignIN(user.UserID);
                _logger.LogInformation(oUser.Username + " has been Signed In.");

                return RedirectToAction("Index", sControllerName);

            }
            else
            {
                ViewBag.AlertMessage = "Invalid Credentials. ";
                return View("Index");
            }


        }
        public ActionResult LogOut()
        {
            //audit here
            if (HttpContext.Session.GetInt32("CurrentUserID") > 0)
            {
                _logger.LogInformation(HttpContext.Session.GetString("CurrentUser") + " has been logged out.");
                R_Account.SaveAuditTrail(HttpContext.Session.GetString("CurrentUser") + " has been logged out from web portal");
            }

            HttpContext.Session.SetInt32("CurrentUserID", 0);
            HttpContext.Session.SetString("CurrentUser", "");
            HttpContext.Session.SetInt32("CurrentOrgUnitID", 0);
            HttpContext.Session.SetString("CurrentOrgUnitName", "");
            HttpContext.Session.SetInt32("CurrentSecurityRoleID", 0);
            HttpContext.Session.SetString("CurrentSecurityRoleName", "");
            HttpContext.Session.SetString("ProfilePictureURL", "");
            return View("Index");
        }

        #region AppUsers
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult ListAppUsers()
        {
            ViewData["AppUsers"] = R_Account.GetAppUsers();
            //set permission
            ViewData["OrgUnits"] = R_Settings.GetOrgUnits();

            return View();
        }
        public JsonResult GetAppUsers()
        {
            var resultData = R_Account.GetAppUsers().Select(c => new { Value = c.UserID, Text = c.FirstName + " " + c.LastName }).OrderBy(x => x.Text).ToList();
            return Json(resultData);
        }
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult ViewAppUser(string sen)
        {
            int UserID = 0;
            if (sen != null) { try { UserID = Convert.ToInt32(Security.Decrypt(sen)); } catch (Exception ex) { _logger.LogError(ex.StackTrace); } }

            if (UserID == 0)
            {
                AppUserView lAppUser = new AppUserView();
                //defaults
                lAppUser.UserID = UserID;

                ViewData["AppUser"] = lAppUser;
            }
            else
            {
                ViewData["AppUser"] = R_Account.GetAppUser(UserID);
            }

            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(SessionTimeout))]
        public ActionResult SaveAppUser(AppUserView oAppUser)
        {
            oAppUser.CreatedBy = (int)HttpContext.Session.GetInt32("CurrentUserID");
            oAppUser.ModifiedBy = oAppUser.CreatedBy;
            var nAppUser = R_Account.SaveAppUser(oAppUser);
            // return RedirectToAction("ViewAppUser", new { sen = nAppUser.UserID.Encrypt() });
            return RedirectToAction("ListAppUsers");
        }
        [HttpPost]
        [TypeFilter(typeof(SessionTimeout))]
        public JsonResult UploadProfilePic(IFormFile postedFile, int UserID)
        {
            bool retval = false;
            if (postedFile != null)
            {
                string uploadsFolder = @"//uploads/users//" + UserID;

                string FileName = Server.DateNow().ToString("yyyyMMddhhmmss") + "_" + postedFile.FileName;
                string uploadedFilePath = FileHelper.UploadFile(postedFile, uploadsFolder, FileName);

                int ModifiedBy = (int)HttpContext.Session.GetInt32("CurrentUserID");
                R_Account.UpdateAppUserProfilePath(UserID, uploadedFilePath, ModifiedBy);
                retval = true;
            }
            return Json(retval);
        }
        [HttpPost]
        [TypeFilter(typeof(SessionTimeout))]
        public JsonResult RemoveAppUser(string sen)
        {
            int UserID = 0;
            if (sen != null) { try { UserID = Convert.ToInt32(Security.Decrypt(sen)); } catch (Exception ex) { _logger.LogError(ex.StackTrace); } }

            int ModifiedBy = (int)HttpContext.Session.GetInt32("CurrentUserID");
            R_Account.RemoveAppUser(UserID, ModifiedBy);
            return Json(true);
        }

        #endregion

        #region Audit Trail

        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult AuditTrail()
        {
            var StartDate = Server.DateNow().AddDays(-7);
            var EndDate = Server.DateNow();
            ViewData["AuditTrail"] = R_Account.GetAuditTrails(StartDate, EndDate);
            return View();
        }
        #endregion
    }
}
