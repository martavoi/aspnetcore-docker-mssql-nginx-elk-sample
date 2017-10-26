using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Oxagile.Internal.Api.Filters.Exception
{
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is System.Exception)
            {
                context.Result = new ObjectResult(new
                    {
                        Result = "error",
                        Message = "internal server error"
                    })
                    {
                        StatusCode = 500
                    };
            }
        }
    }
}