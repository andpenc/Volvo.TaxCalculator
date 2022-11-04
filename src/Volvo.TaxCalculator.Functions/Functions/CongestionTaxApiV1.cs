using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Volvo.TaxCalculator.Functions.Constants;
using Volvo.TaxCalculator.DTO.CongestionTax;
using Volvo.TaxCalculator.Core.Interfaces;
using System.Net;
using AzureFunctions.Extensions.Swashbuckle.Attribute;

namespace Volvo.TaxCalculator.Functions.Functions
{
    public class CongestionTaxApiV1
    {
        private readonly ICongestionTaxService _congestionTaxService;

        public CongestionTaxApiV1(ICongestionTaxService congestionTaxService)
        {
            _congestionTaxService = congestionTaxService;
        }

        [FunctionName(nameof(CalculateCongestionTax))]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [RequestHttpHeader("x-ms-session-id", true)]        
        public async Task<IActionResult> CalculateCongestionTax(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = Routes.CONGESTION_TAX)]
            [RequestBodyType(typeof(CongestionTaxRequest), "Congestion tax request")]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var requestData = JsonConvert.DeserializeObject<CongestionTaxRequest>(requestBody);

            var taxAmount = _congestionTaxService.GetTax(requestData.Dates, requestData.Vehicle);

            return new OkObjectResult(taxAmount);
        }
    }
}
