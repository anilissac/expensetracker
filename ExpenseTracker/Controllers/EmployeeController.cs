using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
