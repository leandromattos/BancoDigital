using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BancoDigitalAPI.Models
{
    public class TransacaoDTO
    {
        [SwaggerSchema("ID da conta do cliente")]
        [Required(ErrorMessage = "O ID da conta do cliente é obrigatório.")]
        [JsonPropertyName("conta_id")]
        public int ContaId { get; set; }

        [SwaggerSchema("Valor do déposito")]
        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }
    }
}
