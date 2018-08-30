using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Utils
{
    public class HasHeaderAttribute : Attribute, IActionFilter
    {
        private string _headers;
        
        public HasHeaderAttribute(string headers)
        {
            _headers = headers;
        }
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _headers.Split(',').ToList().ForEach(header =>
            {
                var headerValue = context.HttpContext.Request.Headers[header];
                if (string.IsNullOrEmpty(headerValue))
                    context.Result = new UnauthorizedResult();
            });
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}