using ExpenseTracker.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DAL.Models
{
    public class OrgUnitView : OrgUnit
    {
        public string? enOrgUnitID { get; set; }
        public string? DefaultOrgUnitName { get; set; }
        public string? DefaultCurrencyName { get; set; }
        public string? StateName { get; set; }
        public string? CountryName { get; set; }
        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
    }
}
