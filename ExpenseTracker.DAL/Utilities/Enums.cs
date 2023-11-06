using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DAL.Utilities
{
    public static class Enums
    {
        public enum AppSettings
        {
            Host = 1,
            Port = 2,
            UseDefaultCredentials = 3,
            EnableSsl = 4,
            SenderMail = 5,
            SenderUsername = 6,
            SenderPassword = 7,
            SenderSignature = 8,
            SenderName = 9,
            BCCMail = 10,
            MailFooter = 11,
            HostDomain = 12,
            LoginBlockTimeOut = 13,
            InvalidLoginAttempts = 14,
            AffiliationStartingYear = 21,
            AIBECOPDate = 22,
            DateFormat = 23,
            RazorPayKey = 24,
            RazorPaySecret = 25,
            RazorPayAccountID = 26,

            SMSAPI = 27,
            SMSAPIUsername = 28,
            SMSAPIPassword = 29,
            SMSSenderID = 30,
            TransactionSMSEnabled = 31,

            PaymentGatewayName = 40,
            BillDeskECOM2ReqHandler = 41,
            BillDeskCheckSumKey = 42,
            BillDeskPGIMerchantPayment = 43,
            BillDeskPGIMerchantID = 44,
            BillDeskPGIRequestID = 45,
            BillDeskPGISecurityID = 46,
            BillDeskPGIQueryAPI = 47,

            EnableOCR = 55,

            DeveloperMail = 100,
            ErrorReporting = 101,
            DatabaseMode = 102,
            AuthenticationToken = 103,
            SignatureType = 104,
            SignatureMode = 105

        }

        public enum OrderStatus
        {
			New = 0,
			Ordered = 1,
            Processing = 2,
            Completed = 3,
            Returned=4

        }
        public enum ConsignmentStatus
        {
            Prepared = 0,
            Delivered = 1,
            InUse = 2,
            Completed=3,
            Returned = 4,
        }
        
        public enum StoreAuditType
        {
            Purchase = 1,
            PurchaseReturns = 2,

            StockAdjustments=3,

			Consignments = 4,
			ConsignmentReturns = 5,

			Sales =6,
            SalesReturns=7,

            DeliveryNote=8
			

		}
		public enum DocumentFolder
		{
			PurchaseBills = 1,
			ConsignmentNotes = 2,
            DeliveryNotes=3,
            SalesInvoices=4,
            EmployeeRecords=5,
			ExpenseFiles = 6,
            SalesOrders=7,
            ConsignmentReturns=8,

		}
        public enum AccountType
        {
            Assets = 1,
            Capital = 2,
            Expenses =3,
            Income = 4,
            Liabilities = 5,
        }
        public enum TransactionType
        { 
            Debit=1,
            Credit=2
        
        }
        public enum InvoiceType
        {
            PurchaseOrder = 1,
            PurchaseVoucher = 2,
            PurchaseReturn =3,
            ConsignmentNote=4,
            DeliveryNote=5,
            Quotation=6,
            SalesInvoice=7,
			ConsignmentReturn = 8,
            ExpenseVoucher=9,
            DebitNote = 10,
            CreditNote = 11,
            PurchaseSettlement = 12,
            SalesReceipt = 13,
            StatementofAccount = 14,

        }
        public enum PaymentMode
        {
            Cash = 1,
            Cheque = 2,
            Online = 3,

            Credit = 9,
        }


    }
}
