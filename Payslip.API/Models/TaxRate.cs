using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.Models
{
    public class TaxRate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [Range(0001, 9999)]
        public int FinancialYearStart { get; set; }

        [Required]
        [Range(0001, 9999)]
        public int FinancialYearEnd { get; set; }

        [Required]
        public int TaxRateLevelId { get; set; }

        public virtual TaxRateLevel TaxRateLevel { get; set; }

        [Required]
        [Range(0, 1)]
        public decimal Rate { get; set; }
    }
}
