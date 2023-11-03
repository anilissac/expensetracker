using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace ExpenseTracker.Filters
{
    public class SessionTimeout : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {

            if (context.HttpContext.Session.Get("CurrentUserID") == null)
            {
                context.Result = new RedirectResult("~/Account/Index");
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //To do : after the action executes  
        }
    }
}
