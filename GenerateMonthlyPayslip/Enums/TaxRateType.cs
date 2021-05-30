using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateMonthlyPayslip.Enums
{
    public enum TaxRateType
    {
        [Description("Resident Tax Rate")]
        ResidentTaxRate = 1,

        [Description("Foreign Resident Tax Rate")]
        ForeignResidentTaxRate = 2,

        [Description("Children Tax Rate")]
        ChildrenTaxRate = 3,

        [Description("Working Holiday Maker Tax Rate")]
        WorkingHolidayMakerTaxRate = 4,
    }
}
