using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DAL.Models
{
    public class AppUserView : AppUser
    {
        public string ContactName {
            get {
                string _name = "";
                _name = FirstName;
                _name += " " + LastName;
                return _name;
            }
        }
        public string? enUserID { get; set; }
        public string? DefaultSecurityRoleName { get; set; }
        public string? DefaultOrgUnitName { get; set; }
        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
        public bool RememberMe { get; set; }
    }
    public class AppUserPermissionView : AppUserPermission
    {
        public string? SecurityRoleName { get; set; }
        public string? OrgUnitName { get; set; }
        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
    }
    public class SecurityRoleView : SecurityRole
    {
        public string? enSecurityRoleID { get; set; }
        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
    }
    public class SecurityRoleFunctionalityView : SecurityRoleFunctionality
    {
        public string? FunctionalityName { get; set; }
        public string? Description { get; set; }
        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
    }

    public class AuditTrailView : AuditTrail
    {
        public string? AppUserName { get; set; }
        public string? OrgUnitName { get; set; }
        public string? CreatedByName { get; set; }
    }


}
