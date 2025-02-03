using BancoDigitalAPI.Models;
using System;

namespace BancoDigitalAPI.Repositories
{
    public interface ITransacaoRepository
    {
        Task<IResult> DepositarSacarAsync(Transacao transacao);

        Task<IResult> ListarTransacoesAsync(int ContaId, int page, int pageSize);

        Task<IResult> GerarRelatorioAsync(DateTime inicio, DateTime fim);
    }
}
