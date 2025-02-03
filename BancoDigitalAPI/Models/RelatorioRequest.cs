using System.ComponentModel.DataAnnotations;

namespace BancoDigitalAPI.Models
{
    public class RelatorioRequest
    {
        [Required(ErrorMessage = "O campo 'dataInicio' é obrigatório.")]
        public string DataInicio { get; set; }

        [Required(ErrorMessage = "O campo 'dataFim' é obrigatório.")]
        public string DataFim { get; set; }
    }
}
