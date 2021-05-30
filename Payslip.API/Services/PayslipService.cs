using Payslip.API.Dtos;
using Payslip.API.Enums;
using Payslip.API.Helpers;
using Payslip.API.Models;
using Payslip.API.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.Services
{
    public class PayslipService: IPayslipService
    {
        private readonly PayslipDbContext _payslipDbContext;

        public PayslipService(PayslipDbContext payslipDbContext)
        {
            _payslipDbContext = payslipDbContext;
        }

        /// <summary>
        /// Generate monthly pay slip giving taxable income and tax rate type
        /// </summary>
        /// <param name="taxableIncome"></param>
        /// <param name="taxRateType"></param>
        /// <returns></returns>
        public MonthlyPayslipDto GenerateMonthlyPayslip(decimal taxableIncome, TaxRateType taxRateType)
        {
            try
            {
                if (taxableIncome < 0)
                    throw new ArgumentException($"Invalid parameter taxableIncome value: {taxableIncome}.");

                GetRelatedDataFromDb(_payslipDbContext, taxRateType, out List<TaxRateLevel> taxRateLevels, out List<TaxRate> taxRates);

                BaseTaxCalculateStrategy taxCalculateStrategy = taxRateType switch
                {
                    TaxRateType.ResidentTaxRate => new ResidentIncomeTaxCalculateStrategy(taxRateLevels, taxRates),
                    _ => throw new NotImplementedException(),
                };

                var annualTax = taxCalculateStrategy.CalculateTax(taxableIncome);

                decimal grossMonthlyIncome = taxableIncome / 12;
                decimal monthlyIncomeTax = annualTax / 12;
                decimal netMonthlyIncome = grossMonthlyIncome - monthlyIncomeTax;

                var paySlip = new MonthlyPayslipDto
                {
                    GrossMonthlyIncome = grossMonthlyIncome,
                    MonthlyIncomeTax = monthlyIncomeTax,
                    NetMonthlyIncome = netMonthlyIncome
                };

                return paySlip;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception happened when generating monthly pay slip for taxable income: {taxableIncome} and tax rate type: {taxRateType}", ex);
            }
        }



        /// <summary>
        /// Get related tax rate levels and tax rates data from db giving the finianl year and tax rate type. 
        /// </summary>
        /// <param name="taxRateLevelsDataInDb"></param>
        /// <param name="taxRatesDataInDb"></param>
        /// <param name="taxRateType"></param>
        /// <param name="taxRateLevels"></param>
        /// <param name="taxRates"></param>
        /// <returns></returns>
        private static bool GetRelatedDataFromDb(PayslipDbContext payslipDbContext, 
                                          TaxRateType taxRateType, 
                                          out List<TaxRateLevel> taxRateLevels, 
                                          out List<TaxRate> taxRates)
        {
            try
            {
                taxRateLevels = payslipDbContext.TaxRateLevels.Where(x => x.TaxRateTypeInternal == (int)taxRateType).ToList();

                var financialYearStart = DateTime.Now.GetFinancialYearStart();
                var financialYearEnd = DateTime.Now.GetFinancialYearEnd();
                var taxRateLevelsIds = taxRateLevels.Select(x => x.Id).ToList();

                taxRates = payslipDbContext.TaxRates.Where(t => taxRateLevelsIds.Contains(t.TaxRateLevelId) 
                                                                && t.FinancialYearStart == financialYearStart
                                                                && t.FinancialYearEnd == financialYearEnd).ToList();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception happened when getting related data from db for tax rate type: {taxRateType}", ex);
            }
        }
    }
}
