using Payslip.API.ChainOfResponsibility.ResidentIncomeTaxCalculation;
using Payslip.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.Strategies
{
    public class ResidentIncomeTaxCalculateStrategy : BaseTaxCalculateStrategy
    {
        public ResidentIncomeTaxCalculateStrategy(List<TaxRateLevel> taxRateLevels, List<TaxRate> taxRates) : base(taxRateLevels, taxRates)
        {
        }

        public override decimal CalculateTax(decimal taxableIncome)
        {
            if (taxableIncome < 0)
                throw new ArgumentException($"Invalid parameter taxableIncome value: {taxableIncome}.");

            var taxCalculation = TaxRateLevelOneTaxCalculation.GetInstance(TaxRateLevels, TaxRates);

            var tax = taxCalculation.CalculateTax(taxableIncome);

            return tax;
        }

    }
}
