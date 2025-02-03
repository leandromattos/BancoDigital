using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDigitalAPI.Models;
using System.Globalization;
using BancoDigitalAPI.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace BancoDigitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly BancoContext _context;
        private readonly ITransacaoService _transacaoService;

        public TransacoesController(BancoContext context, ITransacaoService transacaoService)
        {
            _context = context;
            _transacaoService = transacaoService;
        }

        // POST: api/transacoes/depositar
        [HttpPost("depositar")]
        [SwaggerOperation(Summary = "Realiza depósito em uma conta bancária.")]
        public async Task<IResult> Depositar([FromBody] TransacaoDTO transacaoDTO)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            var result = await _transacaoService.DepositarAsync(transacaoDTO);
            return result;
        }

        // POST: api/transacoes/sacar
        [HttpPost("sacar")]
        [SwaggerOperation(Summary = "Realiza saque em uma conta bancária.")]
        public async Task<IResult> Sacar([FromBody] TransacaoDTO transacaoDTO)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            var result = await _transacaoService.SacarAsync(transacaoDTO);
            return result;
        }

        // GET: api/transacoes/{contaId}
        [HttpGet("historico/{contaId}")]
        [SwaggerOperation(Summary = "Histórico de transações(saques/depósitos) em uma conta bancária.")]
        public async Task<IResult> ListarHistoricoTransacoes(
            int contaId, 
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            var result = await _transacaoService.ListarTransacoesAsync(contaId, page, pageSize);
            return result;
        }

        // GET: api/transacoes/relatorio
        [HttpGet("relatorio")]
        [SwaggerOperation(Summary = "Relatório com o total de saque(s)/depósito(s) realizado(s) em uma conta bancária.")]
        public async Task<IResult> GerarRelatorio([FromQuery] RelatorioRequest request)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            var result = await _transacaoService.GerarRelatorioAsync(request.DataInicio, request.DataFim);
            return result;
        }

    }
}
