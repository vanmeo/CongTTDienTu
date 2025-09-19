using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Xim.AppApi.Models;
using Xim.Library.Exceptions;

namespace Xim.AppApi.ActionFilters
{
    public class ExceptionActionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 1;
        readonly IServiceProvider _serviceProvider;
        
        public ExceptionActionFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                return;
            }

            if (context.Exception is BusinessException businessException)
            {
                context.Result = new ObjectResult(businessException)
                {
                    StatusCode = 444,
                    Value = businessException.ToString()
                };
                context.ExceptionHandled = true;
                return;
            }

            var logger = _serviceProvider.GetService<ILogger<ExceptionActionFilter>>();
            logger.LogError(context.Exception, context.Exception.Message);

            var appConfig = _serviceProvider.GetService<AppConfig>();
            var msg = appConfig?.Debug == true ? context.Exception.ToString() : "Exception";
            
            context.Result = new ObjectResult(context.Exception)
            {
                StatusCode = 500,
                Value = msg
            };
            context.ExceptionHandled = true;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //nothing
        }
    }
}
