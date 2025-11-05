namespace TelephoneDiary.Models
{
    public class Contatos
    {
        public Guid ID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
        public List<Telefones> Telefones { get; set; } = [];
    }
}