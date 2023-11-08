using ExpenseTracker.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DAL.Models
{
    public class VendorView : Vendor
    {
        public string? enVendorID { get; set; }
        public string? ParentVendorName { get; set; }
        public string? DefaultOrgUnitName { get; set; }
        public string? DefaultCurrencyName { get; set; }
        public string? StateName { get; set; }
        public string? CountryName { get; set; }
        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
    }
    public class PurchaseView : Purchase
    {
        public string? enPurchaseID { get; set; }
        public string? VendorName { get; set; }
        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
    }
}
