using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Controllers
{
    public class ErrorController : Controller
    {
        readonly ILogger _logger;
        private IHttpContextAccessor _httpContextAccessor;
        public ErrorController(ILogger<ErrorController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        //[HttpGet("/Error")]
        public IActionResult Index()
        {

            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            //SettingsRepository R_Settings = new SettingsRepository(_httpContextAccessor);

           // R_Settings.NotifyDeveloper(exception.Error);
            return View(exception);
        }

        //[HttpGet("/Error/NotFound/{statusCode}")]
        public IActionResult NotFound(int statusCode)
        {
            var exception = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            _logger.LogError($"PAGE NOT FOUND: {exception.OriginalPath}");
            return View("NotFound", exception.OriginalPath);
        }

        public IActionResult AccessDenied(string BackURL)
        {
            //var exception = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            //_logger.LogError($"ACCESS DENIED: {exception.OriginalPath}");
            ViewBag.BackURL = BackURL;
            return View("AccessDenied");
        }
    }
}
