using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using AutoMapper;
using Dapper;

namespace Quick5.Models
{
    /// <summary>
    /// Objet Table utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class Table
    {
        public string Name { get; set; }
        public List<Column> Columns { get; set; }
    }

    /// <summary>
    /// Objet Column utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class Column
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsNotNull { get; set; }
        public string Default { get; set; }
        public decimal Size { get; set; }
    }

    /// <summary>
    /// Objet SqlCeColumn stocké dans la base de données SQL Server CE
    /// </summary>
    [Table("Information_Schema.Columns")]
    public class SqlCeColumn
    {
        public string Table_Name { get; set; }
        public string Column_Name { get; set; }
        public string Column_Default { get; set; }
        public string Column_Flags { get; set; }
        public string Is_Nullable { get; set; }
        public string Data_Type { get; set; }
        public string Character_Maximum_Length { get; set; }
        public string Numeric_Precision { get; set; }
        public string Numeric_Scale { get; set; }
        public string Autoinc_Increment { get; set; }
    }

    /// <summary>
    /// Fonctions utilitaires pour gérer les tables
    /// </summary>
    public class Tables
    {
        private IDbConnection connexion;

        public Tables(IDbConnection connexion)
        {
            this.connexion = connexion;
        }

        public List<Table> List()
        {
            // Retrouve toutes les tables depuis SQL Server CE
            var sql = @"SELECT Table_Name AS Name
                        FROM   Information_Schema.Tables
                        WHERE  (Table_Type = 'TABLE')
                        ORDER BY Table_Name";

            var data = connexion.Query<Table>(sql).ToList();
            return data;
        }

        public Table Get(string Table_Name)
        {
            // Retrouve toutes les colonnes de la table depuis SQL Server CE
            var where = @"WHERE  (Table_Name = :Table_Name)
                          ORDER BY Ordinal_Position";

            var data = connexion.List<SqlCeColumn>(where, new { Table_Name }).ToList();

            data.ForEach(c => { if (c.Data_Type == "ntext") c.Character_Maximum_Length = null; });
            data.ForEach(c => { if (c.Data_Type == "datetime") c.Numeric_Precision = null; });
            data.ForEach(c => { if (c.Data_Type == "datetime") c.Numeric_Scale = null; });
            data.ForEach(c => { if (c.Data_Type == "image") c.Character_Maximum_Length = null; });
            data.ForEach(c => { if (c.Autoinc_Increment != null) c.Column_Default = "(autoincrement)"; });

            var table = new Table
            {
                Name = Table_Name,
                Columns = Mapper.Map<IEnumerable<SqlCeColumn>, List<Column>>(data)
            };

            return table;
        }
    }

    public partial class MappingConfig
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de SqlCeColumns à Column
        /// </summary>
        public static void Columns()
        {
            Mapper.CreateMap<SqlCeColumn, Column>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<SqlCeColumn, Column>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Column_Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Data_Type))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => Convert.ToInt32(src.Character_Maximum_Length) + Convert.ToInt32(src.Numeric_Precision) + (Convert.ToInt32(src.Numeric_Scale) / 10)))
                .ForMember(dest => dest.Default, opt => opt.MapFrom(src => src.Column_Default))
                .ForMember(dest => dest.IsNotNull, opt => opt.MapFrom(src => src.Is_Nullable == "NO"))
            ;
        }
    }
}
