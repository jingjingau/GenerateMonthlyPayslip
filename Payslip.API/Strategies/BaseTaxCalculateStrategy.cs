using Payslip.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.Strategies
{
    public abstract class BaseTaxCalculateStrategy
    {
        protected List<TaxRateLevel> TaxRateLevels { get; set; }
        protected List<TaxRate> TaxRates { get; set; }

        public BaseTaxCalculateStrategy(List<TaxRateLevel> taxRateLevels, List<TaxRate> taxRates)
        {
            TaxRateLevels = taxRateLevels;
            TaxRates = taxRates;
        }

        public abstract decimal CalculateTax(decimal taxableIncome);
    }
}
