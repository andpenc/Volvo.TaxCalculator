using System;
using Volvo.TaxCalculator.Core.Models;

namespace Volvo.TaxCalculator.Core.Interfaces
{
    public interface ICongestionTaxService
    {
        public int GetTax(DateTime[] dates, Vehicle vehicle);

    }
}
