using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using ExpenseTracker.DAL.Models;
using ExpenseTracker.DAL.Repositories;
using System.Net.Mail;


using System.Net;
using System.Collections.Specialized;
using System.Web;

namespace ExpenseTracker.DAL.Utilities
{
    public static class Mailer
    {
              
        public static void SendMail(MailMessage mail, IEnumerable<AppSetting> oAppSettings, int MailTemplateID=1)
        {
            string FilePath = Directory.GetCurrentDirectory() + @"//wwwroot//templates//mailtemplate"+ MailTemplateID + ".html";
            StreamReader str = new StreamReader(FilePath);
            string sHTMLBody = str.ReadToEnd();
            str.Close();

            mail.Body = sHTMLBody.Replace("[[MailBody]]", mail.Body);



            mail.From = new MailAddress(oAppSettings.Where(c => c.AppSettingID == (int)Enums.AppSettings.SenderMail).FirstOrDefault().AppSettingValue, oAppSettings.Where(c => c.AppSettingID == (int)Enums.AppSettings.SenderName).FirstOrDefault().AppSettingValue);
            mail.IsBodyHtml = true;
            mail.Bcc.Add(oAppSettings.Where(c => c.AppSettingID == (int)Enums.AppSettings.BCCMail).FirstOrDefault().AppSettingValue);

            SmtpClient smtp = new SmtpClient();
            smtp.Host = oAppSettings.Where(c => c.AppSettingID == (int)Enums.AppSettings.Host).FirstOrDefault().AppSettingValue;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential(oAppSettings.Where(c => c.AppSettingID == (int)Enums.AppSettings.SenderUsername).FirstOrDefault().AppSettingValue, oAppSettings.Where(c => c.AppSettingID == (int)Enums.AppSettings.SenderPassword).FirstOrDefault().AppSettingValue);
            smtp.UseDefaultCredentials = false;
            smtp.Port = Convert.ToInt32(oAppSettings.Where(c => c.AppSettingID == (int)Enums.AppSettings.Port).FirstOrDefault().AppSettingValue);
            smtp.Send(mail);

        }

        

       
    }
}
