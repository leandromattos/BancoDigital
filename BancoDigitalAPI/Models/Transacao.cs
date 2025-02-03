namespace BancoDigitalAPI.Models
{
    public class Transacao
    {
        public int Id { get; set; }
        public string Tipo { get; set; } // "Deposito" ou "Saque"
        public DateTime DataHora { get; set; }
        public decimal Valor { get; set; }
        public int ContaId { get; set; } // Chave estrangeira
        public Conta Conta { get; set; } // Propriedade de navegação
    }
}
