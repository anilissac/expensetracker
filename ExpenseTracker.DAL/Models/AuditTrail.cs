using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class AuditTrail
{
    public long AuditID { get; set; }

    public string AuditText { get; set; }

    public int? OrgUnitID { get; set; }

    public int? CustomerID { get; set; }

    public int? VendorID { get; set; }

    public string IPAddress { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual AppUser CreatedByNavigation { get; set; }

    public virtual OrgUnit OrgUnit { get; set; }

    public virtual Vendor Vendor { get; set; }
}
