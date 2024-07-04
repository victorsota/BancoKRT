using BancoKRT.Domain.Entities;
using BancoKRT.Infraestructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace BancoKRT.WebUI.Controllers
{
    public class LimitePixController : Controller
    {
        private readonly ILogger<LimitePixController> _logger;
        private readonly ILimitePixRepository _repository;

        public LimitePixController(ILogger<LimitePixController> logger, ILimitePixRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(LimitePixModel viewmodel)
        {
            var existingCliente = await _repository.Buscar(viewmodel.Cpf);
            if (existingCliente != null && existingCliente.Any())
            {
                ModelState.AddModelError("Cpf", "Este CPF já foi cadastrado.");
                return View(viewmodel);
            }

            var cliente = new LimitePixModel
            {
                Id = (await _repository.ListarTodos()).Count() + 1,
                Agencia = viewmodel.Agencia,
                Conta = viewmodel.Conta,
                Cpf = viewmodel.Cpf,
                LimitePix = viewmodel.LimitePix,
            };
            await _repository.Adicionar(cliente);

            return RedirectToAction("Buscar");
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(string cpfOuConta)
        {
            if (string.IsNullOrEmpty(cpfOuConta))
            {
                var limitesPix = await _repository.ListarTodos();
                return View(limitesPix);
            }
            else
            {
                var limitesFiltrados = await _repository.Buscar(cpfOuConta);
                return View(limitesFiltrados);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var limitePix = await _repository.BuscarPorId(id);
            if (limitePix == null)
            {
                return NotFound();
            }
            return View(limitePix);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(LimitePixModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _repository.Atualizar(viewModel);
                return RedirectToAction("Buscar");
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Deletar(string id)
        {
            try
            {
                // Verifica se o parâmetro id é válido
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Id é obrigatório.");
                }

                await _repository.Excluir(int.Parse(id));

                return RedirectToAction("Buscar");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir registro.");
                return StatusCode(500, "Erro interno ao tentar excluir o registro.");
            }
        }

        [HttpGet]
        public IActionResult ConsultarLimite()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConsultarLimite(string cpf, int agencia, string conta, int limitePix)
        {
            var clientes = await _repository.Buscar(cpf);
            if (clientes == null || !clientes.Any())
            {
                ViewData["Message"] = "CPF não encontrado.";
                return View("ConsultarLimite");
            }

            var cliente = clientes.FirstOrDefault(c => c.Agencia == agencia && c.Conta == conta);
            if (cliente == null)
            {
                ViewData["Message"] = "Os dados informados não correspondem ao CPF informado.";
                return View("ConsultarLimite");
            }

            if (limitePix <= cliente.LimitePix)
            {
                ViewData["Message"] = $"O LimitePix informado ({limitePix}) é menor ou igual ao limite já registrado ({cliente.LimitePix}). TRANSAÇÃO ACEITA";
            }
            else
            {
                ViewData["Message"] = $"O LimitePix informado ({limitePix}) é maior que o limite já registrado ({cliente.LimitePix}). TRANSAÇÃO NEGADA";
            }

            return View("ConsultarLimite");
        }
    }
}
