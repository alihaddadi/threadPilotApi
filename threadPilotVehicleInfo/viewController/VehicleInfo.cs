using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;
using threadpilot.models.vehicle;

namespace threadpilot
{
    public class VehicleInfo
    {
        private readonly ILogger<VehicleInfo> _logger;
        private List<Vehicle> mockedVehicles;

        public VehicleInfo(ILogger<VehicleInfo> logger)
        {
            _logger = logger;
            var environmentName = Environment.GetEnvironmentVariable("env");
            if(environmentName == "dev")
                mockedVehicles = this.getMockedVehicles();

        }

        [Function("getVehicleInfo")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "getvehicle/{platenumber}")] HttpRequest req, string platenumber)
        {
            _logger.LogInformation("GET:{URL} method:{handlerName}", req.GetDisplayUrl(), this.ToString());
            var results = mockedVehicles.FirstOrDefault(v => v.RegistrationNumber == platenumber);
            if (results == null)
            {
                return new NotFoundObjectResult($"No vehicles found with plate number: {platenumber}");
            }
            return new OkObjectResult(new Vehicle
                   (results.RegistrationNumber,
                    results.Model,
                    results.Make,
                    results.ChassiNumber)
                   );
        }

        private List<Vehicle> getMockedVehicles()
        {
            return new List<Vehicle>
        {
            new Vehicle("ABC123", "2020", "Toyota Camry", "1HGBH41JXMN109186"),
            new Vehicle("DEF456", "2021", "Honda Accord", "1HGCM82633A123456"),
            new Vehicle("LMN456", "2022", "Ford Focus", "1FADP3F23JL123456"),
            new Vehicle("JKL012", "2023", "Chevrolet Malibu", "1G1ZB5ST8JF123456"),
            new Vehicle("MNO345", "2024", "Nissan Altima", "1N4AL3APXEC123456"),
            new Vehicle("PQR678", "2025", "Hyundai Sonata", "5NPE24AF2FH123456"),
            new Vehicle("STU901", "2026", "Kia Optima", "5XXGT4L39GG123456"),
            new Vehicle("VWX234", "2027", "Subaru Legacy", "4S3BNAC6XJ123456"),
            new Vehicle("YZA567", "2028", "Mazda6", "JM1GJ1U67J123456"),
            new Vehicle("BCD890", "2029", "Volkswagen Passat", "1VWAA7A3XHC123456"),
            new Vehicle("EFG123", "2030", "Audi A4", "WAUDF78E58A123456")
        };
        }
    }
}
