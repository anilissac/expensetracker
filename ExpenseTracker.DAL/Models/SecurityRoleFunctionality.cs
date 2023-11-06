using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class SecurityRoleFunctionality
{
    public int SecurityRoleFunctionalityID { get; set; }

    public int SecurityRoleID { get; set; }

    public int FunctionalityID { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual Functionality Functionality { get; set; }

    public virtual SecurityRole SecurityRole { get; set; }
}
