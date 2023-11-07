using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class Sale
{
    public long SalesID { get; set; }

    public DateTime SalesOn { get; set; }

    public string Description { get; set; }

    public decimal CashAmount { get; set; }

    public decimal BankAmount { get; set; }

    public decimal CreditAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public bool IsDayClosing { get; set; }

    public int OrgUnitID { get; set; }

    public int? RecordStatus { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
