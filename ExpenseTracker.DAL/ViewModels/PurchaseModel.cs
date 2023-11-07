using ExpenseTracker.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DAL.Models
{
    public class PurchaseView : Purchase
    {
        public string? enPurchaseID { get; set; }
        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
    }
}
