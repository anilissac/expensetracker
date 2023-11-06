using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
