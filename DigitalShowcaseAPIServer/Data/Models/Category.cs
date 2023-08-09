using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DigitalShowcaseAPIServer.Data.Models
{
    public class Category
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum CategoryId
        {
            [Description("Incorrect Category")]
            None = 0,

            [Description("Diablo IV")]
            Diablo4 = 1,

            [Description("VersaDebug")]
            VersaDebug = 500,
        }

        //[NotMapped]
        //[Column("column_name")]
        //[Column(TypeName = "varchar(200)")]
        //[Comment("Column comment yes")]
        //[Column(Order = 1)]

        [Required]
        public CategoryId Id { get; set; }

        /// <summary>
        /// Value to sort categories by to display, higher value - higher priority
        /// </summary>
        [Required]
        public int Priority { get; set; } = 0;
        
        [Required, MaxLength(128)]
        public string? Name { get; set; }
        
        [Required]
        public bool IsVisible { get; set; } = true;

        [Required, MaxLength(256)]
        public string? IconURL { get; set; }

        [NotMapped]
        public static readonly Dictionary<CategoryId, Category> Categories = new Dictionary<CategoryId, Category>()
        {
            [CategoryId.Diablo4] = new Category()
            {
                Id = CategoryId.Diablo4,
                Priority = 0,
                Name = "Diablo IV",
                IsVisible = true,
                IconURL = "",
            },
            [CategoryId.VersaDebug] = new Category()
            {
                Id = CategoryId.VersaDebug,
                Priority = -1,
                Name = "VersaDebug",
                IsVisible = false,
                IconURL = "",
            },
        };
    }
}
