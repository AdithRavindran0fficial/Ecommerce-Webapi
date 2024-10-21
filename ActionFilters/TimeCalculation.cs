using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Ecommerce_Webapi.ActionFilters
{
    public class TimeCalculationAttribute : ActionFilterAttribute
    {
        private Stopwatch _stopwatch;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();
           

        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();
            var elapsedTime = _stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"This method takes {elapsedTime} ms  to execute");
        }
    }
}
