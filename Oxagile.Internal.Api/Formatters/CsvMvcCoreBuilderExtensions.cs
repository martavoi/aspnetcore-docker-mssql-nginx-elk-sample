using Microsoft.Extensions.DependencyInjection;

namespace Oxagile.Internal.Api.Formatters
{
    public static class CsvMvcCoreBuilderExtensions
    {
        public static IMvcCoreBuilder AddCsvSerializerFormatters(this IMvcCoreBuilder builder)
        {
            builder.AddMvcOptions(_ => 
            {
                _.OutputFormatters.Add(new CsvOutputFormatter());
            });
            return builder;
        }
    }
}