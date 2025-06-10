using NUnit.Framework;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace threadPilotInsuranceInfo.UnitTests
{
    [TestFixture]
    public class InsuranceInfoApiTests
    {
        private HttpClient _client;
        public InsuranceInfoApiTests(HttpClient client)
        {
            _client = client;
        }

        public InsuranceInfoApiTests()
        {
            // Default constructor for NUnit compatibility
        }
        [SetUp]
        public void Setup()
        {
            // Initialize HttpClient or mock as needed
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:7185/");
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose of HttpClient to avoid resource leaks
            _client.Dispose();
        }

        [Test]
        public async Task GetInsuranceInfo_ReturnsOkAndData()
        {
            // Arrange
            var requestUrl = "api/insuranceinfo/123456789"; // Adjust endpoint from the actual API project

            // Act
            var response = await _client.GetAsync(requestUrl);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(content);
      
        }

        [Test]
        public async Task PostInsuranceInfo_CreatesNewInsuranceInfo()
        {
            // Arrange
            var requestUrl = "/api/insuranceinfo";
            var requestBody = new
            {
                SSN = "123456789"
            };
            var postContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(requestUrl, postContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(content);
            // Optionally, check returned object
        }
    }
}
