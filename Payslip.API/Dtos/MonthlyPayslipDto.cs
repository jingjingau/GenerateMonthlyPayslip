using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.Dtos
{
    public class MonthlyPayslipDto
    {
        /// <summary>
        /// Gross Monthly Income.
        /// </summary>
        public decimal GrossMonthlyIncome { get; set; }

        /// <summary>
        /// Monthly Income Tax.
        /// </summary>
        public decimal MonthlyIncomeTax { get; set; }

        /// <summary>
        /// Net Monthly Income.
        /// </summary>
        public decimal NetMonthlyIncome { get; set; }
    }
}
