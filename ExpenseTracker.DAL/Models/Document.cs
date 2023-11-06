using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class Document
{
    public long DocumentID { get; set; }

    public int? VendorID { get; set; }

    public long? SalesID { get; set; }

    public long? PurchaseID { get; set; }

    public int? EmployeeID { get; set; }

    public byte? DocumentFolderID { get; set; }

    public string DocumentName { get; set; }

    public string DocumentPath { get; set; }

    public int? RecordStatus { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
