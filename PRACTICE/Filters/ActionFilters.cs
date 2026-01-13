using Microsoft.AspNetCore.Mvc.Filters;

namespace PRACTICE.Filters
{
    public class ActionFilters : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            
            Console.WriteLine("OnActionExecuting");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("OnActionExecuted");
        }

    }
}
