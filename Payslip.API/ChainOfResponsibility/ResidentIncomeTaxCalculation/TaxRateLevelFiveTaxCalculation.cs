using Payslip.API.Models;
using Payslip.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.ChainOfResponsibility.ResidentIncomeTaxCalculation
{
    public class TaxRateLevelFiveTaxCalculation : BaseTaxRateCalculation
    {
        private static TaxRateLevelFiveTaxCalculation Current { get; set; }

        private static readonly object _lockObj = new();

        private TaxRateLevelFiveTaxCalculation(List<TaxRateLevel> taxRateLevels, List<TaxRate> taxRates) : base(taxRateLevels, taxRates)
        {
        }

        public static TaxRateLevelFiveTaxCalculation GetInstance(List<TaxRateLevel> taxRateLevels, List<TaxRate> taxRates)
        {
            if (Current == null)
            {
                lock (_lockObj)
                {
                    Current ??= new TaxRateLevelFiveTaxCalculation(taxRateLevels, taxRates);
                }
            }

            return Current;
        }

        public override void SetTaxRateLevel()
        {
            var taxRateLevel = TaxRateLevels.Where(x => x.Level == 5 && x.TaxRateTypeInternal == (int)TaxRateType.ResidentTaxRate).FirstOrDefault();
            TaxRateLevel = taxRateLevel ?? throw new Exception($"Failed to get tax rate level for Level 5, tax rate type: {TaxRateType.ResidentTaxRate}");
        }

        public override decimal CalculateTax(decimal taxableIncome)
        {
            decimal tax = 0;

            if (taxableIncome < 0)
                throw new ArgumentException($"Invalid parameter taxableIncome value: {taxableIncome}");

            if (taxableIncome >= TaxRateLevel.TaxableIncomeLowerBound)
            {
                tax = (taxableIncome - TaxRateLevel.TaxableIncomeLowerBound + 1) * TaxRate.Rate;
            }

            return tax;

        }

    }
}
