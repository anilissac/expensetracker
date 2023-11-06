using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ExpenseTracker.DAL.Models;
using ExpenseTracker.DAL.Configurations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ExpenseTracker.DAL.Utilities;
using System.Linq.Expressions;
using System.Net.Mail;

namespace ExpenseTracker.DAL.Repositories
{
    public class SettingsRepository
    {
        //Declarations
        private ETDBContext db = new ETDBContext();
        private readonly IHttpContextAccessor _httpContextAccessor;

        private AccountRepository R_Account;
        public SettingsRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            R_Account = new AccountRepository(httpContextAccessor);

        }

        #region Email
        public void NotifyDeveloper(Exception oException)
        {

            IEnumerable<AppSetting> oAppSettings = GetAppSettings();

            if (oAppSettings.Where(c => c.AppSettingID == (int)Enums.AppSettings.ErrorReporting).FirstOrDefault().AppSettingValue.Trim() == "ON")
            {
                MailMessage mail = new MailMessage();

                mail.To.Add(oAppSettings.Where(c => c.AppSettingID == (int)Enums.AppSettings.DeveloperMail).FirstOrDefault().AppSettingValue);
                mail.Subject = "Error Reporting : " + oAppSettings.Where(c => c.AppSettingID == (int)Enums.AppSettings.DatabaseMode).FirstOrDefault().AppSettingValue;

                string sHTMLBody = "";
                string sIPAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                sHTMLBody = sHTMLBody + "<table border=1 cellspacing=1 cellpadding=1>";
                sHTMLBody = sHTMLBody + "<tr><td><strong>Environment: </strong>" + oAppSettings.Where(c => c.AppSettingID == (int)Enums.AppSettings.DatabaseMode).FirstOrDefault().AppSettingValue + "</td>";
                sHTMLBody = sHTMLBody + "</tr>";
                sHTMLBody = sHTMLBody + "<tr><td><strong>Current User: </strong>" + _httpContextAccessor.HttpContext.Session.GetString("CurrentUser") + "</td>";
                sHTMLBody = sHTMLBody + "</tr>";
                sHTMLBody = sHTMLBody + "<tr><td><strong>IP Address: </strong>" + sIPAddress + "</td>";
                sHTMLBody = sHTMLBody + "</tr>";
                sHTMLBody = sHTMLBody + "<tr><td><strong>Reported Time: </strong>" + Server.DateNow().ToString("dd/MM/yyyy hh:mm tt") + "</td>";
                sHTMLBody = sHTMLBody + "</tr>";
                sHTMLBody = sHTMLBody + "<tr><td><strong>Host: </strong>" + _httpContextAccessor.HttpContext.Request.Host.ToString() + "</td>";
                sHTMLBody = sHTMLBody + "</tr>";
                sHTMLBody = sHTMLBody + "<tr><td><strong>URL: </strong>" + _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString() + "</td>";
                sHTMLBody = sHTMLBody + "</tr>";
                sHTMLBody = sHTMLBody + "<tr><td><strong>Error Message: </strong><br>" + oException.Message.ToString() + "</td>";
                sHTMLBody = sHTMLBody + "</tr>";
                sHTMLBody = sHTMLBody + "<tr><td><strong>Source: </strong>" + oException.Source + "</td>";
                sHTMLBody = sHTMLBody + "</tr>";
                sHTMLBody = sHTMLBody + "<tr><td><strong>Stack Trace: </strong><br>" + oException.StackTrace.ToString() + "</td>";
                sHTMLBody = sHTMLBody + "</tr>";
                sHTMLBody = sHTMLBody + "</table>";

                mail.Body = sHTMLBody;

                Mailer.SendMail(mail, oAppSettings, 0);
            }


        }
        #endregion

