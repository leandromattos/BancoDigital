using BancoDigitalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BancoDigitalAPI.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly BancoContext _context;

        public ContaRepository(BancoContext context)
        {
            _context = context;
        }

        public async Task<Conta> ObterContaPorIdAsync(int id)
        {
            return await _context.Contas.FindAsync(id);
        }

        public async Task<IEnumerable<Conta>> ListarContasAsync()
        {
            return await _context.Contas.ToListAsync();
        }

        public async Task<IResult> AdicionarContaAsync(Conta conta)
        {
            try
            {
                _context.Contas.Add(conta);
                await _context.SaveChangesAsync();
                return Results.Ok(conta);
            }
            catch (DbUpdateException ex) // Erros relacionados ao banco de dados
            {
                return Results.BadRequest(new { Error = "Erro ao salvar a conta no banco de dados.", Details = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception) // Outros erros inesperados
            {
                return Results.BadRequest("Ocorreu um erro inesperado ao criar a conta.");
            }
        }


        public async Task<Conta> ObterContaPorCpfAsync(string cpf)
        {
            //garantir que o CPF é único antes de criar uma nova conta
            return await _context.Contas
                .FirstOrDefaultAsync(c => c.CPF == cpf);
        }
    }
}
