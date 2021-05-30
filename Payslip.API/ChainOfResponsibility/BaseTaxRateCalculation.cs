using Payslip.API.Enums;
using Payslip.API.Helpers;
using Payslip.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.ChainOfResponsibility
{
    public abstract class BaseTaxRateCalculation
    {
        protected TaxRateLevel TaxRateLevel { get; set; }
        protected TaxRateType TaxRateType { get; set; }
        protected TaxRate TaxRate { get; set; }

        protected List<TaxRateLevel> TaxRateLevels { get; set; }

        protected List<TaxRate> TaxRates { get; set; }

        public BaseTaxRateCalculation NextTaxRateLevelCalculation { get; set; }

        public BaseTaxRateCalculation(List<TaxRateLevel> taxRateLevels, List<TaxRate> taxRates)
        {
            TaxRateLevels = taxRateLevels;
            TaxRates = taxRates;

            TaxRateType = TaxRateType.ResidentTaxRate;
            SetProperties();
        }

        public virtual decimal CalculateTax(decimal taxableIncome)
        {
            if (taxableIncome < 0)
                throw new ArgumentException($"Invalid parameter taxableIncome value: {taxableIncome}.");

            decimal tax;
            if (taxableIncome <= TaxRateLevel.TaxableIncomeUpperBound
                && taxableIncome >= TaxRateLevel.TaxableIncomeLowerBound)
            {
                tax = (taxableIncome - TaxRateLevel.TaxableIncomeLowerBound + 1) * TaxRate.Rate;
            }
            else
            {
                int upperBound = TaxRateLevel.TaxableIncomeUpperBound ?? 0;
                decimal taxWithinTaxRateLevelRange = (upperBound - TaxRateLevel.TaxableIncomeLowerBound + 1) * TaxRate.Rate;
                decimal taxGreatThanTaxRatelevelRange = NextTaxRateLevelCalculation.CalculateTax(taxableIncome);
                tax = taxWithinTaxRateLevelRange + taxGreatThanTaxRatelevelRange;
            }

            return tax;
        }

        public void SetProperties()
        {
            SetTaxRateLevel();
            SetTaxRate();
        }


        public void SetTaxRate()
        {
            if (TaxRateLevel == null)
                throw new Exception("No tax rate level found.");

            var financialYearStart = DateTime.Now.GetFinancialYearStart();
            var financialYearEnd = DateTime.Now.GetFinancialYearEnd();

            TaxRate = TaxRates.Where(t => t.TaxRateLevelId == (int)TaxRateLevel.Id
                                                      && t.FinancialYearStart == financialYearStart
                                                      && t.FinancialYearEnd == financialYearEnd).FirstOrDefault();

            if (TaxRate == null)
                throw new Exception($"Failed to set tax rate for tax rate type: {TaxRateType}, level: {TaxRateLevel.Level}. ");
        }

        public abstract void SetTaxRateLevel();

    }
}