        #region Application Settings
        public IEnumerable<AppSetting> GetAppSettings()
		{
			return db.AppSettings.ToList();
		}
        public int SaveAppSetting(AppSetting oAppSetting)
        {
            int Action = 0;//This Action is Used for  Identify this action is creat or edit
            AppSetting lAppSetting = db.AppSettings.Where(c => c.AppSettingID == oAppSetting.AppSettingID).FirstOrDefault();
            if (lAppSetting != null)
            {//Edit

                lAppSetting.AppSettingValue = oAppSetting.AppSettingValue;
                db.SaveChanges();
                Action = 2;// Update
            }

            else
            {
                lAppSetting = new AppSetting();
                lAppSetting.AppSettingValue = oAppSetting.AppSettingValue;
                db.AppSettings.Add(lAppSetting);
                db.SaveChanges();
                Action = 1;// Create
            }
            return Action;
        }

        #endregion

		#region OrgUnits
		public IEnumerable<OrgUnitView> GetOrgUnits()
        {
            var _data = (from x in db.OrgUnits
                         join a in db.Countries on x.CountryID equals a.CountryID into kxa
                         from xa in kxa.DefaultIfEmpty()
                         join b in db.States on x.StateID equals b.StateID into kxb
                         from xb in kxb.DefaultIfEmpty()
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         select new OrgUnitView
                         {
                             enOrgUnitID = Security.Encrypt(x.OrgUnitID),
                             OrgUnitID = x.OrgUnitID,
                             OrgUnitName = x.OrgUnitName,
                             ContactPerson = x.ContactPerson,
                             ContactEmail = x.ContactEmail,
                             ContactNumber = x.ContactNumber,
                             ContactAddress = x.ContactAddress,
                             ContactPostalCode = x.ContactPostalCode,
                             StateID = x.StateID,
                             StateName = xb.StateName,
                             CountryID = x.CountryID,
                             CountryName = xa.CountryName,
                             Website = x.Website,
                             DefaultCurrencyID = x.DefaultCurrencyID,
                             TRN = x.TRN,
                             LogoURL = x.LogoURL,
                             AuthorisedSignatureURL = x.AuthorisedSignatureURL,
                             AuthorisedStampSealURL = x.AuthorisedStampSealURL,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(x => x.RecordStatus != 99);
            return _data.ToList();
        }
        public OrgUnitView GetOrgUnit(int OrgUnitID)
        {
            var _data = (from x in db.OrgUnits
                         join a in db.Countries on x.CountryID equals a.CountryID into kxa
                         from xa in kxa.DefaultIfEmpty()
                         join b in db.States on x.StateID equals b.StateID into kxb
                         from xb in kxb.DefaultIfEmpty()
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         select new OrgUnitView
                         {
                             enOrgUnitID = Security.Encrypt(x.OrgUnitID),
                             OrgUnitID = x.OrgUnitID,
                             OrgUnitName = x.OrgUnitName,
                             ContactPerson = x.ContactPerson,
                             ContactEmail = x.ContactEmail,
                             ContactNumber = x.ContactNumber,
                             ContactAddress = x.ContactAddress,
                             ContactPostalCode = x.ContactPostalCode,
                             StateID = x.StateID,
                             StateName = xb.StateName,
                             CountryID = x.CountryID,
                             CountryName = xa.CountryName,
                             Website = x.Website,
                             DefaultCurrencyID = x.DefaultCurrencyID,
                             TRN = x.TRN,
                             LogoURL = x.LogoURL,
                             AuthorisedSignatureURL = x.AuthorisedSignatureURL,
                             AuthorisedStampSealURL = x.AuthorisedStampSealURL,
                             BillingAddressLine1=x.BillingAddressLine1,
                             BillingAddressLine2 = x.BillingAddressLine2,
                             BillingAddressLine3 = x.BillingAddressLine3,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(x => x.OrgUnitID == OrgUnitID).FirstOrDefault();
            return _data;
        }

        public OrgUnit SaveOrgUnit(OrgUnitView oOrgUnit)
        {
            OrgUnit lOrgUnit = new OrgUnit();
            bool bEditMode = false;
            if (oOrgUnit.OrgUnitID > 0)
            {
                lOrgUnit = db.OrgUnits.Where(c => c.OrgUnitID == oOrgUnit.OrgUnitID).FirstOrDefault();
                bEditMode = true;
            }
            lOrgUnit.OrgUnitName = oOrgUnit.OrgUnitName;
            lOrgUnit.ContactPerson = oOrgUnit.ContactPerson;
            lOrgUnit.ContactNumber = oOrgUnit.ContactNumber;
            lOrgUnit.ContactEmail = oOrgUnit.ContactEmail;
            lOrgUnit.ContactAddress = oOrgUnit.ContactAddress;
            lOrgUnit.ContactPostalCode = oOrgUnit.ContactPostalCode;
            lOrgUnit.StateID = oOrgUnit.StateID;
            lOrgUnit.CountryID = oOrgUnit.CountryID;
            lOrgUnit.Website = oOrgUnit.Website;
            lOrgUnit.Description = oOrgUnit.Description;
            lOrgUnit.DefaultCurrencyID = oOrgUnit.DefaultCurrencyID;
            lOrgUnit.TRN = oOrgUnit.TRN;
            lOrgUnit.BillingAddressLine1 = oOrgUnit.BillingAddressLine1;
            lOrgUnit.BillingAddressLine2 = oOrgUnit.BillingAddressLine2;
            lOrgUnit.BillingAddressLine3 = oOrgUnit.BillingAddressLine3;

            lOrgUnit.RecordStatus = 0;
            if (bEditMode)
            {
                lOrgUnit.ModifiedBy = oOrgUnit.CreatedBy;
                lOrgUnit.ModifiedDate = Server.DateNow();
            }
            else
            {
                lOrgUnit.CreatedBy = oOrgUnit.CreatedBy;
                lOrgUnit.CreatedDate = Server.DateNow();
                db.OrgUnits.Add(lOrgUnit);
            }
            db.SaveChanges();
            if (bEditMode)
                R_Account.SaveAuditTrail("New OrgUnit " + oOrgUnit.OrgUnitName + " has been registered.");
            else
                R_Account.SaveAuditTrail("OrgUnit " + oOrgUnit.OrgUnitName + " has been modified.");
            return lOrgUnit;
        }
        public void RemoveOrgUnit(int OrgUnitID, int? ModifiedBy)
        {
            if (OrgUnitID > 0)
            {
                OrgUnit lOrgUnit = db.OrgUnits.Where(c => c.OrgUnitID == OrgUnitID).FirstOrDefault();
                if (lOrgUnit != null)
                {
                    lOrgUnit.RecordStatus = 99;
                    lOrgUnit.ModifiedBy = ModifiedBy;
                    lOrgUnit.ModifiedDate = Server.DateNow();
                    db.SaveChanges();
                    R_Account.SaveAuditTrail("Company " + lOrgUnit.OrgUnitName + " has been removed.");
                }
            }
        }
        #endregion


        public IEnumerable<Country> GetCountries()
        {
            return db.Countries.ToList();
        }

        public IEnumerable<State> GetStates(int CountryID)
        {
            return db.States.Where(c=>c.CountryID==CountryID).ToList();
        }

    }



    public static class QueryableExtensions
    {
        public enum Order
        {
            Asc,
            Desc
        }

        public static IQueryable<T> OrderByDynamic<T>(
            this IQueryable<T> query,
            string orderByMember,
            Order direction)
        {
            var queryElementTypeParam = Expression.Parameter(typeof(T));

            var memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderByMember);

            var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

            var orderBy = Expression.Call(
                typeof(Queryable),
                direction == Order.Asc ? "OrderBy" : "OrderByDescending",
                new Type[] { typeof(T), memberAccess.Type },
                query.Expression,
                Expression.Quote(keySelector));

            return query.Provider.CreateQuery<T>(orderBy);
        }
    }

}
