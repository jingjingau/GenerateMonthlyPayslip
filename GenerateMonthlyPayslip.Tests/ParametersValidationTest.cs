using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GenerateMonthlyPayslip.Tests
{
    public class ParametersValidationTest
    {
        [Fact]
        public void ValidateInputParameters_InputParametersCountNotThree_ReturnFalse()
        {
            // Arrange
            decimal taxableIncome;
            string name;
            string[] args = new[] { "GenerateMonthlyPayslip", "Mary Song" };

            // Act      

            var result = ParametersValidation.ValidateInputParameters(args, out name, out taxableIncome);

            // Assert
            Assert.False(result);

        }

        [Fact]
        public void ValidateInputParameters_InputBlankName_ReturnFalse()
        {
            // Arrange
            decimal taxableIncome;
            string name;
            string[] args = new[] { "GenerateMonthlyPayslip", "", "60000" };

            // Act      

            var result = ParametersValidation.ValidateInputParameters(args, out name, out taxableIncome);

            // Assert
            Assert.False(result);

        }

        [Fact]
        public void ValidateInputParameters_InputTaxableIncomeLessThanZero_ReturnFalse()
        {
            // Arrange
            decimal taxableIncome;
            string name;
            string[] args = new[] { "GenerateMonthlyPayslip", "Mary Blair", "-100" };

            // Act      

            var result = ParametersValidation.ValidateInputParameters(args, out name, out taxableIncome);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateInputParameters_InputTaxableIncomeNotNumber_ReturnFalse()
        {
            // Arrange
            decimal taxableIncome;
            string name;
            string[] args = new[] { "GenerateMonthlyPayslip", "Mary Blair", "10000aus" };

            // Act      

            var result = ParametersValidation.ValidateInputParameters(args, out name, out taxableIncome);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateInputParameters_InputBlankTaxableIncome_ReturnFalse()
        {
            // Arrange
            decimal taxableIncome;
            string name;
            string[] args = new[] { "GenerateMonthlyPayslip", "Mary Blair", "  " };

            // Act      

            var result = ParametersValidation.ValidateInputParameters(args, out name, out taxableIncome);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateInputParameters_InputRightParameters_ReturnTrueAndOuputNameAndTaxableIncome()
        {
            // Arrange
            decimal taxableIncome;
            string name;
            string[] args = new[] { "GenerateMonthlyPayslip", "Mary Blair", "60000" };

            // Act      

            var result = ParametersValidation.ValidateInputParameters(args, out name, out taxableIncome);

            // Assert
            Assert.True(result);
            Assert.Equal("Mary Blair", name);
            Assert.Equal(60000, taxableIncome);
        }

    }
}
