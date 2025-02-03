using BancoDigitalAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BancoDigitalAPI.Repositories
{
    public interface IContaRepository
    {
        Task<Conta> ObterContaPorIdAsync(int id);
        Task<IEnumerable<Conta>> ListarContasAsync();
        Task<IResult> AdicionarContaAsync(Conta conta);
        Task<Conta> ObterContaPorCpfAsync(string cpf);
    }
}
