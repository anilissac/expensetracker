using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class Functionality
{
    public int FunctionalityID { get; set; }

    public string FunctionalityName { get; set; }

    public virtual ICollection<SecurityRoleFunctionality> SecurityRoleFunctionalities { get; set; } = new List<SecurityRoleFunctionality>();
}
