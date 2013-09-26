namespace Quick5.Models
{
    public class Client
    {
        public int Client_ID { get; set; }
        public string Nom { get; set; }
        public string NSiren { get; set; }
        public string NSiret { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string Type { get; set; }
        public bool EstBloque { get; set; }
    }

    public class Siren
    {
        public int Siren_ID { get; set; }
        public string Nom { get; set; }
        public string NSiren { get; set; }
        public bool EstBloque { get; set; }
    }
}