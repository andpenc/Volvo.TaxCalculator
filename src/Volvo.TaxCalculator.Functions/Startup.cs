

using System.Collections;
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
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<ICongestionTaxService, CongestionTaxService>();
            builder.Services.AddScoped<CongestionTaxApiV1>();
        }
    }
}
