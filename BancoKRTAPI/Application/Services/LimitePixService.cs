using BancoKRT.Application.Interfaces;
using BancoKRT.Domain.Entities;
using BancoKRT.Infraestructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BancoKRT.Application.Services
{
    public class LimitePixService : ILimitePixService
    {
        private readonly ILimitePixRepository _limitePixRepository;

        public LimitePixService(ILimitePixRepository limitePixRepository)
        {
            _limitePixRepository = limitePixRepository;
        }

        public async Task AdicionarLimitePix(LimitePixModel limitePix)
        {
            await _limitePixRepository.Adicionar(limitePix);
        }

        public async Task AtualizarLimitePix(LimitePixModel limitePix)
        {
            await _limitePixRepository.Atualizar(limitePix);
        }

        public async Task<IEnumerable<LimitePixModel>> BuscarLimitesPorCpfOuConta(string cpfOuConta)
        {
            return await _limitePixRepository.Buscar(cpfOuConta);
        }

        public async Task ExcluirLimitePix(int id)
        {
            await _limitePixRepository.Excluir(id);
        }

        public async Task<LimitePixModel> BuscarLimitePorId(int id)
        {
            return await _limitePixRepository.BuscarPorId(id);
        }

        public async Task<string> ConsultarLimite(string cpf, int agencia, string conta, int limitePix)
        {
            var clientes = await _limitePixRepository.Buscar(cpf);
            if (clientes == null || !clientes.Any())
            {
                return "CPF não encontrado.";
            }

            var cliente = clientes.FirstOrDefault(c => c.Agencia == agencia && c.Conta == conta);
            if (cliente == null)
            {
                return "Os dados informados não correspondem ao CPF informado.";
            }

            if (limitePix <= cliente.LimitePix)
            {
                return $"O LimitePix informado ({limitePix}) é menor ou igual ao limite já registrado ({cliente.LimitePix}). TRANSAÇÃO ACEITA";
            }
            else
            {
                return $"O LimitePix informado ({limitePix}) é maior que o limite já registrado ({cliente.LimitePix}). TRANSAÇÃO NEGADA";
            }
        }
    }
}
