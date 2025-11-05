namespace TelephoneDiary.Models
{
    public class Telefone
    {
        public int ID { get; set; }
        public int IDContato { get; set; }
        public string Numero { get; set; } = string.Empty;
    }
}