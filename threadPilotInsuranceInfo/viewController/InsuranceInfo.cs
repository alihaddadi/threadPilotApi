using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using threadPilotModel;


namespace threadpilot
{
    public class InsuranceInfo
    {
        private readonly ILogger<InsuranceInfo> _logger;
        private List<Insurance> mockedInsurances;

        public InsuranceInfo(ILogger<InsuranceInfo> logger)
        {
            _logger = logger;
            var environmentName = Environment.GetEnvironmentVariable("env");
            if(environmentName == "dev")
                mockedInsurances = this.getMockedInsurances();

        }

        [Function("getInsurance")]
        [OpenApiOperation(operationId: "getInsuranceInfo")]
       // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody("application/json", typeof(InsuranceRequestModel),
                   Description = "JSON request body containing { \"ssn\" : \"social security number\"}")]

        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Vehicle),
            Description = "The OK response returns insurance information.")]

        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "application/json", bodyType: typeof(_404_Response),
            Description = "The 404 response returns an error code and message.")]

        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "insuranceinfo")] HttpRequest req)
        {
            _logger.LogInformation("POST:{URL} method:{handlerName}", req.GetDisplayUrl(), this.ToString());

            // Read POST body
            // Note: In a real-world scenario, you should use asynchronous methods to read the body.
            // Here we use synchronous reading for simplicity, but it's not recommended in production code.
            string requestBody;
            using (var reader = new StreamReader(req.Body))
            {
                requestBody =  reader.ReadToEndAsync().GetAwaiter().GetResult();
            }

            // Example: parse JSON body to get 'platenumber'
            // if (string.IsNullOrEmpty(requestBody))
            // 
            {
                return new BadRequestObjectResult("Request body is empty");
            }
            string ssn = "";
            if (!string.IsNullOrEmpty(requestBody))
            {
                var json = System.Text.Json.JsonDocument.Parse(requestBody);
                if (json.RootElement.TryGetProperty("SSN", out var SSN))
                {
                    ssn = SSN.GetString();
                }
            }

            var results = mockedInsurances.FirstOrDefault(v => v.SSN == ssn);
            if (results == null)
            {
                return new NotFoundObjectResult(new _404_Response(err_msg: $"No insurance found with ssn: {ssn}", err_cod: 1));
            }

            return new OkObjectResult(new Insurance
                   (results.SSN,
                    results.InsuranceHolderName,
                    results.InsurancePrice,
                    results.InsuranceType,
                    results.VehicleRegistrationNumber)
                   );
        }

        private List<Insurance> getMockedInsurances()
        {
            return new List<Insurance> {
                new Insurance("123456789", "John Doe", 10  , "Pet", ""),
                new Insurance("987654321", "Jane Smith", 20 , "Personal Health", ""),
                new Insurance("555555555", "Alice Johnson", 30 , "Car", "LMN456"),
                new Insurance("111223333", "Bob Brown", 10, "Pet", ""),
                new Insurance("444444444", "Charlie White", 20 , "Personal Health", ""),
                new Insurance("222334444", "Diana Green", 30 , "Car", "WXY987"),
                new Insurance("333445555", "Ethan Blue", 10 , "Pet", ""),
                new Insurance("666778888", "Fiona Black", 20 , "Personal Health", "")
            };

        }
    }
}
