using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace BancoKRT.Models
{
    [DynamoDBTable("ContaLimitePix")]
    public class LimitePixModel
    {
        [DynamoDBHashKey("pk")]
        public int Id { get; set; }

        [DynamoDBProperty]
        public string Cpf { get; set; }

        [DynamoDBProperty]
        public string Conta { get; set; }

        [DynamoDBProperty]
        public int Agencia { get; set; }

        [DynamoDBProperty]
        [Range(0, int.MaxValue, ErrorMessage = "O valor do Limite PIX deve ser maior ou igual a zero.")]
        public int LimitePix { get; set; }
    }
}
