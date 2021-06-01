using Microsoft.AspNetCore.Mvc;
using Moq;
using Payslip.API.Controllers;
using Payslip.API.Dtos;
using Payslip.API.Models;
using Payslip.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Payslip.API.Tests
{
    public class PayslipControllerTest
    {
        [Fact]
        public void MonthlyPayslip_WhenInputRightParameters_ReturnRightValues()
        {
            // arrange
            var mock = new Mock<IPayslipService>();

            var requestMonthlyPayslipDtoInput = new RequestMonthlyPayslipDto
            {
                TaxableIncome = 100000,
                TaxRateType = Enums.TaxRateType.ResidentTaxRate,
            };

            decimal value = requestMonthlyPayslipDtoInput.TaxableIncome;
            decimal expectedAnnualTax = (value - 80000) * (decimal)0.3 + 2000 + 8000;
            decimal expectedMonthlyIncomeTax = expectedAnnualTax / 12;
            decimal expectedGrossMonthlyIncome = value / 12;
            decimal expectedNetMonthlyIncome = expectedGrossMonthlyIncome - expectedMonthlyIncomeTax;

            var expectedMonthlyPayslip = new MonthlyPayslipDto
            {
                GrossMonthlyIncome = expectedGrossMonthlyIncome,
                MonthlyIncomeTax = expectedMonthlyIncomeTax,
                NetMonthlyIncome = expectedNetMonthlyIncome
            };

            mock.Setup(r => r.GenerateMonthlyPayslip(100000, Enums.TaxRateType.ResidentTaxRate)).Returns(expectedMonthlyPayslip);
            var controller = new PayslipController(mock.Object);

            // act
            var result = controller.MonthlyPayslip(requestMonthlyPayslipDtoInput);

            var objectResult = result as ObjectResult;

            var returnMonthlyPayslip = objectResult.Value as MonthlyPayslipDto; 

            // assert
            Assert.NotNull(objectResult);

            Assert.Equal(expectedGrossMonthlyIncome.ToString("#.##"), returnMonthlyPayslip.GrossMonthlyIncome.ToString("#.##"));
            Assert.Equal(expectedMonthlyIncomeTax.ToString("#.##"), returnMonthlyPayslip.MonthlyIncomeTax.ToString("#.##"));
            Assert.Equal(expectedNetMonthlyIncome.ToString("#.##"), returnMonthlyPayslip.NetMonthlyIncome.ToString("#.##"));

        }

        [Fact]
        public void MonthlyPayslip_WhenInputWrongTaxRateType_ReturnBadRequest()
        {
            // arrange
            var mock = new Mock<IPayslipService>();

            var requestMonthlyPayslipDtoInput = new RequestMonthlyPayslipDto
            {
                TaxableIncome = 100000,
                TaxRateType = Enums.TaxRateType.ForeignResidentTaxRate,
            };

            var controller = new PayslipController(mock.Object);

            // act
            var result = controller.MonthlyPayslip(requestMonthlyPayslipDtoInput);

            var badRequestObjectResult = result as BadRequestObjectResult;

            // assert
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public void MonthlyPayslip_WhenInputNegtiveTaxableIncome_ReturnInternalServerError()
        {
            // arrange
            var mock = new Mock<IPayslipService>();

            var requestMonthlyPayslipDtoInput = new RequestMonthlyPayslipDto
            {
                TaxableIncome = -50,
                TaxRateType = Enums.TaxRateType.ResidentTaxRate,
            };

            var controller = new PayslipController(mock.Object);

            mock.Setup(r => r.GenerateMonthlyPayslip(-50, Enums.TaxRateType.ResidentTaxRate))
                .Callback(() => throw new Exception("Exception happened when generating monthly pay slip for taxable income: -50 and tax rate type: ResidentTaxRate."));
            
            // act

            var result = controller.MonthlyPayslip(requestMonthlyPayslipDtoInput);

            // assert
            var statusCodeResult = result as StatusCodeResult;

            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
