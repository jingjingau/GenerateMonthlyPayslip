using Payslip.API.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.Dtos
{
    public class RequestMonthlyPayslipDto
    {
        /// <summary>
        /// Taxable Income
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public decimal TaxableIncome { get; set; }

        /// <summary>
        /// Tax Rate Type
        /// </summary>
        [EnumDataType(typeof(TaxRateType))]
        [Required]
        public TaxRateType TaxRateType { get; set; }
    }
}
