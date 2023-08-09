using DigitalShowcaseAPIServer.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DigitalShowcaseAPIServer.Data.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Case insensetive user name due to security reasons
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty; // TODO: set to lower when property updated and add DisplayedName?

        /// <summary>
        /// Password Hash created in the next way: Argon2id with <see cref="PassSalt"/> and Pepper from secrets.json. 
        /// Password used to create hash should be 8 or more and 64 or less symbols length
        /// </summary>
        [Required, MaxLength(172)]
        public string PassHash { get; set; } = string.Empty;

        /// <summary>
        /// Salt created with <see cref="IPasswordService{TUser}.GenerateSalt"/>.
        /// </summary>
        [Required, MaxLength(36)]
        public string PassSalt { get; set; } = string.Empty;

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public Roles Roles { get; set; } = Roles.User;
    }
}
