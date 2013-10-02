using System;

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

    public class DbGarantie
    {
        public int Risque_ID { get; set; }
        public int Client_ID { get; set; }
        // Garantie principale
        public decimal Montant_Risque { get; set; }
        public string Option_Risque { get; set; }
        public DateTime Date_Risque { get; set; }
        public DateTime Periode_Debut { get; set; }
        public DateTime Periode_Fin { get; set; }
        // Garantie complémentaire
        public decimal Montant_Risque_Compl { get; set; }
        public DateTime Date_Debut_Risque { get; set; }
        public DateTime Date_Fin_Risque { get; set; }
        // Garantie interne
        public decimal Garantie_Interne { get; set; }
        public DateTime Garantie_Periode_Debut { get; set; }
        public DateTime Garantie_Periode_Fin { get; set; }
        // Garantie OAL
        public decimal Mnt_Oal { get; set; }
        public DateTime Dte_Debut_Oal { get; set; }
        public DateTime Dte_Fin_Oal { get; set; }
        // Garantie CAP
        public decimal Mnt_Cap { get; set; }
        public string Option_Cap { get; set; }
        public DateTime Dte_Debut_Cap { get; set; }
        public DateTime Dte_Fin_Cap { get; set; }
        // Montant déblocage
        public decimal Montant_Deblocage { get; set; }
    }

    public class DbDecision
    {
        public int Historique_ID { get; set; }
        public int ID { get; set; }
        public DateTime Decision_Date { get; set; }
        public string Significatif { get; set; }
        public string Result_Code { get; set; }
        public string Decision_Code { get; set; }
        public string Condition_Code { get; set; }
        public decimal Montant { get; set; }
        public decimal Second_Montant { get; set; }
        public DateTime Date_Effet { get; set; }
        public DateTime Date_Fin_Effet { get; set; }
        public DateTime Date_Entree { get; set; }
        public string TCode { get; set; }
        public int Supersede { get; set; }
        public DateTime Date_Last_Update { get; set; }
        public DateTime Date_Import { get; set; }
        public DateTime Date_Fichier { get; set; }
    }
}
