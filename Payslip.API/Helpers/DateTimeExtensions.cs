using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.Helpers
{
    public static class DateTimeExtensions
    {
        public static int GetFinancialYearStart(this DateTime dateTime)
        {
            int financialYearStart = dateTime.Month >= 7 ? dateTime.Year : (dateTime.Year - 1);

            return financialYearStart;
        }

        public static int GetFinancialYearEnd(this DateTime dateTime)
        {
            int financialYearEnd = dateTime.Month >= 7 ? (dateTime.Year + 1) : (dateTime.Year);

            return financialYearEnd;
        }

    }
}
