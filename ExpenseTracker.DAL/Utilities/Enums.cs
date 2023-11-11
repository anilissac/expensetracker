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
        public enum EventType
        {
            Appointment=1,
            Task=2
        }
        public enum EventStatus
        {
            Active = 0,
            Started = 1,
            InProgress = 2,
            OnHold=3,
            Completed=4
                
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
