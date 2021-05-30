using Payslip.API.Models;
using Payslip.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.ChainOfResponsibility.ResidentIncomeTaxCalculation
{
    public class TaxRateLevelFourTaxCalculation : BaseTaxRateCalculation
    {
        private static TaxRateLevelFourTaxCalculation Current { get; set; }

        private static readonly object _lockObj = new();

        private TaxRateLevelFourTaxCalculation(List<TaxRateLevel> taxRateLevels, List<TaxRate> taxRates) : base(taxRateLevels, taxRates)
        {
            NextTaxRateLevelCalculation = TaxRateLevelFiveTaxCalculation.GetInstance(taxRateLevels, taxRates);
        }

        public static TaxRateLevelFourTaxCalculation GetInstance(List<TaxRateLevel> taxRateLevels, List<TaxRate> taxRates)
        {
            if (Current == null)
            {
                lock (_lockObj)
                {
                    Current ??= new TaxRateLevelFourTaxCalculation(taxRateLevels, taxRates);
                }
            }

            return Current;
        }

        public override void SetTaxRateLevel()
        {
            var taxRateLevel = TaxRateLevels.Where(x => x.Level == 4 && x.TaxRateTypeInternal == (int)TaxRateType.ResidentTaxRate).FirstOrDefault();
            if (taxRateLevel == null)
                throw new Exception($"Failed to get tax rate level for Level 4, tax rate type: {TaxRateType.ResidentTaxRate}");

            if (taxRateLevel.TaxableIncomeLowerBound >= taxRateLevel.TaxableIncomeUpperBound)
            {
                throw new Exception($"Invalid taxable income lower bound and upper bound settings for Level 4, tax rate type: {TaxRateType.ResidentTaxRate}");
            }

            TaxRateLevel = taxRateLevel;
        }

    }
}
