using BancoKRT.Application.Interfaces;
using BancoKRT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BancoKRT.WebAPI.Controllers
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
        public async Task<IActionResult> AdicionarLimitePix(LimitePixModel limitePix)
        {
            await _limitePixService.AdicionarLimitePix(limitePix);
            return Ok();
        }

        [HttpPut("AtualizarLimitePix")]
        public async Task<IActionResult> AtualizarLimitePix(LimitePixModel limitePix)
        {
            await _limitePixService.AtualizarLimitePix(limitePix);
            return Ok();
        }

        [HttpDelete("ExcluirLimitePix/{id}")]
        public async Task<IActionResult> ExcluirLimitePix(int id)
        {
            await _limitePixService.ExcluirLimitePix(id);
            return Ok();
        }

        // Adicione outros métodos conforme necessário para outras ações...

        [HttpPost("ConsultarLimitePix")]
        public async Task<IActionResult> ConsultarLimitePix(string cpf, int agencia, string conta, int limitePix)
        {
            var result = await _limitePixService.ConsultarLimite(cpf, agencia, conta, limitePix);
            return Ok(new { Message = result });
        }
    }
}
