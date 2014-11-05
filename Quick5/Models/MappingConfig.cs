namespace Quick5.Models
{
    public partial class MappingConfig
    {
        public static void RegisterMappings()
        {
            Sirens();
            Clients();
            Agences();
            Communes();
            Insees();
            Garanties();
            Decisions();
            EdiAccords();
            EdiSites();
            EdiQualifications();
            Columns();
            MdxOrganisations();
        }
    }
}
