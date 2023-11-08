using ExpenseTracker.DAL.Configurations;
using ExpenseTracker.DAL.Models;
using ExpenseTracker.DAL.Utilities;
using LinqKit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        #region Vendors
        public IEnumerable<VendorView> GetVendors()
        {
            var _data = (from x in db.Vendors
                         join a in db.Countries on x.CountryID equals a.CountryID into kxa
                         from xa in kxa.DefaultIfEmpty()
                         join b in db.States on x.StateID equals b.StateID into kxb
                         from xb in kxb.DefaultIfEmpty()
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         join o in db.OrgUnits on x.DefaultOrgUnitID equals o.OrgUnitID into kxo
                         from xo in kxo.DefaultIfEmpty()
                         select new VendorView
                         {
                             enVendorID = Security.Encrypt(x.VendorID),
                             VendorID = x.VendorID,
                             VendorCode = x.VendorCode,
                             VendorTypeID = x.VendorTypeID,
                             VendorName = x.VendorName,
                             ContactEmail = x.ContactEmail,
                             ContactNumber = x.ContactNumber,
                             ContactAddress = x.ContactAddress,
                             ContactPostalCode = x.ContactPostalCode,
                             StateID = x.StateID,
                             StateName = xb.StateName,
                             CountryID = x.CountryID,
                             CountryName = xa.CountryName,
                             Website = x.Website,
                             DefaultCurrencyID = x.DefaultCurrencyID,
                             DefaultOrgUnitID = x.DefaultOrgUnitID,
                             DefaultOrgUnitName = xo.OrgUnitName,
                             TRN = x.TRN,
                             ProfilePictureURL = x.ProfilePictureURL,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(x => x.RecordStatus != 99).OrderBy(c => c.VendorName);
            return _data.ToList();
        }
        public IEnumerable<VendorView> GetChildVendors(int VendorID)
        {
            var _data = (from x in db.Vendors
                         join a in db.Countries on x.CountryID equals a.CountryID into kxa
                         from xa in kxa.DefaultIfEmpty()
                         join b in db.States on x.StateID equals b.StateID into kxb
                         from xb in kxb.DefaultIfEmpty()
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         join o in db.OrgUnits on x.DefaultOrgUnitID equals o.OrgUnitID into kxo
                         from xo in kxo.DefaultIfEmpty()
                         select new VendorView
                         {
                             enVendorID = Security.Encrypt(x.VendorID),
                             VendorID = x.VendorID,
                             ParentVendorID = x.ParentVendorID,
                             VendorCode = x.VendorCode,
                             VendorTypeID = x.VendorTypeID,
                             VendorName = x.VendorName,
                             ContactEmail = x.ContactEmail,
                             ContactNumber = x.ContactNumber,
                             ContactAddress = x.ContactAddress,
                             ContactPostalCode = x.ContactPostalCode,
                             StateID = x.StateID,
                             StateName = xb.StateName,
                             CountryID = x.CountryID,
                             CountryName = xa.CountryName,
                             Website = x.Website,
                             DefaultCurrencyID = x.DefaultCurrencyID,
                             DefaultOrgUnitID = x.DefaultOrgUnitID,
                             DefaultOrgUnitName = xo.OrgUnitName,
                             TRN = x.TRN,
                             ProfilePictureURL = x.ProfilePictureURL,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(x => x.RecordStatus != 99 && x.ParentVendorID == VendorID).OrderBy(c => c.VendorName);
            return _data.ToList();
        }
        public IEnumerable<VendorView> SearchVendors(string SearchKey)
        {
            var whereClause = BuildVendorFilter(SearchKey);

            var _data = (from x in db.Vendors
                         select new VendorView
                         {
                             enVendorID = Security.Encrypt(x.VendorID),
                             VendorID = x.VendorID,
                             VendorName = x.VendorName,
                             ContactEmail = x.ContactEmail,
                             RecordStatus = x.RecordStatus,
                         }).Where(whereClause);
            return _data.ToList();
        }
        private Expression<Func<VendorView, bool>> BuildVendorFilter(string searchValue)
        {
            var predicate = PredicateBuilder.New<VendorView>(false); // true -where(true) return all
            if (String.IsNullOrWhiteSpace(searchValue) == false)
            {
                var searchTerms = searchValue.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //	predicate = predicate.And(x => x.RecordStatus == 0);
                foreach (var keyword in searchTerms)
                {
                    predicate = predicate.Or(x => x.VendorName.ToLower().Contains(keyword));

                }
            }
            return predicate;
        }
        public VendorView GetVendor(int VendorID)
        {
            var _data = (from x in db.Vendors
                         join a in db.Countries on x.CountryID equals a.CountryID into kxa
                         from xa in kxa.DefaultIfEmpty()
                         join b in db.States on x.StateID equals b.StateID into kxb
                         from xb in kxb.DefaultIfEmpty()
                         join p in db.AppUsers on x.CreatedBy equals p.UserID into kxp
                         from xp in kxp.DefaultIfEmpty()
                         join q in db.AppUsers on x.ModifiedBy equals q.UserID into kxq
                         from xq in kxq.DefaultIfEmpty()
                         join o in db.OrgUnits on x.DefaultOrgUnitID equals o.OrgUnitID into kxo
                         from xo in kxo.DefaultIfEmpty()
                         select new VendorView
                         {
                             enVendorID = Security.Encrypt(x.VendorID),
                             VendorID = x.VendorID,
                             VendorCode = x.VendorCode,
                             VendorTypeID = x.VendorTypeID,
                             VendorName = x.VendorName,
                             ContactEmail = x.ContactEmail,
                             ContactNumber = x.ContactNumber,
                             ContactAddress = x.ContactAddress,
                             ContactPostalCode = x.ContactPostalCode,
                             StateID = x.StateID,
                             StateName = xb.StateName,
                             CountryID = x.CountryID,
                             CountryName = xa.CountryName,
                             Website = x.Website,
                             DefaultCurrencyID = x.DefaultCurrencyID,
                             DefaultOrgUnitID = x.DefaultOrgUnitID,
                             DefaultOrgUnitName = xo.OrgUnitName,
                             TRN = x.TRN,
                             ProfilePictureURL = x.ProfilePictureURL,
                             RecordStatus = x.RecordStatus,
                             CreatedDate = x.CreatedDate,
                             CreatedByName = xp.FirstName + " " + xp.LastName,
                             ModifiedDate = x.ModifiedDate,
                             ModifiedByName = xq.FirstName + " " + xq.LastName,
                         }).Where(x => x.RecordStatus != 99 && x.VendorID == VendorID).FirstOrDefault();
            return _data;
        }

        public Vendor SaveVendor(VendorView oVendor)
        {
            Vendor lVendor = new Vendor();
            bool bEditMode = false;
            if (oVendor.VendorID > 0)
            {
                lVendor = db.Vendors.Where(c => c.VendorID == oVendor.VendorID).FirstOrDefault();
                bEditMode = true;
            }
            lVendor.ParentVendorID = oVendor.ParentVendorID;
            lVendor.VendorTypeID = oVendor.VendorTypeID;
            lVendor.VendorName = oVendor.VendorName;
            lVendor.ContactNumber = oVendor.ContactNumber;
            lVendor.ContactEmail = oVendor.ContactEmail;
            lVendor.ContactAddress = oVendor.ContactAddress;
            lVendor.ContactPostalCode = oVendor.ContactPostalCode;
            lVendor.StateID = oVendor.StateID;
            lVendor.CountryID = oVendor.CountryID;
            lVendor.Website = oVendor.Website;
            lVendor.DefaultOrgUnitID = oVendor.DefaultOrgUnitID;
            lVendor.DefaultCurrencyID = oVendor.DefaultCurrencyID;
            lVendor.TRN = oVendor.TRN;

            lVendor.RecordStatus = 0;
            if (bEditMode)
            {
                lVendor.ModifiedBy = oVendor.CreatedBy;
                lVendor.ModifiedDate = Server.DateNow();
            }
            else
            {
                lVendor.CreatedBy = oVendor.CreatedBy;
                lVendor.CreatedDate = Server.DateNow();
                db.Vendors.Add(lVendor);
            }
            db.SaveChanges();
            if (bEditMode)
                R_Account.SaveVendorAuditTrail("New Vendor " + oVendor.VendorName + " has been registered.", lVendor.VendorID);
            else
                R_Account.SaveVendorAuditTrail("Vendor " + oVendor.VendorName + " has been modified.", lVendor.VendorID);
            return lVendor;
        }

        public void RemoveVendor(int VendorID, int? ModifiedBy)
        {
            if (VendorID > 0)
            {
                Vendor lVendor = db.Vendors.Where(c => c.VendorID == VendorID).FirstOrDefault();
                if (lVendor != null)
                {
                    lVendor.RecordStatus = 99;
                    lVendor.ModifiedBy = ModifiedBy;
                    lVendor.ModifiedDate = Server.DateNow();
                    db.SaveChanges();
                    R_Account.SaveVendorAuditTrail("Vendor " + lVendor.VendorName + " has been removed.", lVendor.VendorID);
                }
            }
        }
        #endregion
        #region Purchases
        public IEnumerable<PurchaseView> GetPurchases()
        {
            var _data = (from x in db.Purchases
                         join v in db.Vendors on x.VendorID equals v.VendorID into kxv
                         from xv in kxv.DefaultIfEmpty()
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
                             VendorName = xv.VendorName,
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
        public IEnumerable<PurchaseView> GetTodaysPurchases()
        {
            var _data = (from x in db.Purchases
                         join v in db.Vendors on x.VendorID equals v.VendorID into kxv
                         from xv in kxv.DefaultIfEmpty()
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
                             VendorName = xv.VendorName,
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
                         }).Where(c => c.RecordStatus != 99 && c.PurchasedOn.Date == Server.DateNow().Date).ToList();

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
                lPurchase.ModifiedBy = oPurchase.CreatedBy;
                lPurchase.ModifiedDate = Server.DateNow();
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
