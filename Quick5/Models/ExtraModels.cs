namespace Quick5.Models
{
    public class DbSiren
    {
        public int ID { get; set; }
        public string Raison_Social { get; set; }
        public string Siren { get; set; }
        public string Blocage { get; set; }
    }

    public class DbClient
    {
        public int IdCompany { get; set; }
        public string Name { get; set; }
        public string Siren { get; set; }
        public string Fld109 { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Fld138 { get; set; }
        public string Fld129 { get; set; }
    }
}
