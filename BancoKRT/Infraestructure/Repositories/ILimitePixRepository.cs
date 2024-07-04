using BancoKRT.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BancoKRT.Infraestructure.Repositories
{
    public interface ILimitePixRepository
    {
        Task Adicionar(LimitePixModel limitepix);
        Task Atualizar(LimitePixModel limitepix);
        Task<IEnumerable<LimitePixModel>> Buscar(string cpfOuConta);
        Task Excluir(int id);
        Task<IEnumerable<LimitePixModel>> ListarTodos();
        Task<LimitePixModel> BuscarPorId(int id);
    }
}
