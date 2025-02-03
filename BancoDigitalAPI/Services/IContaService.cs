using BancoDigitalAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BancoDigitalAPI.Services
{
    public interface IContaService
    {
        Task<IResult> CriarContaAsync(CriarContaDTO contaDTO);
        Task<Conta> ObterContaPorIdAsync(int id);
        Task<IEnumerable<Conta>> ListarContasAsync();
    }
}
