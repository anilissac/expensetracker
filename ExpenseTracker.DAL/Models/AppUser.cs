using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class AppUser
{
    public int UserID { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string ContactNumber { get; set; }

    public string ContactEmail { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public DateTime? PasswordResetOn { get; set; }

    public DateTime? LastAccessedOn { get; set; }

    public int? DefaultOrgUnitID { get; set; }

    public int? DefaultSecurityRoleID { get; set; }

    public string ProfilePictureURL { get; set; }

    public int? RecordStatus { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<AuditTrail> AuditTrails { get; set; } = new List<AuditTrail>();

    public virtual OrgUnit DefaultOrgUnit { get; set; }

    public virtual SecurityRole DefaultSecurityRole { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
