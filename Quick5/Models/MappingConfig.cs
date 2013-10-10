namespace Quick5.Models
{
    public partial class MappingConfig
    {
        public static void RegisterMappings()
        {
            Sirens();
            Clients();
            Garanties();
            Decisions();
            EdiAccords();
            EdiSites();
        }
    }
}