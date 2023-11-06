using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class State
{
    public int StateID { get; set; }

    public string StateName { get; set; }

    public int CountryID { get; set; }

    public virtual Country Country { get; set; }

    public virtual ICollection<OrgUnit> OrgUnits { get; set; } = new List<OrgUnit>();

    public virtual ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();
}
