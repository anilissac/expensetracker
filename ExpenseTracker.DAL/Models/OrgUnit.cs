using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class OrgUnit
{
    public int OrgUnitID { get; set; }

    public string OrgUnitName { get; set; }

    public string ContactPerson { get; set; }

    public string ContactNumber { get; set; }

    public string ContactEmail { get; set; }

    public string ContactAddress { get; set; }

    public string ContactPostalCode { get; set; }

    public int? StateID { get; set; }

    public int? CountryID { get; set; }

    public string Website { get; set; }

    public string Description { get; set; }

    public int? DefaultCurrencyID { get; set; }

    public string TRN { get; set; }

    public string LogoURL { get; set; }

    public string AuthorisedSignatureURL { get; set; }

    public string AuthorisedStampSealURL { get; set; }

    public string BillingAddressLine1 { get; set; }

    public string BillingAddressLine2 { get; set; }

    public string BillingAddressLine3 { get; set; }

    public int? RecordStatus { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<AppUserPermission> AppUserPermissions { get; set; } = new List<AppUserPermission>();

    public virtual ICollection<AppUser> AppUsers { get; set; } = new List<AppUser>();

    public virtual ICollection<AuditTrail> AuditTrails { get; set; } = new List<AuditTrail>();

    public virtual Country Country { get; set; }

    public virtual State State { get; set; }

    public virtual ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();
}
