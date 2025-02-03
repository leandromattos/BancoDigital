using BancoDigitalAPI.Models;
using BancoDigitalAPI.Services;
using BancoDigitalAPI.Swagger.Examples;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BancoDigitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContasController : ControllerBase
    {
        private readonly IContaService _contaService;

        public ContasController(IContaService contaService)
        {
            _contaService = contaService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cria uma nova conta bancária.")]
        [SwaggerResponse(200, "Conta criada com sucesso", typeof(Conta))]
        [SwaggerResponseExample(200, typeof(Conta))]
        [SwaggerResponse(400, "Erro de validação ou negócio", typeof(string))]        
        public async Task<IResult> CriarConta(
            [FromBody] CriarContaDTO contaDTO
        )
        {

            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            var result = await _contaService.CriarContaAsync(contaDTO);
            return result;

        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Consulta uma conta bancária através do id da conta.")]
        [SwaggerResponse(200, "Conta encontrada", typeof(Conta))]
        [SwaggerResponseExample(200, typeof(Conta))]
        [SwaggerResponse(400, "Erro de validação ou negócio", typeof(string))]
        [SwaggerResponse(404, "Conta não encontrada", typeof(string))]
        public async Task<ActionResult<Conta>> ObterContaPorId(
            [FromRoute]int id
        )
        {
            var conta = await _contaService.ObterContaPorIdAsync(id);
            if (conta == null)
                return NotFound($"Conta:{id}, não foi econtrada.");

            return Ok(conta);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Cria uma nova conta bancária.")]
        [SwaggerResponse(200, "Listagem de Contas", typeof(IEnumerable<Conta>))]
        [SwaggerResponse(400, "Erro de validação ou negócio", typeof(string))]
        public async Task<ActionResult<IEnumerable<Conta>>> ListarContas()
        {
            return Ok(await _contaService.ListarContasAsync());
        }
    }
}
