using BancoKRT.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BancoKRT.API.Application.Interfaces
{
    public interface ILimitePixService
    {
        Task<string> AdicionarLimitePix(LimitePixModel limitePix);
        Task AtualizarLimitePix(LimitePixModel limitePix);
        Task<IEnumerable<LimitePixModel>> BuscarLimitesPorCpfOuConta(string cpfOuConta);
        Task ExcluirLimitePix(int id);
        Task<LimitePixModel> BuscarLimitePorId(int id);
        Task<string> ConsultarLimite(string cpf, int agencia, string conta, int limitePix);
        Task<IEnumerable<LimitePixModel>> ListarTodosLimitesPix();
    }
}
