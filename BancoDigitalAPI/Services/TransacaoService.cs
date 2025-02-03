using BancoDigitalAPI.Models;
using BancoDigitalAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Globalization;

namespace BancoDigitalAPI.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IContaRepository _contaRepository;

        public TransacaoService(ITransacaoRepository transacaoRepository, IContaRepository contaRepository)
        {
            _transacaoRepository = transacaoRepository;
            _contaRepository = contaRepository;
        }

        public async Task<IResult> DepositarAsync(TransacaoDTO transacaoDTO)
        {
            var conta = await _contaRepository.ObterContaPorIdAsync(transacaoDTO.ContaId);
            if (conta == null)
                return Results.NotFound("Conta não encontrada.");

            if (transacaoDTO.Valor <= 0)
                return Results.BadRequest("O valor do depósito deve ser maior que 0.");

            conta.Saldo += transacaoDTO.Valor;

            var transacao = new Transacao
            {
                Tipo = "Deposito",
                DataHora = DateTime.UtcNow,
                Valor = transacaoDTO.Valor,
                ContaId = transacaoDTO.ContaId
            };

            var result = await _transacaoRepository.DepositarSacarAsync(transacao);
            return result;

        }

        public async Task<IResult> SacarAsync(TransacaoDTO transacaoDTO)
        {
            var conta = await _contaRepository.ObterContaPorIdAsync(transacaoDTO.ContaId);
            if (conta == null)
                return Results.NotFound("Conta não encontrada.");

            if (transacaoDTO.Valor <= 0)
                return Results.BadRequest("O valor do saque deve ser maior que 0.");

            if (conta.Saldo < transacaoDTO.Valor)
                return Results.BadRequest("Saldo insuficiente.");

            conta.Saldo -= transacaoDTO.Valor;

            var transacao = new Transacao
            {
                Tipo = "Saque",
                DataHora = DateTime.UtcNow,
                Valor = transacaoDTO.Valor,
                ContaId = transacaoDTO.ContaId
            };

            var result = await _transacaoRepository.DepositarSacarAsync(transacao);
            return result;

        }

        public async Task<IResult> GerarRelatorioAsync(string dataInicio, string dataFim)
        {
            // Verifica se as datas foram informadas
            if (string.IsNullOrEmpty(dataInicio) || string.IsNullOrEmpty(dataFim))
            {
                return Results.BadRequest("As datas de início e fim são obrigatórias.");
            }

            // Converte a string para DateTime no formato correto
            if (!DateTime.TryParseExact(dataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime inicio) ||
                !DateTime.TryParseExact(dataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fim))
            {
                return Results.BadRequest("As datas devem estar no formato dd/MM/yyyy.");
            }

            // Verifica se a data de início é anterior à data de fim
            if (inicio > fim)
            {
                return Results.BadRequest("A data de início não pode ser posterior à data de fim.");
            }

            // Ajusta para UTC
            inicio = inicio.Date.ToUniversalTime();
            fim = fim.Date.AddDays(1).AddSeconds(-1).ToUniversalTime(); // Pega até o final do dia

            return await _transacaoRepository.GerarRelatorioAsync(inicio, fim);
        }

        public async Task<IResult> ListarTransacoesAsync(int contaID, int page, int pageSize)
        {
            // Valida página e tamanho da página
            if (page <= 0 || pageSize <= 0)
                return Results.BadRequest("Os parâmetros 'page' e 'pageSize' devem ser maiores que zero.");

            return await _transacaoRepository.ListarTransacoesAsync(contaID, page, pageSize);
        }

    }
}
