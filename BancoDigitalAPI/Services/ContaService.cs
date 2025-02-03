using BancoDigitalAPI.Models;
using BancoDigitalAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoDigitalAPI.Services
{
    public class ContaService : IContaService
    {
        private readonly IContaRepository _contaRepository;
        private readonly ICPFValidatorService _cpfValidatorService;

        public ContaService(IContaRepository contaRepository, ICPFValidatorService cpfValidatorService)
        {
            _contaRepository = contaRepository;
            _cpfValidatorService = cpfValidatorService;
        }

        public async Task<IResult> CriarContaAsync(CriarContaDTO contaDTO)
        {
            // 1. Validação do CPF com a API externa
            var cpfValido = await _cpfValidatorService.ValidarCPFAsync(contaDTO.CPF);
            if (!cpfValido.Valid)
            {
                return Results.BadRequest("O CPF informado não é válido.");
            }

            // 2. Validação de unicidade do CPF
            var contaExistente = await _contaRepository.ObterContaPorCpfAsync(contaDTO.CPF);
            if (contaExistente != null)
            {
                return Results.BadRequest("Já existe uma conta cadastrada com este CPF.");
            }

            // 3. Criação da conta
            var conta = new Conta
            {
                NomeCliente = contaDTO.NomeCliente,
                CPF = contaDTO.CPF,
                Saldo = contaDTO.SaldoInicial
            };

            var result = await _contaRepository.AdicionarContaAsync(conta);
            return result;
            
        }


        public async Task<Conta> ObterContaPorIdAsync(int id)
        {
            return await _contaRepository.ObterContaPorIdAsync(id);
        }

        public async Task<IEnumerable<Conta>> ListarContasAsync()
        {
            return await _contaRepository.ListarContasAsync();
        }
    }
}
