using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    public class ChangeArgSyncAttribute : Attribute, IActionFilter, IAsyncActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.ContainsKey("message1"))
            {
                context.ActionArguments["message1"] = "New message";
            }
            ActionExecutedContext exc = await next();
            System.Console.WriteLine(exc);
        }
    }
}