using System.ComponentModel.DataAnnotations;
using static DigitalShowcaseAPIServer.Data.Models.Category;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DigitalShowcaseAPIServer.Data.Models.Diablo4
{
    public class ItemType
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ItemId
        {
            //None = 0,
            Helm = 1,
            Chest = 2,
            Gloves = 3,
            Pants = 4,
            Boots = 5,
            Amulet = 6,
            Ring = 7,

            // 1H
            Axe = 8,
            Mace = 9,
            Sword = 10,
            Dagger = 11,
            Scythe = 12,
            Wand = 13,
            Bow = 14,
            Crossbow = 15,

            // 2H
            Staff = 16,
            Polearm = 17,
            TwoHandedAxe = 18,
            TwoHandedMace = 19,
            TwoHandedSword = 20,
            TwoHandedScythe = 21,

            // OH
            Shield = 22,
            Totem = 23,
            Focus = 24,
        }

        [Required]
        public ItemId? Id { get; set; }

        /// <summary>
        /// Displayed name
        /// </summary>
        [Required, MaxLength(64)]
        public string? Name { get; set; }

        [NotMapped]
        public static readonly Dictionary<ItemId, ItemType> ItemTypes = new Dictionary<ItemId, ItemType>()
        {
            [ItemId.Helm] = new ItemType() { Id = ItemId.Helm, Name = ItemId.Helm.ToString(), },
            [ItemId.Chest] = new ItemType() { Id = ItemId.Chest, Name = ItemId.Chest.ToString(), },
            [ItemId.Gloves] = new ItemType() { Id = ItemId.Gloves, Name = ItemId.Gloves.ToString(), },
            [ItemId.Pants] = new ItemType() { Id = ItemId.Pants, Name = ItemId.Pants.ToString(), },
            [ItemId.Boots] = new ItemType() { Id = ItemId.Boots, Name = ItemId.Boots.ToString(), },
            [ItemId.Amulet] = new ItemType() { Id = ItemId.Amulet, Name = ItemId.Amulet.ToString(), },
            [ItemId.Ring] = new ItemType() { Id = ItemId.Ring, Name = ItemId.Ring.ToString(), },
            [ItemId.Axe] = new ItemType() { Id = ItemId.Axe, Name = ItemId.Axe.ToString(), },
            [ItemId.Mace] = new ItemType() { Id = ItemId.Mace, Name = ItemId.Mace.ToString(), },
            [ItemId.Sword] = new ItemType() { Id = ItemId.Sword, Name = ItemId.Sword.ToString(), },
            [ItemId.Dagger] = new ItemType() { Id = ItemId.Dagger, Name = ItemId.Dagger.ToString(), },
            [ItemId.Scythe] = new ItemType() { Id = ItemId.Scythe, Name = ItemId.Scythe.ToString(), },
            [ItemId.Wand] = new ItemType() { Id = ItemId.Wand, Name = ItemId.Wand.ToString(), },
            [ItemId.Bow] = new ItemType() { Id = ItemId.Bow, Name = ItemId.Bow.ToString(), },
            [ItemId.Crossbow] = new ItemType() { Id = ItemId.Crossbow, Name = ItemId.Crossbow.ToString(), },
            [ItemId.Staff] = new ItemType() { Id = ItemId.Staff, Name = ItemId.Staff.ToString(), },
            [ItemId.Polearm] = new ItemType() { Id = ItemId.Polearm, Name = ItemId.Polearm.ToString(), },

            [ItemId.TwoHandedAxe] = new ItemType() { Id = ItemId.TwoHandedAxe, Name = "Two-Handed Axe", },
            [ItemId.TwoHandedMace] = new ItemType() { Id = ItemId.TwoHandedMace, Name = "Two-Handed Mace", },
            [ItemId.TwoHandedSword] = new ItemType() { Id = ItemId.TwoHandedSword, Name = "Two-Handed Sword", },
            [ItemId.TwoHandedScythe] = new ItemType() { Id = ItemId.TwoHandedScythe, Name = "Two-Handed Scythe", },

            [ItemId.Shield] = new ItemType() { Id = ItemId.Shield, Name = ItemId.Shield.ToString(), },
            [ItemId.Totem] = new ItemType() { Id = ItemId.Totem, Name = ItemId.Totem.ToString(), },
            [ItemId.Focus] = new ItemType() { Id = ItemId.Focus, Name = ItemId.Focus.ToString(), },
        };
    }
}
