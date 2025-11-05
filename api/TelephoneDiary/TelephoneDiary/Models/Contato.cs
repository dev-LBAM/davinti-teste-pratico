namespace TelephoneDiary.Models
{
    public class Contato
    {
        public int ID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
        public List<Telefone> Telefones { get; set; } = [];
    }
}