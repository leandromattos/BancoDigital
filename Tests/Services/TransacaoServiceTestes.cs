using Moq;
using BancoDigitalAPI.Models;
using BancoDigitalAPI.Repositories;
using BancoDigitalAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Testes.Services
{
    public class TransacaoServiceTestes
    {
        private readonly Mock<ITransacaoRepository> _transacaoRepositoryMock;
        private readonly Mock<IContaRepository> _contaRepositoryMock;
        private readonly TransacaoService _transacaoService;

        public TransacaoServiceTestes()
        {
            _transacaoRepositoryMock = new Mock<ITransacaoRepository>();
            _contaRepositoryMock = new Mock<IContaRepository>();

            _transacaoService = new TransacaoService(
                _transacaoRepositoryMock.Object,
                _contaRepositoryMock.Object
            );
        }

        [Fact]
        public async Task DepositarAsync_ContaNaoEncontrada()
        {
            var transacaoDTO = new TransacaoDTO
            {
                ContaId = 1,
                Valor = 100
            };

            _contaRepositoryMock.Setup(r => r.ObterContaPorIdAsync(transacaoDTO.ContaId)).ReturnsAsync((Conta)null);

            var result = await _transacaoService.DepositarAsync(transacaoDTO);

            var notFoundResult = Assert.IsType<NotFound<string>>(result); // Tipo correto para "Not Found"
            Assert.Equal("Conta não encontrada.", notFoundResult.Value); // Verifica a mensagem
        }

        [Fact]
        public async Task DepositarAsync_ValorZero()
        {
            var transacaoDTO = new TransacaoDTO
            {
                ContaId = 1,
                Valor = 0
            };

            var conta = new Conta { Id = 1, Saldo = 500 };
            _contaRepositoryMock.Setup(r => r.ObterContaPorIdAsync(transacaoDTO.ContaId)).ReturnsAsync(conta);

            var result = await _transacaoService.DepositarAsync(transacaoDTO);

            var badRequest = Assert.IsType<BadRequest<string>>(result); // Tipo correto para "Bad Request"
            Assert.Equal("O valor do depósito deve ser maior que 0.", badRequest.Value); // Verifica a mensagem
        }

        [Fact]
        public async Task DepositarAsync_OK()
        {
            // Arrange
            var transacaoDTO = new TransacaoDTO { ContaId = 1, Valor = 100 };
            var transacao = new Transacao { ContaId = 1, Valor = 100, Tipo = "Deposito", DataHora = DateTime.UtcNow };
            var conta = new Conta { Id = 1, Saldo = 500 };

            _contaRepositoryMock.Setup(r => r.ObterContaPorIdAsync(transacaoDTO.ContaId)).ReturnsAsync(conta);
            _transacaoRepositoryMock.Setup(r => r.DepositarSacarAsync(It.IsAny<Transacao>()))
                .ReturnsAsync(Results.Ok(transacao)); // Resultado esperado com a transação

            // Act
            var result = await _transacaoService.DepositarAsync(transacaoDTO);

            _contaRepositoryMock.Verify(r => r.ObterContaPorIdAsync(transacaoDTO.ContaId), Times.Once);

            var okResult = Assert.IsType<Ok<Transacao>>(result); // Tipo correto para "Ok<Transacao>"
            Assert.Equal(200, okResult.StatusCode); // Verifica se o valor é o esperado
            Assert.Equal(600, conta.Saldo); // Saldo atualizado para 600
        }

        [Fact]
        public async Task SacarAsync_ContaNaoEncontrada()
        {
            var transacaoDTO = new TransacaoDTO
            {
                ContaId = 1,
                Valor = 100
            };

            _contaRepositoryMock.Setup(r => r.ObterContaPorIdAsync(transacaoDTO.ContaId)).ReturnsAsync((Conta)null);

            var result = await _transacaoService.SacarAsync(transacaoDTO);

            var notFoundResult = Assert.IsType<NotFound<string>>(result); // Tipo correto para "Not Found"
            Assert.Equal("Conta não encontrada.", notFoundResult.Value);
        }

        [Fact]
        public async Task SacarAsync_SaldoInsuficiente()
        {
            var transacaoDTO = new TransacaoDTO
            {
                ContaId = 1,
                Valor = 1000
            };
            var conta = new Conta { Id = 1, Saldo = 500 };
            _contaRepositoryMock.Setup(r => r.ObterContaPorIdAsync(transacaoDTO.ContaId)).ReturnsAsync(conta);

            var result = await _transacaoService.SacarAsync(transacaoDTO);

            var badRequest = Assert.IsType<BadRequest<string>>(result); // Tipo correto para "Bad Request"
            Assert.Equal("Saldo insuficiente.", badRequest.Value);
        }

        [Fact]
        public async Task SacarAsync_OK()
        {
            var transacaoDTO = new TransacaoDTO
            {
                ContaId = 1,
                Valor = 100
            };
            var conta = new Conta { Id = 1, Saldo = 500 };
            _contaRepositoryMock.Setup(r => r.ObterContaPorIdAsync(transacaoDTO.ContaId)).ReturnsAsync(conta);
            _transacaoRepositoryMock.Setup(r => r.DepositarSacarAsync(It.IsAny<Transacao>())).ReturnsAsync(Results.Ok(new Transacao()));

            var result = await _transacaoService.SacarAsync(transacaoDTO);

            _contaRepositoryMock.Verify(r => r.ObterContaPorIdAsync(transacaoDTO.ContaId), Times.Once);

            var okResult = Assert.IsType<Ok<Transacao>>(result); // Tipo correto para "Ok<Transacao>"
            Assert.Equal(200, okResult.StatusCode); // Verifica se o status é 200 OK
            Assert.Equal(400, conta.Saldo); // Saldo atualizado para 400
        }

        [Fact]
        public async Task GerarRelatorioAsync_DataInvalida()
        {
            var dataInicio = "invalid-date";
            var dataFim = "01/01/2025";

            var result = await _transacaoService.GerarRelatorioAsync(dataInicio, dataFim);

            var badRequest = Assert.IsType<BadRequest<string>>(result); // Tipo correto para "Bad Request"
            Assert.Equal("As datas devem estar no formato dd/MM/yyyy.", badRequest.Value);
        }

        [Fact]
        public async Task GerarRelatorioAsync_OK()
        {
            var dataInicio = "01/01/2025";
            var dataFim = "31/12/2025";

            var transacoes = new List<Transacao>
            {
                new Transacao { Tipo = "Deposito", Valor = 100, DataHora = DateTime.UtcNow },
                new Transacao { Tipo = "Saque", Valor = 50, DataHora = DateTime.UtcNow }
            };

            _transacaoRepositoryMock.Setup(r => r.GerarRelatorioAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(Results.Ok(new RelatorioDTO { TotalDepositos = 100, TotalSaques = 50 }));

            var result = await _transacaoService.GerarRelatorioAsync(dataInicio, dataFim);

            var okResult = Assert.IsType<Ok<RelatorioDTO>>(result); // Tipo correto para "Ok<RelatorioDTO>"
            Assert.Equal(200, okResult.StatusCode); // Verifica se o status é 200 OK
        }

        [Fact]
        public async Task ListarTransacoesAsync_PaginaInvalida()
        {
            var contaId = 1;
            var page = 0;  // Página inválida
            var pageSize = 10;

            var result = await _transacaoService.ListarTransacoesAsync(contaId, page, pageSize);

            var badRequest = Assert.IsType<BadRequest<string>>(result); // Tipo correto para "Bad Request"
            Assert.Equal("Os parâmetros 'page' e 'pageSize' devem ser maiores que zero.", badRequest.Value);
        }

        public class TransacaoListResult
        {
            public List<object> Transacoes { get; set; }
            public int TotalRegistros { get; set; }
        }

        [Fact]
        public async Task ListarTransacoesAsync_OK()
        {
            var contaId = 1;
            var page = 1;
            var pageSize = 10;

            _transacaoRepositoryMock.Setup(r => r.ListarTransacoesAsync(contaId, page, pageSize))
                .ReturnsAsync(Results.Ok(new TransacaoListResult { Transacoes = new List<object>(), TotalRegistros = 1 }));

            var result = await _transacaoService.ListarTransacoesAsync(contaId, page, pageSize);

            var okResult = Assert.IsType<Ok<TransacaoListResult>>(result); // Tipo correto para "Ok<TransacaoListResult>"
            Assert.Equal(200, okResult.StatusCode); // Verifica se o status é 200 OK
            Assert.Equal(1, okResult.Value.TotalRegistros); // Verifica se o total de registros está correto
        }

    }
}
