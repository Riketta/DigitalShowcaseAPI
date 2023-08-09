using Microsoft.EntityFrameworkCore;
using DigitalShowcaseAPIServer.Data.Models;
using static DigitalShowcaseAPIServer.Data.Models.Category;

namespace DigitalShowcaseAPIServer.Data.Contexts
{
    public class DigitalShowcaseContext : DbContext
    {
        public DigitalShowcaseContext(DbContextOptions<DigitalShowcaseContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (options.IsConfigured) { }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().HasIndex(user => user.Name).IsUnique();
            modelBuilder.Entity<User>().HasAlternateKey(user => user.Name);
            modelBuilder.Entity<Models.File>().HasAlternateKey(file => file.Name);


            //modelBuilder.Entity<Category>().Property(e => e.Id).HasConversion<int>();
            List<Category> categories = new List<Category>();
            foreach (CategoryId category in Enum.GetValues(typeof(CategoryId)))
            {
                if (category == CategoryId.None)
                {
                    // TODO: logging skipped elements
                    continue;
                }

                if (Category.Categories.ContainsKey(category))
                    categories.Add(Category.Categories[category]);
                else
                    throw new Exception($"Trying to add non-existent category to database: {category}!");
            }
            modelBuilder.Entity<Category>().HasData(categories);


            #region Diablo 4
            // Diablo4.ItemType
            var itemTypes = new List<Models.Diablo4.ItemType>();
            foreach (Models.Diablo4.ItemType.ItemId itemType in Enum.GetValues(typeof(Models.Diablo4.ItemType.ItemId)))
            {
                if (Models.Diablo4.ItemType.ItemTypes.ContainsKey(itemType))
                    itemTypes.Add(Models.Diablo4.ItemType.ItemTypes[itemType]);
                else
                    throw new Exception($"Trying to add non-existent item type to database: {itemType}!");
            }
            modelBuilder.Entity<Models.Diablo4.ItemType>().HasData(itemTypes);

            // Diablo4.Class
            var classes = new List<Models.Diablo4.Class>();
            foreach (Models.Diablo4.Class.ClassId playerClass in Enum.GetValues(typeof(Models.Diablo4.Class.ClassId)))
            {
                if (Models.Diablo4.Class.Classes.ContainsKey(playerClass))
                    classes.Add(Models.Diablo4.Class.Classes[playerClass]);
                else
                    throw new Exception($"Trying to add non-existent class to database: {playerClass}!");
            }
            modelBuilder.Entity<Models.Diablo4.Class>().HasData(classes);
            #endregion

            #region VersaDebug
            User user = new User()
            {
                Id = -1,
                DateCreated = new DateTime(2003, 3, 23),
                Name = "DebugUser",
                PassHash = "Hash",
                PassSalt = "Salt",
                Roles = Roles.Guest,
            };
            modelBuilder.Entity<User>().HasData(user);

            modelBuilder.Entity<Lot>().HasData(
                new Lot()
                {
                    Id = 1,
                    IsSold = false,
                    Priority = 5,
                    AddedByUserId = user.Id,
                    DateAdded = DateTime.Now,
                    DateSold = DateTime.Now.AddDays(500),
                    ImageURL = "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png",
                    CategoryId = CategoryId.VersaDebug,
                    VersaDebug_LotDataId = 1,
                    Diablo4_LotDataId = null, // null not 0!
                },
                new Lot()
                {
                    Id = 2,
                    IsSold = true,
                    Priority = 3,
                    AddedByUserId = user.Id,
                    DateAdded = DateTime.Now,
                    DateSold = DateTime.Now.AddDays(900),
                    ImageURL = "https://www.minecraft.net/content/dam/games/minecraft/logos/logo-minecraft.svg",
                    CategoryId = CategoryId.VersaDebug,
                    VersaDebug_LotDataId = 2,
                    Diablo4_LotDataId = null,
                },
                new Lot()
                {
                    Id = 3,
                    IsSold = false,
                    Priority = 1,
                    AddedByUserId = user.Id,
                    DateAdded = DateTime.Now,
                    DateSold = DateTime.Now.AddDays(1200),
                    ImageURL = "https://cdn.sstatic.net/Img/teams/teams-illo-free-sidebar-promo.svg",
                    CategoryId = CategoryId.VersaDebug,
                    VersaDebug_LotDataId = 3,
                    Diablo4_LotDataId = null,
                });

            modelBuilder.Entity<Models.VersaDebug.LotData>().HasData(
                new Models.VersaDebug.LotData()
                {
                    Id = 1,
                    GUID = "GUID-0010",
                    GemLevel = 10,
                },
                new Models.VersaDebug.LotData()
                {
                    Id = 2,
                    GUID = "GUID-0020",
                    GemLevel = 20,
                },
                new Models.VersaDebug.LotData()
                {
                    Id = 3,
                    GUID = "GUID-0030",
                    GemLevel = 30,
                });
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users => Set<User>(); // TODO: move to separate DB?
        public DbSet<Models.File> Files => Set<Models.File>(); // TODO: move to separate DB?
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Lot> Lots => Set<Lot>();


        #region Categories Specific Lot Data
        #region VersaDebug
        public DbSet<Models.VersaDebug.LotData> VersaDebug_LotsData => Set<Models.VersaDebug.LotData>();
        #endregion

        #region Diablo 4
        public DbSet<Models.Diablo4.ItemType> Diablo4_ItemTypes => Set<Models.Diablo4.ItemType>();
        public DbSet<Models.Diablo4.Class> Diablo4_Classes => Set<Models.Diablo4.Class>();
        public DbSet<Models.Diablo4.LotData> Diablo4_LotsData => Set<Models.Diablo4.LotData>();
        #endregion
        #endregion
    }
}