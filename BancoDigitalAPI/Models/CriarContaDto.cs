namespace BancoDigitalAPI.Models
{
    using Swashbuckle.AspNetCore.Filters;
    using Swashbuckle.AspNetCore.Annotations;    
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class CriarContaDTO
    {
        [SwaggerSchema("Nome completo do cliente")]
        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        [JsonPropertyName("nome_cliente")]
        public string NomeCliente { get; set; }

        [SwaggerSchema("Número do CPF do cliente.")]
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve estar no formato 000.000.000-00.")]
        [JsonPropertyName("cpf")]
        public string CPF { get; set; }

        [SwaggerSchema("Saldo inicial da conta.")]
        [Required(ErrorMessage = "O saldo inicial é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O saldo inicial não pode ser negativo.")]
        [JsonPropertyName("saldo_inicial")]
        public decimal SaldoInicial { get; set; }
    }
}
