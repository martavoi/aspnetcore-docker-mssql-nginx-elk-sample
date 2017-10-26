using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Oxagile.Internal.Api.Formatters
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse(ContentType));

            SupportedEncodings.Add(Encoding.GetEncoding("utf-8"));
        }
        
        public string ContentType => "text/csv";

        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;

            Type type = context.Object.GetType();
            Type itemType;

            if (type.GetGenericArguments().Length > 0)
            {
                itemType = type.GetGenericArguments()[0];
            }
            else
            {
                itemType = type.GetElementType();
            }

            StringWriter stringWriter = new StringWriter();
            stringWriter.WriteLine(string.Join<string>(',', itemType
                    .GetProperties()
                    .Where(p => p.PropertyType.IsPrimitive || p.PropertyType == typeof(string) || p.PropertyType == typeof(DateTime))
                    .Select(x => x.Name)));

            foreach (var obj in (IEnumerable<object>)context.Object)
            {

                var vals = obj
                    .GetType()
                    .GetProperties()
                    .Where(p => p.PropertyType.IsPrimitive || p.PropertyType == typeof(string) || p.PropertyType == typeof(DateTime))
                    .Select(pi => new { Value = pi.GetValue(obj, null) });

                string valueLine = string.Empty;

                foreach (var val in vals)
                {
                    if (val.Value != null)
                    {
                        var valStr = val.Value.ToString();

                        //Check if the value contans a comma and place it in quotes if so
                        if (valStr.Contains(",")) 
                        { 
                            valStr = string.Concat("\"", valStr, "\""); 
                        }
                            
                        //Replace any \r or \n special characters from a new line with a space
                        if (valStr.Contains("\r"))
                        {
                            valStr = valStr.Replace("\r", " "); 
                        }

                        if (valStr.Contains("\n"))
                        { 
                            valStr = valStr.Replace("\n", " ");
                        }

                        valueLine = string.Concat(valueLine, valStr, ',');
                    }
                    else
                    {
                        valueLine = string.Concat(valueLine, string.Empty, ',');
                    }
                }

                stringWriter.WriteLine(valueLine.TrimEnd(','));
            }

            var streamWriter = new StreamWriter(response.Body);
            await streamWriter.WriteAsync(stringWriter.ToString());
            await streamWriter.FlushAsync();
        }

        protected override bool CanWriteType(Type type)
        {
            return type.GetInterface("IEnumerable") != null;
        }
    }
}