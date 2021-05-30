using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateMonthlyPayslip
{
    public class ParametersValidation
    {
        public static bool ValidateInputParameters(string[] args, out string name, out decimal taxableIncome)
        {
            name = string.Empty;
            taxableIncome = 0;

            if (args.Length != 3)
            {
                Console.WriteLine($"Please input the right parameters: GenerateMonthlypayslip Name TaxableIncome");
                return false;
            }

            if (string.IsNullOrWhiteSpace(args[1]))
            {
                Console.WriteLine($"Invalid Name");
                return false;
            }
            name = args[1].Trim();

            if (string.IsNullOrWhiteSpace(args[2]))
            {
                Console.WriteLine($"Invalid TaxableIncome");
                return false;
            }

            if (!decimal.TryParse(args[2], out taxableIncome))
            {
                Console.WriteLine($"Invalid TaxableIncome");
                return false;
            }

            if (taxableIncome < 0)
            {
                Console.WriteLine($"TaxableIncome must be equal or greater than 0");
                return false;
            }

            return true;
        }
    }
}
