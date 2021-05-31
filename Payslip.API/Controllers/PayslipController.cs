using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payslip.API.Dtos;
using Payslip.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PayslipController : ControllerBase
    {
        private readonly IPayslipService _payslipService;

        private readonly ILogger<PayslipController> _logger;

        public PayslipController(IPayslipService payslipService, ILogger<PayslipController> logger = null)
        {
            _logger = logger;
            _payslipService = payslipService;
        }

        /// <summary>
        /// Get monthly pay slip given the taxable income and the tax rate type
        /// </summary>
        /// <param name="model">The parameters for getting the monthly pay slip, including taxable income and tax rate type. </param>
        /// <returns>The monthly pay slip</returns>
        [HttpPost]
        [Route("MonthlyPayslip")]
        public IActionResult MonthlyPaySlip([FromBody]RequestMonthlyPayslipDto model)
        {
            if (model.TaxRateType != Enums.TaxRateType.ResidentTaxRate)
            {
                ModelState.AddModelError("MonthlyPaySlipViewModel.TaxRateType", $"Tax rate type: {model.TaxRateType} is not supported now.");
                return BadRequest(ModelState);
            }

            try
            {
                var monthlyPaySlip = _payslipService.GenerateMonthlyPayslip(model.TaxableIncome, model.TaxRateType);

                return new ObjectResult(monthlyPaySlip);
            }
            catch (Exception ex)
            {
                LogInformation($"Exception happened when calling the monthly pay slip service. Exception: {ex.Message}. ");
                return StatusCode(500);
            }
        }

        private void LogInformation(string info)
        {
            if (_logger != null)
                _logger.LogInformation(info);
        }
    }
}
