using Payslip.API.Dtos;
using Payslip.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.Services
{
    public interface IPayslipService
    {
        /// <summary>
        /// Generate monthly pay slip giving taxable income and tax rate type
        /// </summary>
        /// <param name="taxableIncome"></param>
        /// <param name="taxRateType"></param>
        /// <returns></returns>
        MonthlyPayslipDto GenerateMonthlyPayslip(decimal taxableIncome, TaxRateType taxRateType);
    }
}
