using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Oxagile.Demos.Api.Infrastructure.ActionResults;
using Serilog;

namespace Oxagile.Demos.Api.Infrastructure.Filters.Exception
{
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is System.Exception)
            {
                Log.Error(context.Exception, "Exception");
                context.Result = new InternalServerErrorObjectResult(new
                    {
                        Result = "error",
                        Message = "internal server error"
                    });
            }
        }
    }
}