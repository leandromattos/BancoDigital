using BancoDigitalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BancoDigitalAPI.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly BancoContext _context;

        public TransacaoRepository(BancoContext context)
        {
            _context = context;
        }

        public async Task<IResult> DepositarSacarAsync(Transacao transacao)
        {
            try
            {
                _context.Transacoes.Add(transacao);
                await _context.SaveChangesAsync();
                return Results.Ok(transacao);
            }
            catch (DbUpdateException ex) // Erros relacionados ao banco de dados
            {
                return Results.BadRequest(new { Error = $"Erro ao criar o {transacao.Tipo} no banco de dados.", Details = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception) // Outros erros inesperados
            {
                return Results.BadRequest($"Ocorreu um erro inesperado ao criar o {transacao.Tipo}.");
            }
        }

        public async Task<IResult> GerarRelatorioAsync(DateTime inicio, DateTime fim)
        {
            var transacoes = await _context.Transacoes
                .Where(t => t.DataHora >= inicio && t.DataHora <= fim)
                .ToListAsync();

            // Se não houver transações dentro do intervalo, retornamos uma resposta vazia
            if (!transacoes.Any())
            {
                return Results.NotFound("Nenhuma transação encontrada no intervalo especificado.");
            }

            // Calcula os totais de depósitos e saques
            var relatorio = new RelatorioDTO
            {
                TotalDepositos = transacoes.Where(t => t.Tipo == "Deposito").Sum(t => t.Valor),
                TotalSaques = transacoes.Where(t => t.Tipo == "Saque").Sum(t => t.Valor)
            };

            return Results.Ok(relatorio);
        }

        public async Task<IResult> ListarTransacoesAsync(int ContaId, int page, int pageSize)
        {
            // Busca total de transações da conta
            var query = _context.Transacoes
                .Where(t => t.ContaId == ContaId)
                .OrderByDescending(t => t.DataHora); // Ordenado da mais recente para a mais antiga


            int totalRegistros = await query.CountAsync();

            if (totalRegistros == 0)
                return Results.NotFound("Nenhuma transação encontrada para essa conta.");

            // Paginação
            var transacoes = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new
                {
                    Data = t.DataHora,
                    Tipo = t.Tipo,
                    Valor = t.Valor
                })
                .ToListAsync();

            // Retorna resposta com metadados de paginação
            var resposta = new
            {
                TotalRegistros = totalRegistros,
                PaginaAtual = page,
                TamanhoPagina = pageSize,
                TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)pageSize),
                Transacoes = transacoes
            };

            return Results.Ok(resposta);
        }
    }
}
