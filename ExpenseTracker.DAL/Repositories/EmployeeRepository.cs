using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ExpenseTracker.DAL.Models;
using ExpenseTracker.DAL.Configurations;
using ExpenseTracker.DAL.Utilities;
using static QRCoder.PayloadGenerator;

namespace ExpenseTracker.DAL.Repositories
{
    public class EmployeeRepository
    {
        //Declarations
        private ETDBContext db = new ETDBContext();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private AccountRepository R_Account;
        public EmployeeRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            R_Account = new AccountRepository(httpContextAccessor);
        }


        public IEnumerable<EmployeeView> GetEmployees()
        {
            var _data = (from x in db.Employees
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         select new EmployeeView
                         {
                             enEmployeeID = Security.Encrypt(x.EmployeeID),
                             EmployeeID = x.EmployeeID,
                             EmployeeCode= x.EmployeeCode,
                             FirstName = x.FirstName,
                             LastName = x.LastName,
                             Designation=x.Designation,
                             ContactEmail = x.ContactEmail,
                             ContactNumber = x.ContactNumber,
                             ContactAddress = x.ContactAddress,
                             ContactPostalCode = x.ContactPostalCode,
                             StateID = x.StateID,
                             CountryID = x.CountryID,
                             PermanentAddress = x.PermanentAddress,
                             PermanentPostalCode = x.PermanentPostalCode,
                             PermanentStateID = x.PermanentStateID,
                             PermanentCountryID = x.PermanentCountryID,
                             PassportNumber = x.PassportNumber,
                             PassportIssuedOn = x.PassportIssuedOn,
                             PassportExpiresOn = x.PassportExpiresOn,
                             IDNumber = x.IDNumber,
                             IDExpiresOn = x.IDExpiresOn,
                             IDIssuedOn = x.IDIssuedOn,
                             ProfilePictureURL = x.ProfilePictureURL,
                             OrgUnitID = x.OrgUnitID,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(x => x.RecordStatus != 99);
            return _data.ToList();
        }

        public EmployeeView GetEmployee(int EmployeeID)
        {
            var _data = (from x in db.Employees
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         select new EmployeeView
                         {
                             enEmployeeID = Security.Encrypt(x.EmployeeID),
                             EmployeeID = x.EmployeeID,
                             EmployeeCode = x.EmployeeCode,
                             FirstName = x.FirstName,
                             LastName = x.LastName,
                             Designation = x.Designation,
                             JoinDate = x.JoinDate,
                             ContactEmail = x.ContactEmail,
                             ContactNumber = x.ContactNumber,
                             ContactAddress=x.ContactAddress,
                             ContactPostalCode = x.ContactPostalCode,
                             StateID = x.StateID,
                             CountryID = x.CountryID,
                             PermanentAddress = x.PermanentAddress,
                             PermanentPostalCode = x.PermanentPostalCode,
                             PermanentStateID = x.PermanentStateID,
                             PermanentCountryID = x.PermanentCountryID,
                             PassportNumber = x.PassportNumber,
                             PassportIssuedOn = x.PassportIssuedOn,
                             PassportExpiresOn = x.PassportExpiresOn,
                             IDNumber = x.IDNumber,
                             IDExpiresOn = x.IDExpiresOn,
                             IDIssuedOn = x.IDIssuedOn,
                             LabourCardNumber = x.LabourCardNumber,
                             LabourCardIssuedOn = x.LabourCardIssuedOn,
                             LabourCardExpiresOn = x.LabourCardExpiresOn,

                             BankName=x.BankName,
                             IBANNumber=x.IBANNumber,

                             ProfilePictureURL = x.ProfilePictureURL,
                             OrgUnitID = x.OrgUnitID,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(x => x.RecordStatus != 99 && x.EmployeeID == EmployeeID).FirstOrDefault();
            return _data;
        }

        public IEnumerable<EmployeeView> GetEmployeesPendingRenewal()
        {
            var _data = (from x in db.Employees
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         select new EmployeeView
                         {
                             enEmployeeID = Security.Encrypt(x.EmployeeID),
                             EmployeeID = x.EmployeeID,
                             EmployeeCode = x.EmployeeCode,
                             FirstName = x.FirstName,
                             LastName = x.LastName,
                             Designation = x.Designation,
                             ContactEmail = x.ContactEmail,
                             ContactNumber = x.ContactNumber,
                             ContactAddress = x.ContactAddress,
                             ContactPostalCode = x.ContactPostalCode,
                             StateID = x.StateID,
                             CountryID = x.CountryID,
                             PermanentAddress = x.PermanentAddress,
                             PermanentPostalCode = x.PermanentPostalCode,
                             PermanentStateID = x.PermanentStateID,
                             PermanentCountryID = x.PermanentCountryID,
                             PassportNumber = x.PassportNumber,
                             PassportIssuedOn = x.PassportIssuedOn,
                             PassportExpiresOn = x.PassportExpiresOn,
                             IDNumber = x.IDNumber,
                             IDExpiresOn = x.IDExpiresOn,
                             IDIssuedOn = x.IDIssuedOn,
                             LabourCardNumber = x.LabourCardNumber,
                             LabourCardIssuedOn = x.LabourCardIssuedOn,
                             LabourCardExpiresOn = x.LabourCardExpiresOn,
                             ProfilePictureURL = x.ProfilePictureURL,
                             OrgUnitID = x.OrgUnitID,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(x => x.RecordStatus != 99 && (x.PassportExpiresOn <= Server.DateNow() || x.IDExpiresOn <= Server.DateNow() || x.LabourCardExpiresOn <= Server.DateNow()));
            return _data.ToList();
        }

        public Employee SaveEmployee(EmployeeView oEmployee)
        {
            Employee lEmployee = new Employee();
            bool bEditMode = false;
            if (oEmployee.EmployeeID > 0)
            {
                lEmployee = db.Employees.Where(c => c.EmployeeID == oEmployee.EmployeeID).FirstOrDefault();
                bEditMode = true;
            }
            lEmployee.EmployeeCode = oEmployee.EmployeeCode;
            lEmployee.FirstName = oEmployee.FirstName;
            lEmployee.LastName = oEmployee.LastName;

            lEmployee.JoinDate = oEmployee.JoinDate;
            lEmployee.Designation = oEmployee.Designation;

            lEmployee.ContactNumber = oEmployee.ContactNumber;
            lEmployee.ContactEmail = oEmployee.ContactEmail;
            
            lEmployee.ContactAddress = oEmployee.ContactAddress;
            lEmployee.ContactPostalCode = oEmployee.ContactPostalCode;
            lEmployee.StateID = oEmployee.StateID;
            lEmployee.CountryID = oEmployee.CountryID;

            lEmployee.PermanentAddress = oEmployee.PermanentAddress;
            lEmployee.PermanentPostalCode = oEmployee.PermanentPostalCode;
            lEmployee.PermanentStateID = oEmployee.PermanentStateID;
            lEmployee.PermanentCountryID = oEmployee.PermanentCountryID;

            lEmployee.PassportNumber = oEmployee.PassportNumber;
            lEmployee.PassportIssuedOn = oEmployee.PassportIssuedOn;
            lEmployee.PassportExpiresOn = oEmployee.PassportExpiresOn;
           
            lEmployee.IDNumber = oEmployee.IDNumber;
            lEmployee.IDIssuedOn = oEmployee.IDIssuedOn;
            lEmployee.IDExpiresOn = oEmployee.IDExpiresOn;

            lEmployee.LabourCardNumber = oEmployee.LabourCardNumber;
            lEmployee.LabourCardIssuedOn = oEmployee.LabourCardIssuedOn;
            lEmployee.LabourCardExpiresOn = oEmployee.LabourCardExpiresOn;

            lEmployee.BankName = oEmployee.BankName;
            lEmployee.IBANNumber = oEmployee.IBANNumber;

            lEmployee.OrgUnitID = oEmployee.OrgUnitID;

            lEmployee.RecordStatus = 0;
            if (bEditMode)
            {
                lEmployee.ModifiedBy = oEmployee.CreatedBy;
                lEmployee.ModifiedDate = Server.DateNow();
            }
            else
            {
                lEmployee.CreatedBy = oEmployee.CreatedBy;
                lEmployee.CreatedDate = Server.DateNow();
                db.Employees.Add(lEmployee);
            }
            db.SaveChanges();
            return lEmployee;
        }

        public void RemoveEmployee(int EmployeeID, int? ModifiedBy)
        {
            if (EmployeeID > 0)
            {
                Employee lEmployee = db.Employees.Where(c => c.EmployeeID == EmployeeID).FirstOrDefault();
                if (lEmployee != null)
                {
                    lEmployee.RecordStatus = 99;
                    lEmployee.ModifiedBy = ModifiedBy;
                    lEmployee.ModifiedDate = Server.DateNow();
                    db.SaveChanges();
                }
            }
        }

        #region "Calendar Events"
        public List<CalendarEventView> GetCalendarEvents(DateTime StartDate, DateTime EndDate)
        {
            var _data = (from x in db.CalendarEvents
                       
                         select new CalendarEventView
                         {
                             CalendarEventID = x.CalendarEventID,
                             EventStartDateTime = x.EventStartDateTime,
                             EventEndDateTime = x.EventEndDateTime,
                             EventTitle = x.EventTitle,
                         
                             AssignedStaffs =x.AssignedStaffs,
                             EventDescription = x.EventDescription,
                             EventTypeID = x.EventTypeID,
                             NotificationColor = x.EventTypeID == 1 ?  "#ffa500" : "#669900",
                             OrgUnitID =x.OrgUnitID,
                             RecordStatus=x.RecordStatus,
                         }).Where(s => s.RecordStatus==0 &&  s.EventStartDateTime >= StartDate && s.EventEndDateTime <= EndDate).ToList();

            //re-mapping column names for full-calendar model 

            List<CalendarEventView> result = new List<CalendarEventView>();
            foreach (var item in _data)
            {
                CalendarEventView rec = new CalendarEventView();
                rec.ID = item.CalendarEventID;
                // rec.SomeImportantKeyID = item.SurgeonID.ToString();
                rec.StartDateString = item.EventStartDateTime.ToString("s"); // "s" is a preset format that outputs as: "2009-02-27T12:12:22"
                rec.EventStartDateTime = item.EventStartDateTime;
                rec.EndDateString = item.EventEndDateTime.ToString("s"); // "s" is a preset format that outputs as: "2009-02-27T12:12:22"
                rec.EventEndDateTime = item.EventEndDateTime;
                rec.Title = item.EventTitle;
                rec.Description = item.EventDescription;
                rec.EventTypeName = item.EventTypeName;
                rec.StatusColor = item.NotificationColor;
                result.Add(rec);
            }

            return result;
        }
     
        public CalendarEventView GetCalendarEvent(long CalendarEventID)
        {
            var _data = (from x in db.CalendarEvents
                         select new CalendarEventView
                         {
                             CalendarEventID = x.CalendarEventID,
                             EventStartDateTime = x.EventStartDateTime,
                             EventEndDateTime = x.EventEndDateTime,
                             EventTitle = x.EventTitle,
                             AssignedStaffs = x.AssignedStaffs,
                             EventDescription = x.EventDescription,
                             EventTypeID = x.EventTypeID,
                             NotificationColor = x.EventTypeID == 1 ? "#ffa500" : "#669900",
                             OrgUnitID = x.OrgUnitID,
                         }).Where(s => s.CalendarEventID == CalendarEventID).FirstOrDefault();

            return _data;
        }
        public void SaveCalendarEvent(CalendarEventView oCalendarEvent)
        {
            Models.CalendarEvent lCalendarEvent = db.CalendarEvents.Where(x => x.CalendarEventID == oCalendarEvent.CalendarEventID).FirstOrDefault();
            if (lCalendarEvent == null)
            {
                lCalendarEvent = new Models.CalendarEvent();
                lCalendarEvent.CalendarEventID = oCalendarEvent.CalendarEventID;

                lCalendarEvent.EventStartDateTime = oCalendarEvent.EventStartDateTime;
                lCalendarEvent.EventEndDateTime = oCalendarEvent.EventEndDateTime;
                lCalendarEvent.EventTitle = oCalendarEvent.EventTitle;
                lCalendarEvent.EventDescription = oCalendarEvent.EventDescription;
                lCalendarEvent.EventTypeID = oCalendarEvent.EventTypeID;


             if(oCalendarEvent.SelectedAssignedStaffs !=null) lCalendarEvent.AssignedStaffs = String.Join(",", oCalendarEvent.SelectedAssignedStaffs) ;

                if(lCalendarEvent.EventTypeID == 2) lCalendarEvent.EventTitle ="Tasks - " + lCalendarEvent.AssignedStaffs;
                else lCalendarEvent.EventTitle = "Leave - " + lCalendarEvent.AssignedStaffs;

                lCalendarEvent.CreatedBy = oCalendarEvent.CreatedBy;
                lCalendarEvent.CreatedDate = Server.DateNow();
                lCalendarEvent.ModifiedBy = oCalendarEvent.CreatedBy;
                lCalendarEvent.ModifiedDate = Server.DateNow();
                lCalendarEvent.RecordStatus = 0;
                lCalendarEvent.OrgUnitID = (int)_httpContextAccessor.HttpContext.Session.GetInt32("CurrentOrgUnitID");

                db.CalendarEvents.Add(lCalendarEvent);
                db.SaveChanges();
            }
            else
            {
                lCalendarEvent.CalendarEventID = oCalendarEvent.CalendarEventID;
                lCalendarEvent.EventStartDateTime = oCalendarEvent.EventStartDateTime;
                lCalendarEvent.EventEndDateTime = oCalendarEvent.EventEndDateTime;
                lCalendarEvent.EventTitle = oCalendarEvent.EventTitle;
                lCalendarEvent.EventDescription = oCalendarEvent.EventDescription;
                //lCalendarEvent.EventTypeID = oCalendarEvent.EventTypeID;


                if (oCalendarEvent.SelectedAssignedStaffs != null) lCalendarEvent.AssignedStaffs = String.Join(",", oCalendarEvent.SelectedAssignedStaffs);
                if (lCalendarEvent.EventTypeID == 2) lCalendarEvent.EventTitle = "Tasks - " + lCalendarEvent.AssignedStaffs;
                else lCalendarEvent.EventTitle = "Leave - " + lCalendarEvent.AssignedStaffs;

                lCalendarEvent.EventTypeID = oCalendarEvent.EventTypeID;
                lCalendarEvent.ModifiedBy = oCalendarEvent.ModifiedBy;
                lCalendarEvent.ModifiedDate = Server.DateNow();
                db.SaveChanges();
            }

        }

        public void UpdateCalendarEvent(CalendarEventView oCalendarEvent)
        {
            Models.CalendarEvent lCalendarEvent = db.CalendarEvents.Where(x => x.CalendarEventID == oCalendarEvent.CalendarEventID).FirstOrDefault();
           if(lCalendarEvent!=null)
            {
                lCalendarEvent.CalendarEventID = oCalendarEvent.CalendarEventID;
                lCalendarEvent.EventStartDateTime = oCalendarEvent.EventStartDateTime;
                lCalendarEvent.EventEndDateTime = oCalendarEvent.EventEndDateTime;
                lCalendarEvent.ModifiedBy = oCalendarEvent.ModifiedBy;
                lCalendarEvent.ModifiedDate = Server.DateNow();
                db.SaveChanges();
            }

        }
        public void RemoveCalendarEvent(int CalendarEventID, int? ModifiedBy)
        {
            if (CalendarEventID > 0)
            {

                Models.CalendarEvent lCalendarEvent = db.CalendarEvents.Where(x => x.CalendarEventID == CalendarEventID).FirstOrDefault();
                if (lCalendarEvent != null)
                {
                    lCalendarEvent.RecordStatus = 99;
                    lCalendarEvent.ModifiedBy = ModifiedBy;
                    lCalendarEvent.ModifiedDate = Server.DateNow();
                    db.SaveChanges();
                    R_Account.SaveAuditTrail("Schedule " + lCalendarEvent.EventTitle + " has been removed.");
                }

            }
        }
        #endregion
    }
}
