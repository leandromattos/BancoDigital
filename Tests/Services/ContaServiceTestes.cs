using Moq;
using BancoDigitalAPI.Services;
using BancoDigitalAPI.Repositories;
using BancoDigitalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

public class ContaServiceTests
{
    private readonly Mock<IContaRepository> _contaRepositoryMock;
    private readonly Mock<ICPFValidatorService> _cpfValidatorServiceMock;
    private readonly ContaService _contaService;

    public ContaServiceTests()
    {
        _contaRepositoryMock = new Mock<IContaRepository>();
        _cpfValidatorServiceMock = new Mock<ICPFValidatorService>();

        _contaService = new ContaService(
            _contaRepositoryMock.Object,
            _cpfValidatorServiceMock.Object
        );
    }

    [Fact]
    public async Task CriarContaAsync_DeveRetornarErroSeCPFInvalido()
    {
        // Arrange
        var contaDTO = new CriarContaDTO { NomeCliente = "João", CPF = "12345678900", SaldoInicial = 100 };
        _cpfValidatorServiceMock.Setup(x => x.ValidarCPFAsync(contaDTO.CPF))
            .ReturnsAsync(new CPFValidationResult { Valid = false });

        // Act
        var result = await _contaService.CriarContaAsync(contaDTO);

        // Assert
        var badRequest = Assert.IsType<BadRequest<string>>(result);
        Assert.Equal("O CPF informado não é válido.", badRequest.Value);
    }

    [Fact]
    public async Task CriarContaAsync_DeveRetornarErroSeCPFJaExistente()
    {
        // Teste
        var contaDTO = new CriarContaDTO { NomeCliente = "Maria", CPF = "11122233344", SaldoInicial = 200 };
        _cpfValidatorServiceMock.Setup(x => x.ValidarCPFAsync(contaDTO.CPF))
            .ReturnsAsync(new CPFValidationResult { Valid = true });

        _contaRepositoryMock.Setup(x => x.ObterContaPorCpfAsync(contaDTO.CPF))
            .ReturnsAsync(new Conta { NomeCliente = "Maria", CPF = "11122233344", Saldo = 200 });

        // exec
        var result = await _contaService.CriarContaAsync(contaDTO);

        // Assert
        var badRequest = Assert.IsType<BadRequest<string>>(result);
        Assert.Equal("Já existe uma conta cadastrada com este CPF.", badRequest.Value);
    }

    [Fact]
    public async Task CriarContaAsync_DeveCriarContaSeDadosForemValidos()
    {
        // Teste
        var contaDTO = new CriarContaDTO { NomeCliente = "Carlos", CPF = "44455566677", SaldoInicial = 500 };

        _cpfValidatorServiceMock.Setup(x => x.ValidarCPFAsync(contaDTO.CPF))
            .ReturnsAsync(new CPFValidationResult { Valid = true });

        _contaRepositoryMock.Setup(x => x.ObterContaPorCpfAsync(contaDTO.CPF))
            .ReturnsAsync((Conta)null); // CPF não cadastrado

        _contaRepositoryMock.Setup(x => x.AdicionarContaAsync(It.IsAny<Conta>()))
            .ReturnsAsync(Results.Ok(new Conta { NomeCliente = "Carlos", CPF = "44455566677", Saldo = 500 }));

        // Act
        var result = await _contaService.CriarContaAsync(contaDTO);

        // Assert
        var okResult = Assert.IsType<Ok<Conta>>(result);
        Assert.Equal("Carlos", okResult.Value.NomeCliente);
        Assert.Equal(500, okResult.Value.Saldo);
    }
}
