using System.Reflection;
using AzureFunctions.Extensions.Swashbuckle;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Volvo.TaxCalculator.Core.Interfaces;
using Volvo.TaxCalculator.Functions;
using Volvo.TaxCalculator.Functions.Functions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Volvo.TaxCalculator.Functions
{
    public class Startup : FunctionsStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICongestionTaxService, CongestionTaxService>();
            services.AddScoped<CongestionTaxApiV1>();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureServices(builder.Services);

            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
        }
    }
}
