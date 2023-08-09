using DigitalShowcaseAPIServer.Data.Models;

namespace DigitalShowcaseAPIServer.Data.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Create user in database
        /// </summary>
        /// <param name="userCredential"></param>
        /// <returns>true if user successfully created, false otherwise</returns>
        public Task<bool> CreateUserAsync(UserCredential userCredential);

        /// <summary>
        /// Find user by case insensitive name
        /// </summary>
        /// <param name="name">case insensitive user name</param>
        /// <returns></returns>
        public Task<User?> FindUserByNameAsync(string name);

        /// <summary>
        /// Validate user credential
        /// </summary>
        /// <param name="userCredential"></param>
        /// <returns>true if user passed valid credentials</returns>
        public Task<bool> ValidateUserCredentialAsync(UserCredential userCredential);

        /// <summary>
        /// Get user roles, checks if user trying to get roles authorized to do so
        /// </summary>
        /// <param name="username"></param>
        /// <returns>updated user roles</returns>
        public Task<List<Roles>?> GetUserRoles(string username);

        /// <summary>
        /// Set user roles, checks if user trying to modify roles authorized to do so
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roles"></param>
        /// <returns>updated user roles</returns>
        public Task<Roles> SetUserRoles(string username, Roles roles);
    }
}
