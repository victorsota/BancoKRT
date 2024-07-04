using BancoKRT.API.Application.Interfaces;
using BancoKRT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BancoKRT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LimitePixApiController : ControllerBase
    {
        private readonly ILimitePixService _limitePixService;

        public LimitePixApiController(ILimitePixService limitePixService)
        {
            _limitePixService = limitePixService;
        }

        [HttpPost("AdicionarLimitePix")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> AdicionarLimitePix([FromBody] LimitePixModel limitePix)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _limitePixService.AdicionarLimitePix(limitePix);

            if (result.Contains("sucesso"))
            {
                return Ok(new { message = result });
            }
            else
            {
                return BadRequest(new { error = result });
            }
        }

        [HttpPut("AtualizarLimitePix")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarLimitePix([FromBody] LimitePixModel limitePix)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _limitePixService.AtualizarLimitePix(limitePix);
            return Ok();
        }

        [HttpDelete("ExcluirLimitePix/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExcluirLimitePix(int id)
        {
            await _limitePixService.ExcluirLimitePix(id);
            return Ok();
        }

        [HttpPost("ConsultarLimitePix")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConsultarLimitePix([FromBody] LimitePixModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _limitePixService.ConsultarLimite(request.Cpf, request.Agencia, request.Conta, request.LimitePix);
            return Ok(new { Message = result });
        }

        [HttpGet("ListarTodosLimitesPix")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListarTodosLimitesPix()
        {
            var limites = await _limitePixService.ListarTodosLimitesPix();
            return Ok(limites);
        }
    }
}
