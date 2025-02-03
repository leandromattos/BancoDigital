namespace BancoDigitalAPI.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; }
        public string CPF { get; set; }
        public decimal Saldo { get; set; }
    }
}
