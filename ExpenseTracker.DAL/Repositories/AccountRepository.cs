using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ExpenseTracker.DAL.Models;
using ExpenseTracker.DAL.Configurations;
using ExpenseTracker.DAL.Utilities;
namespace ExpenseTracker.DAL.Repositories
{
    public class AccountRepository
    {
        //Declarations
        private ETDBContext db = new ETDBContext();
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }

        #region Login
        public AppUserView ValidateUser(string Username, string Password)
        {
            string enPassword = Security.Encrypt(Password.Trim());
            var _data = (from x in db.AppUsers
                         join s in db.SecurityRoles on x.DefaultSecurityRoleID equals s.SecurityRoleID into kxs
                         from xs in kxs.DefaultIfEmpty()
                         join o in db.OrgUnits on x.DefaultOrgUnitID equals o.OrgUnitID into kxo
                         from xo in kxo.DefaultIfEmpty()
                         select new AppUserView
                         {
                             UserID = x.UserID,
                             FirstName = x.FirstName,
                             LastName = x.LastName,
                             ContactEmail = x.ContactEmail,
                             ContactNumber = x.ContactNumber,
                             Username = x.Username,
                             Password = x.Password,
                             DefaultSecurityRoleID = x.DefaultSecurityRoleID,
                             DefaultSecurityRoleName = xs.SecurityRoleName,
                             DefaultOrgUnitID = x.DefaultOrgUnitID,
                             DefaultOrgUnitName = xo.OrgUnitName,
                             ProfilePictureURL=x.ProfilePictureURL,
                             RecordStatus = x.RecordStatus,
                         }).Where(x => x.RecordStatus == 0 && x.Username.Trim() == Username.Trim() && x.Password.Trim() == enPassword.Trim());

            return _data.FirstOrDefault();

        }
        public AppUserPermissionView GetAppUserPermission(int UserID, int OrgUnitID)
        {
            var _data = (from x in db.AppUserPermissions
                         join s in db.SecurityRoles on x.SecurityRoleID equals s.SecurityRoleID into kxs
                         from xs in kxs.DefaultIfEmpty()
                         join o in db.OrgUnits on x.OrgUnitID equals o.OrgUnitID into kxo
                         from xo in kxo.DefaultIfEmpty()
                         select new AppUserPermissionView
                         {
                            UserID = x.UserID,
                            SecurityRoleID = x.SecurityRoleID,
                            SecurityRoleName = xs.SecurityRoleName,
                            OrgUnitID = x.OrgUnitID,
                            OrgUnitName = xo.OrgUnitName,
                         }).Where(c => c.UserID == UserID && c.OrgUnitID == OrgUnitID).FirstOrDefault();

            return _data;

        }
        public void SignIN(int? UserID)
        {
            if (UserID > 0)
            {
                AppUser lAppUser = db.AppUsers.Where(c => c.UserID == UserID).FirstOrDefault();
                if (lAppUser != null)
                {
                    lAppUser.LastAccessedOn = Server.DateNow();
                    db.SaveChanges();
                    SaveAuditTrail(lAppUser.FirstName + " " + lAppUser.LastName + " has been logged in to web portal.");
                }
            }
        }
        public void SignOUT(int? UserID)
        {
            if (UserID > 0)
            {
                AppUser lAppUser = db.AppUsers.Where(c => c.UserID == UserID).FirstOrDefault();
                if (lAppUser != null)
                {
                    SaveAuditTrail(lAppUser.FirstName + " " + lAppUser.LastName + " has been logged out from web portal.");
                }
            }
        }
        #endregion

        #region AppUsers

