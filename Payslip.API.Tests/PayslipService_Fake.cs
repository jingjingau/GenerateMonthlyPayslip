using Payslip.API.Dtos;
using Payslip.API.Enums;
using Payslip.API.Helpers;
using Payslip.API.Models;
using Payslip.API.Services;
using Payslip.API.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payslip.API.Tests
{
    public class PayslipService_Fake : IPayslipService
    {
        protected List<TaxRateLevel> taxRateLevelsDataInDb { get; set; }
        protected List<TaxRate> taxRatesDataInDb { get; set; }

        public PayslipService_Fake()
        {
            taxRateLevelsDataInDb = new List<TaxRateLevel>
            {
               new TaxRateLevel { Id = 1, TaxRateTypeInternal = 1, Level = 1, TaxableIncomeLowerBound = 0, TaxableIncomeUpperBound = 20000 },
               new TaxRateLevel { Id = 2, TaxRateTypeInternal = 1, Level = 2, TaxableIncomeLowerBound = 20001, TaxableIncomeUpperBound = 40000 },
               new TaxRateLevel { Id = 3, TaxRateTypeInternal = 1, Level = 3, TaxableIncomeLowerBound = 40001, TaxableIncomeUpperBound = 80000 },
               new TaxRateLevel { Id = 4, TaxRateTypeInternal = 1, Level = 4, TaxableIncomeLowerBound = 80001, TaxableIncomeUpperBound = 180000 },
               new TaxRateLevel { Id = 5, TaxRateTypeInternal = 1, Level = 5, TaxableIncomeLowerBound = 180001 },
               new TaxRateLevel { Id = 6, TaxRateTypeInternal = 2, Level = 1, TaxableIncomeLowerBound = 0, TaxableIncomeUpperBound = 120000 },
               new TaxRateLevel { Id = 7, TaxRateTypeInternal = 2, Level = 2, TaxableIncomeLowerBound = 120001, TaxableIncomeUpperBound = 180000 },
               new TaxRateLevel { Id = 8, TaxRateTypeInternal = 2, Level = 3, TaxableIncomeLowerBound = 180001 }
            };

            taxRatesDataInDb = new List<TaxRate>
            {
                new TaxRate { Id = 1, FinancialYearStart = 2020, FinancialYearEnd = 2021, TaxRateLevelId = 1, Rate = 0 },
                new TaxRate { Id = 2, FinancialYearStart = 2020, FinancialYearEnd = 2021, TaxRateLevelId = 2, Rate = (decimal)0.1 },
                new TaxRate { Id = 3, FinancialYearStart = 2020, FinancialYearEnd = 2021, TaxRateLevelId = 3, Rate = (decimal)0.2 },
                new TaxRate { Id = 4, FinancialYearStart = 2020, FinancialYearEnd = 2021, TaxRateLevelId = 4, Rate = (decimal)0.3 },
                new TaxRate { Id = 5, FinancialYearStart = 2020, FinancialYearEnd = 2021, TaxRateLevelId = 5, Rate = (decimal)0.4 },
                new TaxRate { Id = 6, FinancialYearStart = 2020, FinancialYearEnd = 2021, TaxRateLevelId = 6, Rate = (decimal)0.3 },
                new TaxRate { Id = 7, FinancialYearStart = 2020, FinancialYearEnd = 2021, TaxRateLevelId = 7, Rate = (decimal)0.4 },
                new TaxRate { Id = 8, FinancialYearStart = 2020, FinancialYearEnd = 2021, TaxRateLevelId = 8, Rate = (decimal)0.45 }
            };
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

                GetRelatedDataFromDb(taxRateLevelsDataInDb, taxRatesDataInDb, taxRateType, out List<TaxRateLevel> taxRateLevels, out List<TaxRate> taxRates);

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
        private static bool GetRelatedDataFromDb(List<TaxRateLevel> taxRateLevelsDataInDb,
                                                  List<TaxRate> taxRatesDataInDb,
                                                  TaxRateType taxRateType,
                                                  out List<TaxRateLevel> taxRateLevels,
                                                  out List<TaxRate> taxRates)
        {
            try
            {
                taxRateLevels = taxRateLevelsDataInDb.Where(x => x.TaxRateTypeInternal == (int)taxRateType).ToList();

                var financialYearStart = DateTime.Now.GetFinancialYearStart();
                var financialYearEnd = DateTime.Now.GetFinancialYearEnd();
                var taxRateLevelsIds = taxRateLevels.Select(x => x.Id).ToList();

                taxRates = taxRatesDataInDb.Where(t => taxRateLevelsIds.Contains(t.TaxRateLevelId)
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
