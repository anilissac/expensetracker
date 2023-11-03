using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Hosting;
namespace ExpenseTracker
{
    public static class Utilities
    {

        /// <summary>
        /// This method allows us to define multiple actions and controllers — in any other combination — that add the defined class to our selected element.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="controllers"></param>
        /// <param name="actions"></param>
        /// <param name="cssClass"></param>
        /// <returns></returns>
        public static string ActiveClass(this IHtmlHelper htmlHelper, string controllers = null, string actions = null, string cssClass = "active")
        {
            var currentController = htmlHelper?.ViewContext.RouteData.Values["controller"] as string;
            var currentAction = htmlHelper?.ViewContext.RouteData.Values["action"] as string;

            var acceptedControllers = (controllers ?? currentController ?? "").Split(',');
            var acceptedActions = (actions ?? currentAction ?? "").Split(',');

            return acceptedControllers.Contains(currentController) && acceptedActions.Contains(currentAction)
                ? cssClass
                : "";
        }
        public static string GetValidFileName(string fileName)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName.Trim();
        }
        internal static bool CreateFolderIfNeeded(string sDirectory)
        {
            throw new NotImplementedException();
        }
    }
    public static class ServerConfiguration
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static HttpContext Current => _httpContextAccessor.HttpContext;


        public static void ConfigureHttpContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

    }
    public static class SessionExtension
    {
        /// <summary>
        /// https://www.webtrainingroom.com/aspnetcore/session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetInt64(this ISession session, string key, long value)
        {
            var bytes = new byte[]
            {
            (byte)(value >> 56),
            (byte)(0xFF & (value >> 48)),
            (byte)(0xFF & (value >> 40)),
            (byte)(0xFF & (value >> 32)),
            (byte)(0xFF & (value >> 24)),
            (byte)(0xFF & (value >> 16)),
            (byte)(0xFF & (value >> 8)),
            (byte)(0xFF & value)
            };
            session.Set(key, bytes);
        }

        public static long? GetInt64(this ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null || data.Length < 8)
            {
                return null;
            }

            return (long)data[0] << 56 | (long)data[1] << 48 | (long)data[2] << 40 | (long)data[3] << 32 | (long)data[4] << 24 | (long)data[5] << 16 | (long)data[6] << 8 | (long)data[7];
        }

        public static void SetBoolean(this ISession session, string key, bool value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }

        public static bool? GetBoolean(this ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null)
            {
                return null;
            }
            return BitConverter.ToBoolean(data, 0);
        }
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
