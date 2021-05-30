using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateMonthlyPayslip.Models
{
    public class MonthlyPayslip
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
