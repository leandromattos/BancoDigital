using BancoDigitalAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BancoDigitalAPI.Services
{
    public interface ITransacaoService
    {
        Task<IResult> DepositarAsync(TransacaoDTO transacaoDTO);        
        Task<IResult> SacarAsync(TransacaoDTO transacaoDTO);
        Task<IResult> ListarTransacoesAsync(int contaID, int page, int pageSize);
        Task<IResult> GerarRelatorioAsync(string dataInicio, string dataFim);

    }
}
