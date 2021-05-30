using GenerateMonthlyPayslip.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateMonthlyPayslip.Models
{
    public class MonthlyPayslipRequestModel
    {
        public decimal TaxableIncome { get; set; }

        public TaxRateType TaxRateType { get; set; }

    }
}