        public IEnumerable<AppUserView> GetAppUsers()
        {
            var _data = (from x in db.AppUsers
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         join s in db.SecurityRoles on x.DefaultSecurityRoleID equals s.SecurityRoleID into kxs
                         from xs in kxs.DefaultIfEmpty()
                         join o in db.OrgUnits on x.DefaultOrgUnitID equals o.OrgUnitID into kxo
                         from xo in kxo.DefaultIfEmpty()
                         select new AppUserView
                         {
                             enUserID = Security.Encrypt(x.UserID),
                             UserID = x.UserID,
                             FirstName = x.FirstName,
                             LastName = x.LastName,
                             ContactEmail = x.ContactEmail,
                             ContactNumber = x.ContactNumber,
                             Username = x.Username,
                             Password = x.Password,
                             PasswordResetOn = x.PasswordResetOn,
                             DefaultSecurityRoleID = x.DefaultSecurityRoleID,
                             DefaultSecurityRoleName = xs.SecurityRoleName,
                             DefaultOrgUnitID = x.DefaultOrgUnitID,
                             DefaultOrgUnitName = xo.OrgUnitName,
                             ProfilePictureURL = x.ProfilePictureURL,
                             LastAccessedOn = x.LastAccessedOn,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(x => x.RecordStatus != 99);
            return _data.ToList();
        }
        public AppUserView GetAppUser(int UserID)
        {
            var _data = (from x in db.AppUsers
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         join s in db.SecurityRoles on x.DefaultSecurityRoleID equals s.SecurityRoleID into kxs
                         from xs in kxs.DefaultIfEmpty()
                         join o in db.OrgUnits on x.DefaultOrgUnitID equals o.OrgUnitID into kxo
                         from xo in kxo.DefaultIfEmpty()
                         select new AppUserView
                         {
                             enUserID = Security.Encrypt(x.UserID),
                             UserID = x.UserID,
                             FirstName = x.FirstName,
                             LastName = x.LastName,
                             ContactEmail = x.ContactEmail,
                             ContactNumber = x.ContactNumber,
                             Username = x.Username,
                             Password = x.Password,
                             PasswordResetOn = x.PasswordResetOn,
                             DefaultSecurityRoleID = x.DefaultSecurityRoleID,
                             DefaultSecurityRoleName = xs.SecurityRoleName,
                             DefaultOrgUnitID = x.DefaultOrgUnitID,
                             DefaultOrgUnitName = xo.OrgUnitName,
                             ProfilePictureURL=x.ProfilePictureURL,
                             LastAccessedOn = x.LastAccessedOn,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(x => x.UserID == UserID).FirstOrDefault();
            return _data;
        }
        
        public AppUser SaveAppUser(AppUserView oAppUserView)
        {
            AppUser lAppUser = new AppUser();
            if (oAppUserView.UserID > 0)
            {//Edit
                lAppUser = db.AppUsers.Where(c => c.UserID == oAppUserView.UserID).FirstOrDefault();
            }
            else
            {
                lAppUser.Password = Security.Encrypt("Default");
            }
            lAppUser.FirstName = oAppUserView.FirstName.ToUpper();
            lAppUser.LastName = oAppUserView.LastName.ToUpper();
            lAppUser.ContactEmail = oAppUserView.ContactEmail;
            lAppUser.ContactNumber = oAppUserView.ContactNumber;
            lAppUser.Username= oAppUserView.Username;
          
            lAppUser.DefaultOrgUnitID = oAppUserView.DefaultOrgUnitID;
            lAppUser.DefaultSecurityRoleID = oAppUserView.DefaultSecurityRoleID;
            lAppUser.RecordStatus = 0;
            if (oAppUserView.UserID == 0) { 
                lAppUser.CreatedBy = oAppUserView.CreatedBy;
                lAppUser.CreatedDate = Server.DateNow();
                db.AppUsers.Add(lAppUser);
                db.SaveChanges();
                AppUserPermission lAppUserPermission = new AppUserPermission();
                lAppUserPermission.UserID = lAppUser.UserID;
                lAppUserPermission.OrgUnitID = (int)lAppUser.DefaultOrgUnitID;
                lAppUserPermission.SecurityRoleID = (int)lAppUser.DefaultSecurityRoleID;
                lAppUserPermission.CreatedBy = lAppUser.CreatedBy;
                lAppUserPermission.CreatedDate = Server.DateNow();
                db.AppUserPermissions.Add(lAppUserPermission);

                SaveAuditTrail("New user account is registered for " + oAppUserView.FirstName + " " + oAppUserView.LastName + ".");
            }
            else
            {
                lAppUser.ModifiedDate = Server.DateNow();
                lAppUser.ModifiedBy = oAppUserView.CreatedBy;

                AppUserPermission lAppUserPermission = db.AppUserPermissions.Where(c => c.UserID == lAppUser.UserID && c.OrgUnitID == lAppUser.DefaultOrgUnitID).FirstOrDefault();
                if (lAppUserPermission == null)
                {
                    lAppUserPermission = new AppUserPermission();
                    lAppUserPermission.UserID = lAppUser.UserID;
                    lAppUserPermission.OrgUnitID = (int)lAppUser.DefaultOrgUnitID;
                    lAppUserPermission.SecurityRoleID = (int)lAppUser.DefaultSecurityRoleID;
                    lAppUserPermission.CreatedBy = (int)lAppUser.ModifiedBy;
                    lAppUserPermission.CreatedDate = Server.DateNow();
                    db.AppUserPermissions.Add(lAppUserPermission);
                }

               SaveAuditTrail("User account for " + oAppUserView.FirstName + " " + oAppUserView.LastName + " has been modified.");
            }
           

            db.SaveChanges();
            return lAppUser;
        }

        public AppUser ChangePassword(AppUserView oAppUserView)
        {
            AppUser lAppUser = db.AppUsers.Where(c => c.UserID == oAppUserView.UserID).FirstOrDefault();
            if (lAppUser != null)
            {
                lAppUser.Password = Security.Encrypt(oAppUserView.Password.Trim());
                lAppUser.PasswordResetOn= Server.DateNow();
                lAppUser.ModifiedBy = oAppUserView.ModifiedBy;
                lAppUser.ModifiedDate = Server.DateNow();
                db.SaveChanges();
                SaveAuditTrail("User  " + lAppUser.FirstName + " " + lAppUser.LastName + " has changed password.");
            }
            return lAppUser;
        }

        public void UpdateAppUserProfilePath(int UserID, string FilePath, int? ModifiedBy)
        {
            if (UserID > 0)
            {
                AppUser lAppUser = db.AppUsers.Where(c => c.UserID == UserID).FirstOrDefault();
                if (lAppUser != null)
                {
                    lAppUser.ProfilePictureURL =".."+ FilePath;
                    lAppUser.ModifiedBy = ModifiedBy;
                    lAppUser.ModifiedDate = Server.DateNow();
                    db.SaveChanges();
                    SaveAuditTrail("Profile picture modified for " + lAppUser.FirstName + ".");
                }
            }
        }
        public void RemoveAppUser(int UserID, int? ModifiedBy)
        {
            if (UserID > 0)
            {
                AppUser lAppUser = db.AppUsers.Where(c => c.UserID == UserID).FirstOrDefault();
                if (lAppUser != null)
                {
                    lAppUser.RecordStatus = 99;
                    lAppUser.ModifiedBy = ModifiedBy;
                    lAppUser.ModifiedDate = Server.DateNow();
                    db.SaveChanges();
                    SaveAuditTrail("Application User " + lAppUser.FirstName  + " has been removed.");
                }
            }
        }
      
        #endregion

        #region Security Roles
        public IEnumerable<SecurityRoleView> GetSecurityRoles()
        {
            var _data = (from x in db.SecurityRoles
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         select new SecurityRoleView
                         {
                             enSecurityRoleID = Security.Encrypt(x.SecurityRoleID),
                             SecurityRoleID = x.SecurityRoleID,
                             SecurityRoleName = x.SecurityRoleName,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(c=>c.SecurityRoleID!=1).ToList(); //not showing super admin

            return _data;
        }
        public SecurityRoleView GetSecurityRole(int SecurityRoleID)
        {

            var _data = (from x in db.SecurityRoles
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         select new SecurityRoleView
                         {
                             enSecurityRoleID = Security.Encrypt(x.SecurityRoleID),
                             SecurityRoleID = x.SecurityRoleID,
                             SecurityRoleName = x.SecurityRoleName,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(c => c.SecurityRoleID == SecurityRoleID).FirstOrDefault();

            return _data;

        }

        public SecurityRole SaveSecurityRole(SecurityRoleView oSecurityRole)
        {
            SecurityRole lSecurityRole = new SecurityRole();
            bool bEditMode = false;
            if (oSecurityRole.SecurityRoleID > 0)
            {
                lSecurityRole = db.SecurityRoles.Where(c => c.SecurityRoleID == oSecurityRole.SecurityRoleID).FirstOrDefault();
                bEditMode = true;
            }
            lSecurityRole.SecurityRoleName = oSecurityRole.SecurityRoleName;

            lSecurityRole.RecordStatus = 0;
            if (bEditMode)
            {
                lSecurityRole.ModifiedBy = oSecurityRole.CreatedBy;
                lSecurityRole.ModifiedDate = Server.DateNow();
            }
            else
            {
                lSecurityRole.CreatedBy = oSecurityRole.CreatedBy;
                lSecurityRole.CreatedDate = Server.DateNow();
                db.SecurityRoles.Add(lSecurityRole);
            }
            db.SaveChanges();
            if (bEditMode)
                SaveAuditTrail("New Security Role " + oSecurityRole.SecurityRoleName + " has been registered.");
            else
                SaveAuditTrail("Security Role " + oSecurityRole.SecurityRoleName + " has been modified.");
            return lSecurityRole;
        }
        public void RemoveSecurityRole(int SecurityRoleID, int? ModifiedBy)
        {
            if (SecurityRoleID > 0)
            {
                SecurityRole lSecurityRole = db.SecurityRoles.Where(c => c.SecurityRoleID == SecurityRoleID).FirstOrDefault();
                if (lSecurityRole != null)
                {
                    lSecurityRole.RecordStatus = 99;
                    lSecurityRole.ModifiedBy = ModifiedBy;
                    lSecurityRole.ModifiedDate = Server.DateNow();
                    db.SaveChanges();
                    SaveAuditTrail("Security role " + lSecurityRole.SecurityRoleName + " has been removed.");
                }
            }
        }
        #endregion

        #region AuditTrail

        /// <summary>
        /// Get activity logs of selected user for selected period.
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public IEnumerable<AuditTrailView> GetAuditTrails(DateTime StartDate, DateTime EndDate, int? UserID = 0)
        {
            EndDate = EndDate.AddDays(1).AddTicks(-1);
            var _data = (from x in db.AuditTrails
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         select new AuditTrailView
                         {
                             AuditID = x.AuditID,
                             AuditText = x.AuditText,
                             CreatedBy = x.CreatedBy,
                             IPAddress = x.IPAddress,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             CreatedDate = x.CreatedDate,


                         }).Where(x => x.CreatedDate >= StartDate && x.CreatedDate <= EndDate & x.CreatedBy == UserID).OrderByDescending(e => e.CreatedDate);
            return _data.ToList();
        }
        public IEnumerable<AuditTrailView> GetAuditTrails(DateTime StartDate, DateTime EndDate)
        {
            EndDate = EndDate.AddDays(1).AddTicks(-1);
            var _data = (from x in db.AuditTrails
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         select new AuditTrailView
                         {
                             AuditID = x.AuditID,
                             AuditText = x.AuditText,
                             CreatedBy = x.CreatedBy,
                             IPAddress = x.IPAddress,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             CreatedDate = x.CreatedDate,


                         }).Where(x => x.CreatedDate >= StartDate && x.CreatedDate <= EndDate).OrderByDescending(e => e.CreatedDate);
            return _data.ToList();
        }

        /// <summary>
        /// Save Activity logs to the database.
        /// </summary>
        /// <param name="AuditText"></param>
        /// <param name="ContactID"></param>
        /// <param name="AppUserID"></param>
        /// <param name="RecordStatus"></param>
        /// <returns></returns>
        public AuditTrail SaveAuditTrail(string AuditText)
        {
            AuditTrail lAuditTrail = new AuditTrail();
            string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            lAuditTrail.AuditText = AuditText;
            lAuditTrail.CreatedBy = (int)_httpContextAccessor.HttpContext.Session.GetInt32("CurrentUserID");
            lAuditTrail.OrgUnitID = (int)_httpContextAccessor.HttpContext.Session.GetInt32("CurrentOrgUnitID");
            lAuditTrail.IPAddress = ipAddress;
            lAuditTrail.CreatedDate = Server.DateNow();
            db.AuditTrails.Add(lAuditTrail);
            db.SaveChanges();
            return lAuditTrail;
        }
        public AuditTrail SaveVendorAuditTrail(string AuditText, int VendorID)
        {
            AuditTrail lAuditTrail = new AuditTrail();
            string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            lAuditTrail.AuditText = AuditText;
            lAuditTrail.CreatedBy = (int)_httpContextAccessor.HttpContext.Session.GetInt32("CurrentUserID");
            lAuditTrail.OrgUnitID = (int)_httpContextAccessor.HttpContext.Session.GetInt32("CurrentOrgUnitID");
            lAuditTrail.IPAddress = ipAddress;
            lAuditTrail.VendorID = VendorID;
            lAuditTrail.CreatedDate = Server.DateNow();
            db.AuditTrails.Add(lAuditTrail);
            db.SaveChanges();
            return lAuditTrail;
        }
        public AuditTrail SaveCustomerAuditTrail(string AuditText, int CustomerID)
        {
            AuditTrail lAuditTrail = new AuditTrail();
            string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            lAuditTrail.AuditText = AuditText;
            lAuditTrail.CreatedBy = (int)_httpContextAccessor.HttpContext.Session.GetInt32("CurrentUserID");
            lAuditTrail.OrgUnitID = (int)_httpContextAccessor.HttpContext.Session.GetInt32("CurrentOrgUnitID");
            lAuditTrail.IPAddress = ipAddress;
            lAuditTrail.CustomerID = CustomerID;
            lAuditTrail.CreatedDate = Server.DateNow();
            db.AuditTrails.Add(lAuditTrail);
            db.SaveChanges();
            return lAuditTrail;
        }
        #endregion

    }
}
