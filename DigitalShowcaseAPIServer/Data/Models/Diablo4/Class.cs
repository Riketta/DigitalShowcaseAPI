using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DigitalShowcaseAPIServer.Data.Models.Diablo4
{
    public class Class
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ClassId
        {
            //None = 0,
            Barbarian = 1,
            Druid = 2,
            Necromancer = 3,
            Rogue = 4,
            Sorcerer = 5,
        }

        [Required]
        public ClassId Id { get; set; }

        [Required, MaxLength(64)]
        public string? Name { get; set; }

        [NotMapped]
        public static readonly Dictionary<ClassId, Class> Classes = new Dictionary<ClassId, Class>()
        {
            [ClassId.Barbarian] = new Class() { Id = ClassId.Barbarian, Name = ClassId.Barbarian.ToString(), },
            [ClassId.Druid] = new Class() { Id = ClassId.Druid, Name = ClassId.Druid.ToString(), },
            [ClassId.Necromancer] = new Class() { Id = ClassId.Necromancer, Name = ClassId.Necromancer.ToString(), },
            [ClassId.Rogue] = new Class() { Id = ClassId.Rogue, Name = ClassId.Rogue.ToString(), },
            [ClassId.Sorcerer] = new Class() { Id = ClassId.Sorcerer, Name = ClassId.Sorcerer.ToString(), },
        };
    }
}
