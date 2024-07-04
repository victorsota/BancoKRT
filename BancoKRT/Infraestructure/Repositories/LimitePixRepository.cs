using Amazon.DynamoDBv2.DataModel;
using BancoKRT.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoKRT.Infraestructure.Repositories
{
    public class LimitePixRepository : ILimitePixRepository
    {
        private readonly IDynamoDBContext _dbContext;

        public LimitePixRepository(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Adicionar(LimitePixModel limitepix)
        {
            await _dbContext.SaveAsync(limitepix);
        }

        public async Task Atualizar(LimitePixModel limitepix)
        {
            await _dbContext.SaveAsync(limitepix);
        }

        public async Task<IEnumerable<LimitePixModel>> Buscar(string cpfOuConta)
        {
            if (cpfOuConta.Length == 11 && cpfOuConta.All(char.IsDigit))
            {
                var scanConditions = new List<ScanCondition>
                {
                    new ScanCondition("Cpf", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, cpfOuConta)
                };
                return await _dbContext.ScanAsync<LimitePixModel>(scanConditions).GetRemainingAsync();
            }
            else if (cpfOuConta.All(char.IsDigit))
            {
                var scanConditions = new List<ScanCondition>
                {
                    new ScanCondition("Conta", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, cpfOuConta)
                };
                return await _dbContext.ScanAsync<LimitePixModel>(scanConditions).GetRemainingAsync();
            }
            else
            {
                return Enumerable.Empty<LimitePixModel>();
            }
        }

        public async Task<IEnumerable<LimitePixModel>> ListarTodos()
        {
            return await _dbContext.ScanAsync<LimitePixModel>(new List<ScanCondition>()).GetRemainingAsync();
        }

        public async Task Excluir(int id)
        {
            var itemToDelete = await _dbContext.LoadAsync<LimitePixModel>(id);
            if (itemToDelete != null)
            {
                await _dbContext.DeleteAsync(itemToDelete);
            }
        }

        public async Task<LimitePixModel> BuscarPorId(int id)
        {
            return await _dbContext.LoadAsync<LimitePixModel>(id);
        }
    }
}
