using DigitalShowcaseAPIServer.Data.Models;
using System.Security.Claims;

namespace DigitalShowcaseAPIServer.Data.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Manages user credentials, before storing it in database
        /// </summary>
        /// <param name="userCredential"></param>
        /// <returns></returns>
        public Task<bool> Register(UserCredential userCredential);
        public Task<User?> Login(UserCredential userCredential);
        public Task Logout();
    }
}
