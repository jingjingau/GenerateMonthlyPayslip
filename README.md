# GenerateMonthlyPayslip Solution ReadMe
## 1. About
	A console application that given employee annual salary details, outputs a monthly pay slip. 

## 2. How

### 2.1 Technologies

This Solution was developed in Visual Studio 2019 with .Net Core 5.0, Entity Framework core 5.0, SQL Server Express LocalDB, ASP.NET REST WEB API, C# 8, xUnit, Moq, .Net Core logging 

### 2.2 Projects

There are 4 projects in the solution:

#### Payslip.API
	The REST Web API service project. 
	
#### Payslip.API.Tests 
	The unit test for Payslip.API
	
#### GenerateMonthlyPayslip 
	The client console project which calls the API to get the monthly payslip. 
	
#### GenerateMonthlyPayslip.Tests 
	The unit test for GenerateMonthlyPayslip.

### 2.3 How to run

#### A.  When you run the Payslip.API project for the first time, database will be created and some sample data will be inserted into the database automatically by code through the code first process.

#### B.  The database connection settings is located at the following position. You could change it according to your local environment. 
    Payslip.API\appsettings.json
	
#### C.  Run the Payslip.API project and GenerateMonthlyPayslip project at the same time. Test it from the client console. 

#### D.  Test Payslip.API using Postman or Swagger:
	postman endpoint: 
	               Post: https://localhost:44317/Payslip/MonthlyPayslip
				Body: { "taxableIncome": 60000,  "taxRateType": 1 }
		
	Swagger: https://localhost:44317/swagger/index.html

## 3. Solution Design
 
Software design principle: SOLID

### 3.1  Payslip.API Project

Use Payslip.API as the API service to supply API end point of MonthlyPayslip, which could return the monthly payslip when giving taxable income and tax rate type (currently only TaxRateType: ResidentTaxRate=1 is supported). 

#### 3.1.1 DB Design

My assumption is that there are maybe several tax rate types (Payslip.API\Enums\TaxRateType.cs)

TaxRateType: ResidentTaxRate=1, ForeignResidentTaxRate=2, ChildrenTaxRate=3, WorkingHolidayMakerTaxRate=4

For the DB design, the tax rate is configurable according to different tax rate types and financial years.

There are two DB models (Payslip.API\Models): TaxRateLevel and TaxRate.

TaxRateLevel: Id, TaxRateTypeInternal, Level, TaxableIncomeLowerBound, TaxableIncomeUpperBound 

TaxRate: Id, FinancialYearStart, FinancialYearEnd, TaxRateLevelId, Rate

#### 3.1.2 Design Pattern 

#### 1)  Strategy Pattern:

For each TaxRateType, there is a strategy to compute the monthly payslip. Currently, only resident tax rate type strategy is implemented. Through this way, it is easy to extend the code for other tax rate types without changing current code (SOLID principle: open-close principle). 

Please look at the following files:

Payslip.API\Strategies\BaseTaxCalculateStrategy.cs

Payslip.API\Strategies\ResidentIncomeTaxCalculateStrategy.cs

#### 2)  Chain Of Responsibility Pattern:

For each tax rate level of a given tax rate type, there is a specific class to compute the tax within this tax rate level. In this way, it is easy to extend the code if there are more tax rate levels in the future, without changing current code. 

Please look at the following files:

Payslip.API\ChainOfResponsibility\BaseTaxRateCalculation.cs

Payslip.API\ChainOfResponsibility\ResidentIncomeTaxCalculation\TaxRateLevelOneTaxCalculation.cs

Payslip.API\ChainOfResponsibility\ResidentIncomeTaxCalculation\TaxRateLevelTwoTaxCalculation.cs

Payslip.API\ChainOfResponsibility\ResidentIncomeTaxCalculation\TaxRateLevelThreeTaxCalculation.cs

Payslip.API\ChainOfResponsibility\ResidentIncomeTaxCalculation\TaxRateLevelFourTaxCalculation.cs

Payslip.API\ChainOfResponsibility\ResidentIncomeTaxCalculation\TaxRateLevelFiveTaxCalculation.cs

#### 3)  Singleton Pattern:

All those tax calculation classes are implemented using Singleton Pattern. 

#### 3.1.3  Services

The API controller calls the PayslipService to get the monthly payslip. PayslipService calls the concrete TaxCalculateStrategy to calculate the tax according to the tax rate type. 

Please look at the following file:

Payslip.API\Services\PayslipService.cs

#### 3.1.4  API controller

The API controller supplies one API end point: 

        [HttpPost]
        [Route("MonthlyPayslip")]
        public IActionResult MonthlyPayslip([FromBody]RequestMonthlyPayslipDto model)

Please look at the file: 

Payslip.API\Controllers\PayslipController.cs

The input parameters are validated using the data annotation validation:

    public class RequestMonthlyPayslipDto
    {
        /// <summary>
        /// Taxable Income
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public decimal TaxableIncome { get; set; }

        /// <summary>
        /// Tax Rate Type
        /// </summary>
        [EnumDataType(typeof(TaxRateType))]
        [Required]
        public TaxRateType TaxRateType { get; set; }
    }


### 3.2  GenerateMonthlyPayslip Project: 

Project GenerateMonthlyPayslip is a console client project which can get the user input, do some validation, call the API and return the result. 

#### 3.2.1 User Input and output:

GenerateMonthlyPayslip\Program.cs

#### 3.2.2 Input parameters validation:

GenerateMonthlyPayslip\ParametersValidation.cs

#### 3.2.3 Monthly Payslip Generator

Call the API service agent to get the result:

GenerateMonthlyPayslip\MonthlyPayslipGenerator.cs

#### 3.2.4 API Service Agent: 

Call the API to get the monthly pay slip. 

GenerateMonthlyPayslip\ServiceAgents\MonthlyPayslipServiceAgent.cs

### 3.3  Payslip.API.Tests Project

This is the test project for Payslip.API.

In order to mock the db data, I created one fake service: PayslipService_Fake

### 3.4  GenerateMonthlyPayslip.Tests Project

This is the test project for GenerateMonthlyPayslip project. Use Moq to mock the API call. 


## 4. Assumptions & Caveats

### 4.1 TaxRateType

There are several Tax Rate Types.

TaxRateType: ResidentTaxRate=1, ForeignResidentTaxRate=2, ChildrenTaxRate=3, WorkingHolidayMakerTaxRate=4

Currently only: TaxRateType=1 (ResidentTaxRate) is implemented.

### 4.2 Financial Year

Each financial year has different tax rates for each tax rate type. Currently, the tax is only calculated according to the current financial year's tax rates giving the tax rate type. 

### 4.3 API Authentication and Authorization

In the future, API authentication and authorization need to be added by using the OpenID connection through IdentityServer4. 


## 5. Github Link

https://github.com/jingjingau/GenerateMonthlyPayslip

