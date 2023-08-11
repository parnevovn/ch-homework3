using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Route256.PriceCalculator.Api.Controllers;
using Route256.PriceCalculator.Api.Requests.V3;
using Route256.PriceCalculator.Domain.Exceptions;
using Xunit.Abstractions;

namespace PriceCalculator.IntegrationTests
{
    public class BasicTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public BasicTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task App_SwaggerIsWorking()
        {
            // Arrange
            var app = new AppFixture();
            var httpClient = app.CreateClient();

            // Act
            var responce = await httpClient.GetAsync(requestUri: "/swagger/index.html");

            // Assert
            responce.EnsureSuccessStatusCode();
            _testOutputHelper.WriteLine(await responce.Content.ReadAsStringAsync());
        }

        [Fact]
        public void App_V3DeliveryPrice_GoodCalculate_ShouldReturnResult()
        {
            // Arrange
            var app = new AppFixture();
            var request = new GoodCalculateRequest(1, 1000);
            var controller = app.Services.GetRequiredService<V3DeliveryPriceController>();
       
            // Act
            var response = controller.Calculate(request);

            // Assert
            Assert.True(response.Result.Price != 0);
        }

        [Fact]
        public void App_V3DeliveryPrice_GoodCalculate_WhenIdIsEmpty_ShouldThrow()
        {
            // Arrange
            var app = new AppFixture();
            var request = new GoodCalculateRequest(0, 1000);
            var controller = app.Services.GetRequiredService<V3DeliveryPriceController>();

            // Act
            // Assert
            Assert.ThrowsAsync<DomainException>(() => controller.Calculate(request));
        }

        [Fact]
        public void App_V3DeliveryPrice_GoodCalculate_WhenDistanceIsEmpty_ShouldThrow()
        {
            // Arrange
            var app = new AppFixture();
            var request = new GoodCalculateRequest(1, 0);
            var controller = app.Services.GetRequiredService<V3DeliveryPriceController>();

            // Act
            // Assert
            Assert.ThrowsAsync<DomainException>(() => controller.Calculate(request));
        }
    }
}