using System.Text.Json.Serialization;

namespace DigitalShowcaseAPIServer.Data.Models
{
    [Flags]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Roles
    {
        /// <summary>
        /// Guest, unauthorized user
        /// </summary>
        Guest = 0b0000_0000,

        /// <summary>
        /// ReadOnly user
        /// </summary>
        User = 0b0000_0001,
        Admin = 0b0000_0010,

        /// <summary>
        /// Can be assigned only manually, can assign and remove <see cref="Admin"/> role to other users
        /// </summary>
        MasterAdmin = 0b0000_0100,
    }
}
