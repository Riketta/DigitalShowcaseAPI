using DigitalShowcaseAPIServer.Data.Contexts;
using DigitalShowcaseAPIServer.Data.Interfaces;
using DigitalShowcaseAPIServer.Data.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static DigitalShowcaseAPIServer.Data.Models.Category;

namespace DigitalShowcaseAPIServer.Data.APIs
{
    public class UserService : IUserService // TODO: check authorization
    {
        private readonly DigitalShowcaseContext _db;
        private readonly IPasswordService<User> _ps;

        public UserService(DigitalShowcaseContext db, IPasswordService<User> ps)
        {
            _db = db;
            _ps = ps;
        }

        public static ClaimsPrincipal GenerateClaimsPrincipalFromUser(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
            };

            //claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.ToString())));
            foreach (Roles role in Enum.GetValues(typeof(Roles)))
                if (user.Roles.HasFlag(role))
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);

            return userPrincipal;
        }

        public async Task<bool> CreateUserAsync(UserCredential userCredential) // TODO: fix non-atomic DB operation
        {
            User? user = await FindUserByNameAsync(userCredential.Name);
            if (user is not null)
                return false;

            if (userCredential.Password.Length < 8 || userCredential.Password.Length > 64)
                return false;

            user = new User()
            {
                Name = userCredential.Name,
                PassSalt = _ps.GenerateSalt(),
                DateCreated = DateTime.UtcNow,
            };

            byte[]? hash = await _ps.HashPasswordAsync(userCredential.Password, user.PassSalt);
            if (hash is null)
                return false;
            
            user.PassHash = _ps.ByteArrayToBase64String(hash);

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// This function should be the only way to obtain access to user stored in database. Looking for user by comparing lowered user names (to make names case insensitive)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<User?> FindUserByNameAsync(string name)
        {
            User? user = await _db.Users.SingleOrDefaultAsync(user => user.Name.ToLower() == name.ToLower()); // TODO: fix client-side filtering due to .ToLower()?

            return user;
        }

        public async Task<bool> ValidateUserCredentialAsync(UserCredential userCredential)
        {
            User? user = await FindUserByNameAsync(userCredential.Name);
            if (user is null)
                return false;

            var result = await _ps.VerifyHashedPasswordAsync(user.PassHash, userCredential.Password, user.PassSalt);
            if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
                return false;
            
            return true;
        }

        public Task<Roles> SetUserRoles(string username, Roles roles)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Roles>?> GetUserRoles(string username)
        {
            User? user = await FindUserByNameAsync(username);
            if (user is null)
                return null;

            var roles = new List<Roles>();
            foreach (Roles role in Enum.GetValues(typeof(Roles)))
                roles.Add(role);

            return roles;
        }
    }
}
