using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class SecurityRole
{
    public int SecurityRoleID { get; set; }

    public string SecurityRoleName { get; set; }

    public string Description { get; set; }

    public int? RecordStatus { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<AppUserPermission> AppUserPermissions { get; set; } = new List<AppUserPermission>();

    public virtual ICollection<AppUser> AppUsers { get; set; } = new List<AppUser>();

    public virtual ICollection<SecurityRoleFunctionality> SecurityRoleFunctionalities { get; set; } = new List<SecurityRoleFunctionality>();
}
