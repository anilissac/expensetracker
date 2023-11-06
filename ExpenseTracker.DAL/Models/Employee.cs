using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class Employee
{
    public int EmployeeID { get; set; }

    public int? UserID { get; set; }

    public string EmployeeCode { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Designation { get; set; }

    public DateTime? JoinDate { get; set; }

    public string ContactNumber { get; set; }

    public string ContactEmail { get; set; }

    public string ContactAddress { get; set; }

    public string ContactPostalCode { get; set; }

    public int? StateID { get; set; }

    public int? CountryID { get; set; }

    public string PermanentAddress { get; set; }

    public string PermanentPostalCode { get; set; }

    public int? PermanentStateID { get; set; }

    public int? PermanentCountryID { get; set; }

    public string PassportNumber { get; set; }

    public DateTime? PassportIssuedOn { get; set; }

    public DateTime? PassportExpiresOn { get; set; }

    public string IDNumber { get; set; }

    public DateTime? IDIssuedOn { get; set; }

    public DateTime? IDExpiresOn { get; set; }

    public string LabourCardNumber { get; set; }

    public DateTime? LabourCardIssuedOn { get; set; }

    public DateTime? LabourCardExpiresOn { get; set; }

    public string ProfilePictureURL { get; set; }

    public string BankName { get; set; }

    public string IBANNumber { get; set; }

    public int? OrgUnitID { get; set; }

    public int? RecordStatus { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual AppUser User { get; set; }
}
