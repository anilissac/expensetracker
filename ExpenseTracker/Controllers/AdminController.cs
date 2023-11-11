using ExpenseTracker.Filters;
using ExpenseTracker.DAL.Models;
using ExpenseTracker.DAL.Repositories;
using ExpenseTracker.DAL.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ExpenseTracker.Controllers
{
    public class AdminController : Controller
    {
        //Declarations
        private AccountRepository R_Account;
        private SettingsRepository R_Settings;
        private EmployeeRepository R_Employee;
        private readonly ILogger<AdminController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        public AdminController(ILogger<AdminController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            R_Account = new AccountRepository(_httpContextAccessor);
            R_Settings = new SettingsRepository(httpContextAccessor);
            R_Employee = new EmployeeRepository(httpContextAccessor);
        }
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult Index()
        {
            ViewData["OrgUnits"] = R_Settings.GetOrgUnits();
            return View();
        }
        #region Calendar
        [TypeFilter(typeof(SessionTimeout))]
        public IActionResult Calendar()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetCalendarEvents(double StartDate, double EndDate)
        {
            var oStartDate = DAL.Utilities.Utilities.ConvertFromUnixTimestamp(StartDate);
            var oEndDate = DAL.Utilities.Utilities.ConvertFromUnixTimestamp(EndDate);
            var oCalendarEvents = R_Employee.GetCalendarEvents(Server.DateNow().AddYears(-1), Server.DateNow().AddYears(1));

            var eventList = from e in oCalendarEvents
                            select new
                            {
                                id = e.ID,
                                title = e.Title,
                                start = e.StartDateString,
                                end = e.EndDateString,
                                color = e.StatusColor,
                                someKey = e.SomeImportantKeyID,
                                description = e.Description,
                                allDay = false
                            };
            var rows = eventList.ToArray();
            return Json(rows);
        }

        [HttpGet]
        public JsonResult GetCalendarEvent(long CalendarEventID)
        {
            var resultData = R_Employee.GetCalendarEvent(CalendarEventID);
            return Json(resultData);
        }
        [TypeFilter(typeof(SessionTimeout))]
        [HttpPost]
        public JsonResult SaveCalendarEvent(CalendarEventView oCalendarEvent)
        {
            oCalendarEvent.CreatedBy = (int)HttpContext.Session.GetInt32("CurrentUserID");
            R_Employee.SaveCalendarEvent(oCalendarEvent);
            return Json(true);
        }
        [TypeFilter(typeof(SessionTimeout))]
        [HttpPost]
        public JsonResult UpdateCalendarEvent(CalendarEventView oCalendarEvent)
        {

            oCalendarEvent.ModifiedBy = (int)HttpContext.Session.GetInt32("CurrentUserID");
            R_Employee.UpdateCalendarEvent(oCalendarEvent);
            return Json(true);
        }
        [HttpPost]
        [TypeFilter(typeof(SessionTimeout))]
        public JsonResult RemoveCalendarEvent(int CalendarEventID)
        {
            int ModifiedBy = (int)HttpContext.Session.GetInt32("CurrentUserID");
            R_Employee.RemoveCalendarEvent(CalendarEventID, ModifiedBy);
            return Json(true);
        }
        #endregion
    }
}
