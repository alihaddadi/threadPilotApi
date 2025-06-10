using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using threadpilot.models.insurance;


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

        [Function("getInsuranceInfo")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "insuranceinfo")] HttpRequest req)
        {
            _logger.LogInformation("POST:{URL} method:{handlerName}", req.GetDisplayUrl(), this.ToString());

            // Read POST body
            string requestBody;
            using (var reader = new StreamReader(req.Body))
            {
                requestBody =  reader.ReadToEndAsync().GetAwaiter().GetResult();
            }

            // Example: parse JSON body to get 'platenumber'
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
                return new NotFoundObjectResult("No customers found with the given social security number");
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
