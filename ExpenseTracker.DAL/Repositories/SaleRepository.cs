using ExpenseTracker.DAL.Configurations;
using ExpenseTracker.DAL.Models;
using ExpenseTracker.DAL.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ExpenseTracker.DAL.Utilities.Enums;

namespace ExpenseTracker.DAL.Repositories
{
    public class SaleRepository
    {
        //Declarations
        private ETDBContext db = new ETDBContext();
        private readonly IHttpContextAccessor _httpContextAccessor;

        private AccountRepository R_Account;
        public SaleRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            R_Account = new AccountRepository(httpContextAccessor);

        }
        #region Sales
        public IEnumerable<SaleView> GetSales()
        {
            var _data = (from x in db.Sales
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         select new SaleView
                         {
                             enSalesID = Security.Encrypt(x.SalesID),
                             SalesID = x.SalesID,
                             SalesOn = x.SalesOn,
                             Description = x.Description,
                             CashAmount = x.CashAmount,
                             BankAmount = x.BankAmount,
                             CreditAmount = x.CreditAmount,
                             TotalAmount = x.TotalAmount,
                             IsDayClosing = x.IsDayClosing,
                             OrgUnitID = x.OrgUnitID,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(c => c.RecordStatus != 99).ToList();

            return _data;
        }
        public IEnumerable<SaleView> GetTodaysSales()
        {
            var _data = (from x in db.Sales
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         select new SaleView
                         {
                             enSalesID = Security.Encrypt(x.SalesID),
                             SalesID = x.SalesID,
                             SalesOn = x.SalesOn,
                             Description = x.Description,
                             CashAmount = x.CashAmount,
                             BankAmount = x.BankAmount,
                             CreditAmount = x.CreditAmount,
                             TotalAmount = x.TotalAmount,
                             IsDayClosing = x.IsDayClosing,
                             OrgUnitID = x.OrgUnitID,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(c => c.RecordStatus !=99 && c.SalesOn.Date == Server.DateNow().Date ).ToList();

            return _data;
        }
        public SaleView GetSale(int SalesID)
        {

            var _data = (from x in db.Sales
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         select new SaleView
                         {
                             enSalesID = Security.Encrypt(x.SalesID),
                             SalesID = x.SalesID,
                             SalesOn = x.SalesOn,
                             Description = x.Description,
                             CashAmount = x.CashAmount,
                             BankAmount = x.BankAmount,
                             CreditAmount = x.CreditAmount,
                             TotalAmount = x.TotalAmount,
                             IsDayClosing = x.IsDayClosing,
                             OrgUnitID = x.OrgUnitID,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(c => c.SalesID == SalesID).FirstOrDefault();

            return _data;

        }

        public Sale SaveSale(SaleView oSale)
        {
            Sale lSale = new Sale();
            bool bEditMode = false;
            if (oSale.SalesID > 0)
            {
                lSale = db.Sales.Where(c => c.SalesID == oSale.SalesID).FirstOrDefault();
                bEditMode = true;
            }
            lSale.SalesOn = oSale.SalesOn;
            lSale.Description = oSale.Description;
            lSale.CashAmount = oSale.CashAmount;
            lSale.BankAmount = oSale.BankAmount;
            lSale.CreditAmount = oSale.CreditAmount;
            lSale.TotalAmount = oSale.TotalAmount;
            lSale.IsDayClosing = oSale.IsDayClosing;
            lSale.OrgUnitID = oSale.OrgUnitID;
            lSale.RecordStatus = 0;
            if (bEditMode)
            {
                lSale.ModifiedBy = oSale.CreatedBy;
                lSale.ModifiedDate = Server.DateNow();
            }
            else
            {
                lSale.CreatedBy = oSale.CreatedBy;
                lSale.CreatedDate = Server.DateNow();
                lSale.ModifiedBy = oSale.CreatedBy;
                lSale.ModifiedDate = Server.DateNow();
                db.Sales.Add(lSale);
            }
            db.SaveChanges();
            if (bEditMode)
                R_Account.SaveAuditTrail("New Sale has been recorded.");
            else
                R_Account.SaveAuditTrail("Sale has been modified.");
            return lSale;
        }
        public void RemoveSale(int SalesID, int? ModifiedBy)
        {
            if (SalesID > 0)
            {
                Sale lSale = db.Sales.Where(c => c.SalesID == SalesID).FirstOrDefault();
                if (lSale != null)
                {
                    lSale.RecordStatus = 99;
                    lSale.ModifiedBy = ModifiedBy;
                    lSale.ModifiedDate = Server.DateNow();
                    db.SaveChanges();
                    R_Account.SaveAuditTrail("Sale has been removed.");
                }
            }
        }
        #endregion
    }
}
