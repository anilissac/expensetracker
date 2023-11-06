using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using ExpenseTracker.DAL.Models;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.DAL.Utilities
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
		{
			foreach (var parent in source)
			{
				yield return parent;

				var children = selector(parent);
				foreach (var child in SelectRecursive(children, selector))
					yield return child;
			}
		}
	}
	public static class Utilities
    {
        public static string GetDayNumberSuffix(string day)
        {
            string suffix = "th";
            if (int.Parse(day) < 11 || int.Parse(day) > 20)
            {
                day = day.ToCharArray()[^1].ToString();
                switch (day)
                {
                    case "1":
                        suffix = "st";
                        break;
                    case "2":
                        suffix = "nd";
                        break;
                    case "3":
                        suffix = "rd";
                        break;
                }
            }
            return day+ suffix;
        }
        public static string ReplaceMergeFields(string Template, List<KeyValuePair<string, string>> lstReplace)
        {
            if (!string.IsNullOrEmpty(Template))
            {
                foreach (var item in lstReplace)
                {
                    Template = Template.Replace(item.Key, item.Value);
                }
            }

            return Template;
        }

        public static string GetValidFileName(string fileName)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }
        /// <summary>
        /// Returns random string 
        /// </summary>
        /// <returns>Random String</returns>
        public static string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        /// <summary>
        /// Create Folder if not exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True if folder created</returns>
        public static bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }
        /// <summary>
        /// NumberToWords
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string NumberToWords(long number)
        {
            if (number == 0)
            {
                return "zero";
            }

            if (number < 0)
            {
                return "minus " + NumberToWords(Math.Abs(number));
            }

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                {
                    words += "and ";
                }

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                {
                    words += unitsMap[number];
                }
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                    {
                        words += "-" + unitsMap[number % 10];
                    }
                }
            }

            return words;
        }

        /// <summary>
        /// DecimalToWords
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string DecimalToWords(decimal number)
        {
            string word = string.Empty;
            var formatted = number.ToString();

            if (formatted.Contains("."))
            {
                string[] sides = formatted.Split('.');
                word = NumberToWords(Int64.Parse(sides[0])) + " and " + NumberToWords(Int64.Parse(sides[1]));
            }
            else
            {
                word = NumberToWords(Convert.ToInt64(number));
            }

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word.ToLower());
        }

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
        public static string TimeSinceString(this DateTime value)
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;

        TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - value.Ticks);
        double seconds = ts.TotalSeconds;

        // Less than one minute
        if (seconds < 1 * MINUTE)
            return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

        if (seconds < 60 * MINUTE)
            return ts.Minutes + " minutes ago";

        if (seconds < 120 * MINUTE)
            return "an hour ago";

        if (seconds < 24 * HOUR)
            return ts.Hours + " hours ago";

        if (seconds < 48 * HOUR)
            return "yesterday";

        if (seconds < 30 * DAY)
            return ts.Days + " days ago";

        if (seconds < 12 * MONTH)
        {
            int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
            return months <= 1 ? "one month ago" : months + " months ago";
        }

        int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
        return years <= 1 ? "one year ago" : years + " years ago";
    }

        public static int getMonths(DateTime dt1, DateTime dt2)
        {
            int totalMonths = 0;
            int tyear = dt1.Year - dt2.Year;
            int tmonth = dt1.Month - dt2.Month;
            int days = dt1.Day - dt2.Day;
            totalMonths = (tyear * 12) + tmonth + (days > 0 ? 1 : 0);
            return totalMonths;
        }

		public static string GetPaymentMode(int PaymentModeID)
		{
            string PaymentMode = "";
            switch(PaymentModeID){
                case 1:
                    PaymentMode = "Cash";
					break;
				case 2:
					PaymentMode = "Cheque";
					break;
				case 3:
					PaymentMode = "Online";
					break;
			}
			return PaymentMode;
		}

	}
    public static class Server
    {
        /// <summary>
        /// Returns system date time w.r.t UAE Time Zone. Independent to Hosting time 
        /// </summary>
        /// <returns>Indian Time Now</returns>
        public static DateTime DateNow()
        {
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo myZone = TimeZoneInfo.CreateCustomTimeZone("Dubai", new TimeSpan(+4, +00, 0), "Dubai", "Dubai");
            DateTime custDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, myZone);
            return custDateTime;
        }

        public static string MapPath(string fileName)
        {
            throw new NotImplementedException();
        }
    }


  




    
}
