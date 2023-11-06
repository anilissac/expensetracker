using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class Vendor
{
    public int VendorID { get; set; }

    public int? ParentVendorID { get; set; }

    public byte? VendorTypeID { get; set; }

    public string VendorCode { get; set; }

    public string VendorName { get; set; }

    public string ContactNumber { get; set; }

    public string ContactEmail { get; set; }

    public string ContactAddress { get; set; }

    public string ContactPostalCode { get; set; }

    public int? StateID { get; set; }

    public int? CountryID { get; set; }

    public string BillingName { get; set; }

    public string BillingAddress { get; set; }

    public string Website { get; set; }

    public int? DefaultOrgUnitID { get; set; }

    public int? DefaultCurrencyID { get; set; }

    public string TRN { get; set; }

    public string ProfilePictureURL { get; set; }

    public int? RecordStatus { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<AuditTrail> AuditTrails { get; set; } = new List<AuditTrail>();

    public virtual Country Country { get; set; }

    public virtual OrgUnit DefaultOrgUnit { get; set; }

    public virtual ICollection<Vendor> InverseParentVendor { get; set; } = new List<Vendor>();

    public virtual Vendor ParentVendor { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual State State { get; set; }
}
