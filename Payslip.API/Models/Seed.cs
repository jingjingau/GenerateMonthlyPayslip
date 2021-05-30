using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.Models
{
    public class Seed
    {
        public static void SeedTaxRates(PayslipDbContext payslipDbContext) 
        {
            if (!payslipDbContext.TaxRates.Any())
            {
                var taxRateLevels = new TaxRateLevel[]
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

                var taxRates = new TaxRate[]
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

                payslipDbContext.TaxRateLevels.AddRange(taxRateLevels);
                payslipDbContext.TaxRates.AddRange(taxRates);

                payslipDbContext.SaveChanges();
            }
        }
    }
}
