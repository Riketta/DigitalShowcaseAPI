using DigitalShowcaseAPIServer.Data.Models;

namespace DigitalShowcaseAPIServer.DTOs
{
    public class UserTransferObject
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime DateCreated { get; set; }
        public List<Roles>? Roles { get; set; }

        public static UserTransferObject FromUser(User user)
        {
            var transferObject = new UserTransferObject()
            {
                Id = user.Id,
                Name = user.Name,
                DateCreated = user.DateCreated,
                Roles = new List<Roles>(),
            };

            foreach (Roles role in Enum.GetValues(typeof(Roles)))
                transferObject.Roles.Add(role);

            return transferObject;
        }
    }
}
