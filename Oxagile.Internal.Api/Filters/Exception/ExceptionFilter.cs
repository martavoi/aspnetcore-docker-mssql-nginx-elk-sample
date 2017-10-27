using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Oxagile.Internal.Api.ActionResults;

namespace Oxagile.Internal.Api.Filters.Exception
{
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is System.Exception)
            {
                context.Result = new InternalServerErrorObjectResult(new
                    {
                        Result = "error",
                        Message = "internal server error"
                    });
            }
        }
    }
}