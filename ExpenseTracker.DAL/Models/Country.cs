using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class Country
{
    public int CountryID { get; set; }

    public string CountryName { get; set; }

    public int? DefaultCurrencyID { get; set; }

    public virtual ICollection<OrgUnit> OrgUnits { get; set; } = new List<OrgUnit>();

    public virtual ICollection<State> States { get; set; } = new List<State>();

    public virtual ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();
}
