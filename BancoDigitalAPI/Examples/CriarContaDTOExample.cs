using BancoDigitalAPI.Models;
using Swashbuckle.AspNetCore.Filters;

namespace BancoDigitalAPI.Swagger.Examples
{
    public class CriarContaDTOExample : IExamplesProvider<CriarContaDTO>
    {
        public CriarContaDTO GetExamples()
        {
            return new CriarContaDTO
            {
                NomeCliente = "João da Silva",
                CPF = "123.456.789-10",
                SaldoInicial = 100.00m
            };
        }
    }
}
