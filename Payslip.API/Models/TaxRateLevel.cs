using Payslip.API.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.Models
{
    public class TaxRateLevel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public int TaxRateTypeInternal { get; set; }

        [NotMapped]
        public TaxRateType TaxRateType
        {
            get { return (TaxRateType)TaxRateTypeInternal; }

            set { TaxRateTypeInternal = (int)TaxRateType; }
        }

        [Required]
        [Range(1, 100)]
        public int Level { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int TaxableIncomeLowerBound { get; set; }

        [Range(0, int.MaxValue)]
        public int? TaxableIncomeUpperBound { get; set; }

    }
}
