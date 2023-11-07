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
    public class PurchaseRepository
    {
        //Declarations
        private ETDBContext db = new ETDBContext();
        private readonly IHttpContextAccessor _httpContextAccessor;

        private AccountRepository R_Account;
        public PurchaseRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            R_Account = new AccountRepository(httpContextAccessor);

        }
        #region Purchases
        public IEnumerable<PurchaseView> GetPurchases()
        {
            var _data = (from x in db.Purchases
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         select new PurchaseView
                         {
                             enPurchaseID = Security.Encrypt(x.PurchaseID),
                             PurchaseID = x.PurchaseID,
                             PurchasedOn = x.PurchasedOn,
                             VendorID = x.VendorID,
                             Description = x.Description,
                             TotalAmount = x.TotalAmount,
                             PurchasedBy = x.PurchasedBy,
                             PaymentMode = x.PaymentMode,
                             PaymentRemark = x.PaymentRemark,
                             OrgUnitID = x.OrgUnitID,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(c => c.RecordStatus !=99).ToList(); 

            return _data;
        }
        public PurchaseView GetPurchase(int PurchaseID)
        {

            var _data = (from x in db.Purchases
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         select new PurchaseView
                         {
                             enPurchaseID = Security.Encrypt(x.PurchaseID),
                             PurchaseID = x.PurchaseID,
                             PurchasedOn = x.PurchasedOn,
                             VendorID = x.VendorID,
                             Description = x.Description,
                             TotalAmount = x.TotalAmount,
                             PurchasedBy = x.PurchasedBy,
                             PaymentMode = x.PaymentMode,
                             PaymentRemark = x.PaymentRemark,
                             OrgUnitID = x.OrgUnitID,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(c => c.PurchaseID == PurchaseID).FirstOrDefault();

            return _data;

        }

        public Purchase SavePurchase(PurchaseView oPurchase)
        {
            Purchase lPurchase = new Purchase();
            bool bEditMode = false;
            if (oPurchase.PurchaseID > 0)
            {
                lPurchase = db.Purchases.Where(c => c.PurchaseID == oPurchase.PurchaseID).FirstOrDefault();
                bEditMode = true;
            }
            lPurchase.PurchasedOn = oPurchase.PurchasedOn;
            lPurchase.VendorID = oPurchase.VendorID;
            lPurchase.Description = oPurchase.Description;
            lPurchase.TotalAmount = oPurchase.TotalAmount;
            lPurchase.PurchasedBy = oPurchase.PurchasedBy;
            lPurchase.PaymentMode = oPurchase.PaymentMode;
            lPurchase.PaymentRemark = oPurchase.PaymentRemark;
            lPurchase.OrgUnitID = oPurchase.OrgUnitID;
            lPurchase.RecordStatus = 0;
            if (bEditMode)
            {
                lPurchase.ModifiedBy = oPurchase.CreatedBy;
                lPurchase.ModifiedDate = Server.DateNow();
            }
            else
            {
                lPurchase.CreatedBy = oPurchase.CreatedBy;
                lPurchase.CreatedDate = Server.DateNow();
                db.Purchases.Add(lPurchase);
            }
            db.SaveChanges();
            if (bEditMode)
               R_Account.SaveAuditTrail("New Purchase has been recorded.");
            else
                R_Account.SaveAuditTrail("Purchase has been modified.");
            return lPurchase;
        }
        public void RemovePurchase(int PurchaseID, int? ModifiedBy)
        {
            if (PurchaseID > 0)
            {
                Purchase lPurchase = db.Purchases.Where(c => c.PurchaseID == PurchaseID).FirstOrDefault();
                if (lPurchase != null)
                {
                    lPurchase.RecordStatus = 99;
                    lPurchase.ModifiedBy = ModifiedBy;
                    lPurchase.ModifiedDate = Server.DateNow();
                    db.SaveChanges();
                    R_Account.SaveAuditTrail("Purchase has been removed.");
                }
            }
        }
        #endregion
    }
}
