using Microsoft.AspNetCore.Mvc.Testing;

namespace BancoDigital.Tests
{
    public class HealthCheckTests : IClassFixture<WebApplicationFactory<BancoDigitalAPI.Program>>
    {
        private readonly HttpClient _client;

        public HealthCheckTests(WebApplicationFactory<BancoDigitalAPI.Program> factory)
        {
            // Inicializa o HttpClient para fazer requisições para a API
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task HealthCheck_Endpoint_OK()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            response.EnsureSuccessStatusCode(); // Verifica se o status é 2xx
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Healthy", responseContent); // Verifica se o conteúdo contém "Healthy"
        }

        // Teste para o Endpoint
        [Fact]
        public async Task Swagger_Endpoint_OK()
        {
            // Act
            var response = await _client.GetAsync("/swagger");

            // Assert
            response.EnsureSuccessStatusCode(); // Verifica se o status é 2xx
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Swagger", responseContent); // Verifica se o conteúdo contém "Swagger"
        }
    }
}
