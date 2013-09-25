namespace Quick5.Models
{
    public class Client
    {
        public int Client_ID { get; set; }
        public string Nom { get; set; }
        public string Siren { get; set; }
        public string Siret { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string Type { get; set; }
        public bool EstBloque { get; set; }
    }
}