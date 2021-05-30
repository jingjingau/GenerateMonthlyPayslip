using GenerateMonthlyPayslip.Models;
using GenerateMonthlyPayslip.ServiceAgents;
using GenerateMonthlyPayslip.Utilities;
using System;

namespace GenerateMonthlyPayslip
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string name = string.Empty;

                if (!ParametersValidation.ValidateInputParameters(args, out name, out decimal taxableIncome))
                    return;

                MonthlyPayslipRequestModel model = new()
                {
                    TaxableIncome = taxableIncome,
                    TaxRateType = Enums.TaxRateType.ResidentTaxRate
                };
                var serviceAgent = new MonthlyPayslipServiceAgent(Urls.BaseAddress);
                var monthlyPaySlipGenerator = new MonthlyPayslipGenerator(serviceAgent);
                var task = monthlyPaySlipGenerator.GetMonthlyPayslipAsync(model);

                var monthlyPaySlip = task.Result;

                if (monthlyPaySlip != null)
                {
                    Console.WriteLine($"Monthly Payslip for: {name}");
                    Console.WriteLine($"Gross Monthly Income: {monthlyPaySlip.GrossMonthlyIncome:#.##}");
                    Console.WriteLine($"Gross Income Tax: {monthlyPaySlip.MonthlyIncomeTax:#.##}");
                    Console.WriteLine($"Net Monthly Income: {monthlyPaySlip.NetMonthlyIncome:#.##}");
                }

                return;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
