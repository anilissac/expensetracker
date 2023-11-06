using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class AppUserPermission
{
    public int AppUserPermissionID { get; set; }

    public int UserID { get; set; }

    public int OrgUnitID { get; set; }

    public int SecurityRoleID { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual OrgUnit OrgUnit { get; set; }

    public virtual SecurityRole SecurityRole { get; set; }
}
