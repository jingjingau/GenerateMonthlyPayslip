using GenerateMonthlyPayslip.Models;
using GenerateMonthlyPayslip.ServiceAgents;
using GenerateMonthlyPayslip.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateMonthlyPayslip
{
    public class MonthlyPayslipGenerator
    {
        private readonly MonthlyPayslipServiceAgent _serviceAgent;

        public MonthlyPayslipGenerator(MonthlyPayslipServiceAgent serviceAgent)
        {
            _serviceAgent = serviceAgent;
        }
         
        /// <summary>
        /// Get monthly pay slip
        /// </summary>
        /// <returns></returns>
        public async Task<MonthlyPayslip> GetMonthlyPayslipAsync(MonthlyPayslipRequestModel model)
        {
            if (model.TaxableIncome < 0)
                return null;

            if (model.TaxRateType != Enums.TaxRateType.ResidentTaxRate)
                return null;

            Console.WriteLine(nameof(GetMonthlyPayslipAsync));
            var result = await _serviceAgent.PostAsync(Urls.MonthlyPayslipApi, model);

            return result;
        }

    }
}
