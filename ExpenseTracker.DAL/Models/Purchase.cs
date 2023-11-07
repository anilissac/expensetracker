using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class Purchase
{
    public long PurchaseID { get; set; }

    public DateTime PurchasedOn { get; set; }

    public int? VendorID { get; set; }

    public string Description { get; set; }

    public decimal TotalAmount { get; set; }

    public string PurchasedBy { get; set; }

    public byte? PaymentMode { get; set; }

    public string PaymentRemark { get; set; }

    public int OrgUnitID { get; set; }

    public int? RecordStatus { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Vendor Vendor { get; set; }
}
