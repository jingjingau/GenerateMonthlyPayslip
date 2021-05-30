using Payslip.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Payslip.API.Tests
{
    public class PayslipServiceTest
    {
        PayslipService_Fake _service;

        public PayslipServiceTest()
        {
            _service = new PayslipService_Fake();
        }

        [Fact]
        public void GenerateMonthlyPayslip_TaxableIncomeLessThanZero_ThrowException()
        {
            // Arrange
            decimal taxableIncome = -50;
            TaxRateType taxRateType = TaxRateType.ResidentTaxRate;

            // Act      
            Action act = () => _service.GenerateMonthlyPayslip(taxableIncome, taxRateType);

            // Assert
            Exception exception = Assert.Throws<Exception>(act);

        }

        [Fact]
        public void GenerateMonthlyPayslip_NotImplementedTaxRateType_ThrowException()
        {
            // Arrange
            decimal taxableIncome = 100;
            TaxRateType taxRateType = TaxRateType.ForeignResidentTaxRate;

            // Act      
            Action act = () => _service.GenerateMonthlyPayslip(taxableIncome, taxRateType);

            // Assert
            Exception exception = Assert.Throws<Exception>(act);

        }


        [Theory]
        [InlineData(0)]
        [InlineData(10000)]
        [InlineData(20000)]
        public void GenerateMonthlyPayslip_TaxableIncomeWithInTaxRateLevelOne_ReturnsRightItem(decimal value)
        {
            // Arrange
            TaxRateType taxRateType = TaxRateType.ResidentTaxRate;

            decimal expectedMonthlyIncomeTax = 0;
            decimal expectedGrossMonthlyIncome = value / 12;

            decimal expectedNetMonthlyIncome = expectedGrossMonthlyIncome - expectedMonthlyIncomeTax;

            // Act
            var returnResult = _service.GenerateMonthlyPayslip(value, taxRateType);

            // Assert
            Assert.Equal(expectedGrossMonthlyIncome.ToString("#.##"), returnResult.GrossMonthlyIncome.ToString("#.##"));
            Assert.Equal(expectedMonthlyIncomeTax.ToString("#.##"), returnResult.MonthlyIncomeTax.ToString("#.##"));
            Assert.Equal(expectedNetMonthlyIncome.ToString("#.##"), returnResult.NetMonthlyIncome.ToString("#.##"));
        }

        [Theory]
        [InlineData(20001)]
        [InlineData(30000)]
        [InlineData(40000)]
        public void GenerateMonthlyPayslip_TaxableIncomeWithInTaxRateLevelTwo_ReturnsRightItem(decimal value)
        {
            // Arrange
            TaxRateType taxRateType = TaxRateType.ResidentTaxRate;

            decimal expectedAnnualTax = (value - (decimal)20000.0) * (decimal)0.1;
            decimal expectedMonthlyIncomeTax = expectedAnnualTax / 12;
            decimal expectedGrossMonthlyIncome = value / 12;
            decimal expectedNetMonthlyIncome = expectedGrossMonthlyIncome - expectedMonthlyIncomeTax;

            // Act
            var returnResult = _service.GenerateMonthlyPayslip(value, taxRateType);

            // Assert
            Assert.Equal(expectedGrossMonthlyIncome.ToString("#.##"), returnResult.GrossMonthlyIncome.ToString("#.##"));
            Assert.Equal(expectedMonthlyIncomeTax.ToString("#.##"), returnResult.MonthlyIncomeTax.ToString("#.##"));
            Assert.Equal(expectedNetMonthlyIncome.ToString("#.##"), returnResult.NetMonthlyIncome.ToString("#.##"));
        }

        [Theory]
        [InlineData(40001)]
        [InlineData(50000)]
        [InlineData(80000)]
        public void GenerateMonthlyPayslip_TaxableIncomeWithInTaxRateLevelThree_ReturnsRightItem(decimal value)
        {
            // Arrange
            TaxRateType taxRateType = TaxRateType.ResidentTaxRate;

            decimal expectedAnnualTax = (value - 40000) * (decimal)0.2 + 2000;
            decimal expectedMonthlyIncomeTax = expectedAnnualTax / 12;
            decimal expectedGrossMonthlyIncome = value / 12;
            decimal expectedNetMonthlyIncome = expectedGrossMonthlyIncome - expectedMonthlyIncomeTax;

            // Act
            var returnResult = _service.GenerateMonthlyPayslip(value, taxRateType);

            // Assert
            Assert.Equal(expectedGrossMonthlyIncome.ToString("#.##"), returnResult.GrossMonthlyIncome.ToString("#.##"));
            Assert.Equal(expectedMonthlyIncomeTax.ToString("#.##"), returnResult.MonthlyIncomeTax.ToString("#.##"));
            Assert.Equal(expectedNetMonthlyIncome.ToString("#.##"), returnResult.NetMonthlyIncome.ToString("#.##"));
        }

        [Theory]
        [InlineData(80001)]
        [InlineData(100000)]
        [InlineData(180000)]
        public void GenerateMonthlyPayslip_TaxableIncomeWithInTaxRateLevelFour_ReturnsRightItem(decimal value)
        {
            // Arrange
            TaxRateType taxRateType = TaxRateType.ResidentTaxRate;

            decimal expectedAnnualTax = (value - 80000) * (decimal)0.3 + 2000 + 8000;
            decimal expectedMonthlyIncomeTax = expectedAnnualTax / 12;
            decimal expectedGrossMonthlyIncome = value / 12;
            decimal expectedNetMonthlyIncome = expectedGrossMonthlyIncome - expectedMonthlyIncomeTax;

            // Act
            var returnResult = _service.GenerateMonthlyPayslip(value, taxRateType);

            // Assert
            Assert.Equal(expectedGrossMonthlyIncome.ToString("#.##"), returnResult.GrossMonthlyIncome.ToString("#.##"));
            Assert.Equal(expectedMonthlyIncomeTax.ToString("#.##"), returnResult.MonthlyIncomeTax.ToString("#.##"));
            Assert.Equal(expectedNetMonthlyIncome.ToString("#.##"), returnResult.NetMonthlyIncome.ToString("#.##"));
        }

        [Theory]
        [InlineData(180001)]
        [InlineData(200000)]
        [InlineData(1800000000)]
        public void GenerateMonthlyPayslip_TaxableIncomeWithInTaxRateLevelFive_ReturnsRightItem(decimal value)
        {
            // Arrange
            TaxRateType taxRateType = TaxRateType.ResidentTaxRate;

            decimal expectedAnnualTax = (value - 180000) * (decimal)0.4 + 2000 + 8000 + 30000;
            decimal expectedMonthlyIncomeTax = expectedAnnualTax / 12;
            decimal expectedGrossMonthlyIncome = value / 12;
            decimal expectedNetMonthlyIncome = expectedGrossMonthlyIncome - expectedMonthlyIncomeTax;

            // Act
            var returnResult = _service.GenerateMonthlyPayslip(value, taxRateType);

            // Assert
            Assert.Equal(expectedGrossMonthlyIncome.ToString("#.##"), returnResult.GrossMonthlyIncome.ToString("#.##"));
            Assert.Equal(expectedMonthlyIncomeTax.ToString("#.##"), returnResult.MonthlyIncomeTax.ToString("#.##"));
            Assert.Equal(expectedNetMonthlyIncome.ToString("#.##"), returnResult.NetMonthlyIncome.ToString("#.##"));
        }
    }
}
