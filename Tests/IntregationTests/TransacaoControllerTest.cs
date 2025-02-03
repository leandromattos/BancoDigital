using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using BancoDigitalAPI.Models;

namespace BancoDigitalAPI.Tests.Controllers
{
    using System.Net.Http.Json;
    using BancoDigitalAPI;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    public class TransacaoControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        // O WebApplicationFactory cria um servidor em memória para rodar os testes de integração
        public TransacaoControllerTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(); // Cria o cliente HTTP para fazer requisições
        }
        
        [Fact]
        public async Task DepositarAsync_OK()
        {
            var transacaoDTO = new TransacaoDTO { ContaId = 1, Valor = 100 };

            // Faz a requisição POST para o endpoint de depósito
            var response = await _client.PostAsJsonAsync("/api/Transacoes/depositar", transacaoDTO);

            // Verifica se a resposta foi bem-sucedida
            response.EnsureSuccessStatusCode();

            var transacao = await response.Content.ReadFromJsonAsync<Transacao>(); // Obtém a transação retornada
            Assert.NotNull(transacao); // Verifica se a transação foi retornada
            Assert.Equal(100, transacao.Valor); // Verifica se o valor depositado é o correto
        }

        [Fact]
        public async Task SacarAsync_OK()
        {
            var transacaoDTO = new TransacaoDTO { ContaId = 1, Valor = 100 };

            // Faz a requisição POST para o endpoint de saque
            var response = await _client.PostAsJsonAsync("/api/Transacoes/sacar", transacaoDTO);

            // Verifica se a resposta foi bem-sucedida
            response.EnsureSuccessStatusCode();

            var transacao = await response.Content.ReadFromJsonAsync<Transacao>(); // Obtém a transação retornada
            Assert.NotNull(transacao); // Verifica se a transação foi retornada
            Assert.Equal(100, transacao.Valor); // Verifica se o valor do saque é o correto
        }
    }

}
