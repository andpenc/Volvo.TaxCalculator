using System;
using Volvo.TaxCalculator.Core.Models;

namespace Volvo.TaxCalculator.DTO.CongestionTax
{
    public class CongestionTaxRequest
    {
        public DateTime[] Dates { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
