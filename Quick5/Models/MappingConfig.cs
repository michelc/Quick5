namespace Quick5.Models
{
    public partial class MappingConfig
    {
        public static void RegisterMappings()
        {
            Sirens();
            Clients();
            Agences();
            Garanties();
            Decisions();
            EdiAccords();
            EdiSites();
        }
    }
}