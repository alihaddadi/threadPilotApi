using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace unitTests
{
    public class ThreatPilotVehicleInfoApiTests
    {
        private HttpClient _client;

        public ThreatPilotVehicleInfoApiTests(HttpClient client)
        {
            _client = client;
        }
        public ThreatPilotVehicleInfoApiTests()
        {
            
        }
        [SetUp]
        public void Setup()
        {
     
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:7208/");
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose of HttpClient to release resources
            _client?.Dispose();
        }

        [Test]
        public async Task GetVehicleInfo_ReturnsSuccessStatusCode()
        {
            // Arrange
            var vehicleId = "12345";
            var requestUri = $"api/threatPilotVehicleInfo/{vehicleId}";

            // Act
            var response = await _client.GetAsync(requestUri);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "API did not return success status code.");
        }

        [Test]
        public async Task GetVehicleInfo_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidVehicleId = "invalid";
            var requestUri = $"api/threatPilotVehicleInfo/{invalidVehicleId}";

            // Act
            var response = await _client.GetAsync(requestUri);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

   
      
    }
}