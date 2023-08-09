using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace DigitalShowcaseAPIServer.Data.Interfaces
{
    public interface IPasswordService<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        public byte[] GetRandomByteArray(int length);
        public byte[]? StringToByteArray(string str);
        public string ByteArrayToBase64String(byte[] bytes);
        public string GenerateSalt();

        /// <summary>
        /// https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public Task<byte[]?> HashPasswordAsync(string password, string salt);
        public Task<byte[]?> HashPasswordAsync(string password, byte[] salt);
        public Task<PasswordVerificationResult> VerifyHashedPasswordAsync(string hashedPassword, string password, string salt);
        public Task<PasswordVerificationResult> VerifyHashedPasswordAsync(string hashedPassword, string password, byte[] salt);
    }
}
