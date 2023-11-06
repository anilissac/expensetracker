using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class SalesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
