using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using threadpilot;
using threadpilot.models.vehicle;
using Xunit;

namespace threadPilotVehicleInfo.Tests.viewController
{
    public class VehicleInfoTests
    {
        private VehicleInfo CreateVehicleInfoWithMockedEnv()
        {
            // Set environment variable to "dev" to enable mocked vehicles
            Environment.SetEnvironmentVariable("env", "dev");
            var loggerMock = new Mock<ILogger<VehicleInfo>>();
            return new VehicleInfo(loggerMock.Object);
        }

        private HttpRequest CreateHttpRequest()
        {
            var context = new DefaultHttpContext();
            return context.Request;
        }

        [Fact]
        public void Run_ReturnsOkObjectResult_WhenVehicleExists()
        {
            // Arrange
            var vehicleInfo = CreateVehicleInfoWithMockedEnv();
            var request = CreateHttpRequest();
            string plateNumber = "ABC123";

            // Act
            var result = vehicleInfo.Run(request, plateNumber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var vehicle = Assert.IsType<Vehicle>(okResult.Value);
            Assert.Equal("ABC123", vehicle.RegistrationNumber);
            Assert.Equal("Toyota Camry", vehicle.Make);
        }

        [Fact]
        public void Run_ReturnsNotFoundObjectResult_WhenVehicleDoesNotExist()
        {
            // Arrange
            var vehicleInfo = CreateVehicleInfoWithMockedEnv();
            var request = CreateHttpRequest();
            string plateNumber = "ZZZ999";

            // Act
            var result = vehicleInfo.Run(request, plateNumber);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("No vehicles found", notFoundResult.Value.ToString());
        }
    }
}