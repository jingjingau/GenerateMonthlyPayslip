using Payslip.API.Models;
using Payslip.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.ChainOfResponsibility.ResidentIncomeTaxCalculation
{
    public class TaxRateLevelOneTaxCalculation : BaseTaxRateCalculation
    {
        private static TaxRateLevelOneTaxCalculation Current { get; set; }

        private static readonly object _lockObj = new();


        private TaxRateLevelOneTaxCalculation(List<TaxRateLevel> taxRateLevels, List<TaxRate> taxRates) : base(taxRateLevels, taxRates)
        {
            NextTaxRateLevelCalculation = TaxRateLevelTwoTaxCalculation.GetInstance(taxRateLevels, taxRates);
        }

        public static TaxRateLevelOneTaxCalculation GetInstance(List<TaxRateLevel> taxRateLevels, List<TaxRate> taxRates)
        {
            if (Current == null)
            {
                lock (_lockObj)
                {
                    Current ??= new TaxRateLevelOneTaxCalculation(taxRateLevels, taxRates);
                }
            }

            return Current;
        }

        public override void SetTaxRateLevel()
        {
            var taxRateLevel = TaxRateLevels.Where(x => x.Level == 1 && x.TaxRateTypeInternal == (int)TaxRateType.ResidentTaxRate).FirstOrDefault();
            if (taxRateLevel == null)
                throw new Exception($"Failed to get tax rate level for Level 1, tax rate type: {TaxRateType.ResidentTaxRate}");

            if (taxRateLevel.TaxableIncomeLowerBound != 0)
            {
                throw new Exception($"Taxable income lower bound is not 0 for Level 1, tax rate type: {TaxRateType.ResidentTaxRate}");
            }

            if (taxRateLevel.TaxableIncomeLowerBound >= taxRateLevel.TaxableIncomeUpperBound)
            {
                throw new Exception($"Invalid taxable income lower bound and upper bound settings for Level 1, tax rate type: {TaxRateType.ResidentTaxRate}");
            }

            TaxRateLevel = taxRateLevel;
        }

        public override decimal CalculateTax(decimal taxableIncome)
        {
            if (taxableIncome < 0)
                throw new ArgumentException($"Invalid parameter taxableIncome value: {taxableIncome}");

            decimal tax;
            if (taxableIncome <= TaxRateLevel.TaxableIncomeUpperBound)
            {
                tax = taxableIncome * TaxRate.Rate;
            }
            else
            {
                int upperBound = TaxRateLevel.TaxableIncomeUpperBound ?? 0;
                decimal taxWithinTaxRateLevelRange = (upperBound) * TaxRate.Rate;
                decimal taxGreatThanTaxRatelevelRange = NextTaxRateLevelCalculation.CalculateTax(taxableIncome);
                tax = taxWithinTaxRateLevelRange + taxGreatThanTaxRatelevelRange;
            }

            return tax;

        }

    }
}
