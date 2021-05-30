using GenerateMonthlyPayslip.Models;
using GenerateMonthlyPayslip.ServiceAgents;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GenerateMonthlyPayslip.Tests
{
    public class MonthlyPayslipGeneratorTest
    {
        [Fact]
        public void GetMonthlyPaySlipAsync_InputRightModel_ReturnRightItem()
        {
            // arrange
            var mock = new Mock<MonthlyPayslipServiceAgent>("https://localhost:44351/");

            MonthlyPayslip expectedMonthlyPaySlip = new MonthlyPayslip
            {
                GrossMonthlyIncome = 5000,
                MonthlyIncomeTax = 500,
                NetMonthlyIncome = 4500
            };

            MonthlyPayslipRequestModel inputModel = new MonthlyPayslipRequestModel
            {
                TaxableIncome = 60000,
                TaxRateType = GenerateMonthlyPayslip.Enums.TaxRateType.ResidentTaxRate,
            };

            string url = "Payslip";
            mock.Setup(r => r.PostAsync(url, inputModel)).ReturnsAsync(expectedMonthlyPaySlip);

            // act

            var monthlyPaySlipGenerator = new MonthlyPayslipGenerator(mock.Object);

            var response = monthlyPaySlipGenerator.GetMonthlyPayslipAsync(inputModel).Result;

            // assert

            Assert.Equal(expectedMonthlyPaySlip.GrossMonthlyIncome.ToString("#.##"), response.GrossMonthlyIncome.ToString("#.##"));
            Assert.Equal(expectedMonthlyPaySlip.MonthlyIncomeTax.ToString("#.##"), response.MonthlyIncomeTax.ToString("#.##"));
            Assert.Equal(expectedMonthlyPaySlip.NetMonthlyIncome.ToString("#.##"), response.NetMonthlyIncome.ToString("#.##"));
        }

        [Fact]
        public void GetMonthlyPaySlipAsync_TaxableIncomeLessThanZero_ReturnNullItem()
        {
            // arrange
            var mock = new Mock<MonthlyPayslipServiceAgent>("https://localhost:44351/");

            MonthlyPayslipRequestModel inputModel = new MonthlyPayslipRequestModel
            {
                TaxableIncome = -10,
                TaxRateType = GenerateMonthlyPayslip.Enums.TaxRateType.ResidentTaxRate,
            };


            var monthlyPaySlipGenerator = new MonthlyPayslipGenerator(mock.Object);

            var response = monthlyPaySlipGenerator.GetMonthlyPayslipAsync(inputModel).Result;

            // assert

            Assert.Null(response);

        }

        [Fact]
        public void GetMonthlyPaySlipAsync_WrongTaxRateType_ReturnNullItem()
        {
            // arrange
            var mock = new Mock<MonthlyPayslipServiceAgent>("https://localhost:44351/");

            MonthlyPayslipRequestModel inputModel = new MonthlyPayslipRequestModel
            {
                TaxableIncome = -10,
                TaxRateType = GenerateMonthlyPayslip.Enums.TaxRateType.ForeignResidentTaxRate,
            };


            var monthlyPaySlipGenerator = new MonthlyPayslipGenerator(mock.Object);

            var response = monthlyPaySlipGenerator.GetMonthlyPayslipAsync(inputModel).Result;

            // assert

            Assert.Null(response);

        }
    }
}
