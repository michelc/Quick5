using System;
using System.Collections.Generic;

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

        public Siren Siren { get; set; }
        public Garantie Garantie { get; set; }
    }

    public class DbSiren
    {
        public int ID { get; set; }
        public string Raison_Social { get; set; }
        public string Siren { get; set; }
        public string Blocage { get; set; }
    }

    public class Siren
    {
        public int Siren_ID { get; set; }
        public string Nom { get; set; }
        public string NSiren { get; set; }
        public bool EstBloque { get; set; }

        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<Garantie> Garanties { get; set; }
        public IEnumerable<Decision> Decisions { get; set; }
    }

    public class Garantie
    {
        public int Garantie_ID { get; set; }
        public int Client_ID { get; set; }
        // Garantie principale
        public decimal GarMontant { get; set; }
        public string GarOption { get; set; }
        public DateTime GarDate { get; set; }
        public DateTime GarDebut { get; set; }
        public DateTime GarFin { get; set; }
        // Garantie complémentaire
        public decimal CplMontant { get; set; }
        public DateTime CplDebut { get; set; }
        public DateTime CplFin { get; set; }
        // Garantie interne
        public decimal IntMontant { get; set; }
        public DateTime IntDebut { get; set; }
        public DateTime IntFin { get; set; }
        // Garantie OAL
        public decimal OalMontant { get; set; }
        public DateTime OalDebut { get; set; }
        public DateTime OalFin { get; set; }
        // Garantie CAP
        public decimal CapMontant { get; set; }
        public string CapOption { get; set; }
        public DateTime CapDebut { get; set; }
        public DateTime CapFin { get; set; }
        // Montant déblocage
        public decimal Deblocage { get; set; }
        // Garantie totale
        public decimal Totale { get; set; }

        public Client Client { get; set; }
        public Siren Siren { get; set; }
    }

    public class Decision
    {
        public int Decision_ID { get; set; }
        public int Siren_ID { get; set; }
        public DateTime DDecision { get; set; }
        public bool Significatif { get; set; }
        public string Resultat { get; set; }
        public string Code { get; set; }
        public string Condition { get; set; }
        public decimal Montant { get; set; }
        public decimal Complement { get; set; }
        public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }
        public DateTime DEntree { get; set; }
        public string TCode { get; set; }
        public bool Super { get; set; }
        public DateTime DUpdate { get; set; }
        public DateTime DImport { get; set; }
        public DateTime DFichier { get; set; }
    }
}
