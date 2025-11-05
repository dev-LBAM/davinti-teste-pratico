namespace TelephoneDiary.Models
{
    public class Telefones
    {
        public Guid ID { get; set; }
        public Guid IDContato { get; set; }

        public string Numero { get; set; } = string.Empty;
    }
}