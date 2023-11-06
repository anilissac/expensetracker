using System.IO;
using System;
using System.Drawing;
using System.Drawing.Imaging;

using ExpenseTracker.DAL.Utilities;
using ExpenseTracker.DAL.Models;
using Document = iText.Layout.Document;
using iText.Html2pdf;
using QRCoder;
using iText.Html2pdf.Attach.Impl;
using iText.Html2pdf.Css.Apply.Impl;
using iText.Html2pdf.Resolver.Font;
using iText.StyledXmlParser.Css.Media;

namespace ExpenseTracker.DAL.Utilities
{

    public static class PDFGenerator
    {
        public static string CreatePDF(string htmlcontent, string FileName,OrgUnit oOrgUnit, int MasterTemplateID=1, string PrintedBy="", string IPAddress="")
        {
            string FilePath = Directory.GetCurrentDirectory() + @"//wwwroot//templates//pdftemplate"+ MasterTemplateID .ToString()+ ".html";
            StreamReader str = new StreamReader(FilePath);
            string sHTMLBody = str.ReadToEnd();
            str.Close();

            sHTMLBody = sHTMLBody.Replace("[[CompanyName]]", oOrgUnit.OrgUnitName);
            sHTMLBody = sHTMLBody.Replace("[[CompanyAddressLine1]]", oOrgUnit.BillingAddressLine1);
            sHTMLBody = sHTMLBody.Replace("[[CompanyAddressLine2]]", oOrgUnit.BillingAddressLine2);
            sHTMLBody = sHTMLBody.Replace("[[CompanyAddressLine3]]", oOrgUnit.BillingAddressLine3);
            sHTMLBody = sHTMLBody.Replace("[[CompanyPhone]]", oOrgUnit.ContactNumber);
            sHTMLBody = sHTMLBody.Replace("[[CompanyEmail]]", oOrgUnit.ContactEmail);
            sHTMLBody = sHTMLBody.Replace("[[CompanyWebsite]]", oOrgUnit.Website);
            sHTMLBody = sHTMLBody.Replace("[[CompanyTRN]]", oOrgUnit.TRN);

            sHTMLBody = sHTMLBody.Replace("[[PDFBody]]", htmlcontent);

            sHTMLBody = sHTMLBody.Replace("[[PrintedBy]]", PrintedBy);
            sHTMLBody = sHTMLBody.Replace("[[PrintedOn]]", Server.DateNow().ToString("dd-MM-yyyy HH:mm"));
            sHTMLBody = sHTMLBody.Replace("[[IPAddress]]", IPAddress);

     //       ConverterProperties converterProperties = new ConverterProperties()
     //.SetBaseUri(".")
     //.SetCreateAcroForm(false)
     //.SetCssApplierFactory(new DefaultCssApplierFactory())
     //.SetFontProvider(new DefaultFontProvider())
     //.SetMediaDeviceDescription(MediaDeviceDescription.CreateDefault())
     //.SetOutlineHandler(new OutlineHandler())
     //.SetTagWorkerFactory(new DefaultTagWorkerFactory());

			// HtmlConverter.ConvertToPdf(sHTMLBody, new FileStream(FileName, FileMode.Create), converterProperties);
			HtmlConverter.ConvertToPdf(sHTMLBody, new FileStream(FileName, FileMode.Create));
			//   Wkhtmltopdf.NetCore.GeneratePdf 
			return FileName;
           

        }
        public static string GetQRCodeURL(string QRCodeText)
        {
            string QRCodeUri = "";

            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(QRCodeText, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode oQrCode = new BitmapByteQRCode(QrCodeInfo);
            byte[] BitmapArray = oQrCode.GetGraphic(60);
            QRCodeUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));

            return QRCodeUri;

        }

    }
    public static class BitmapExtension
    {
        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }

}
