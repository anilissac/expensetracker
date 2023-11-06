using ExpenseTracker.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ExpenseTracker.DAL.Utilities
{
    public static class FileHelper
    {
        //File Upload to Local System
        public static string UploadFile(IFormFile PostedFile, string FolderPath, string FileName)
        {
            ////<===== File Saving to Local System ====>
            //string fileName = "";
            string sFilePath = "";
            string sUploadFolder = "";
            string wwwRootPath = Directory.GetCurrentDirectory() + @"//wwwroot//";

          string  sAbsolutePath = Path.Combine(FolderPath, FileName);

            sUploadFolder = wwwRootPath + FolderPath;
           
            if (DAL.Utilities.Utilities.CreateFolderIfNeeded(sUploadFolder))
            {
                sFilePath = Path.Combine(sUploadFolder, FileName);
                using (var fileStream = new FileStream(sFilePath, FileMode.Create))
                {
                    PostedFile.CopyTo(fileStream);
                }
            }
            ////<===== End File Saving to Local System ====>

        

            return sAbsolutePath;
        }
    }
}
