using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class PurchaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
