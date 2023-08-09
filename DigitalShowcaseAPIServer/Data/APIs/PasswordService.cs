using DigitalShowcaseAPIServer.Data.Interfaces;
using DigitalShowcaseAPIServer.Data.Models;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace DigitalShowcaseAPIServer.Data.APIs
{
    public class PasswordService<TUser> : IPasswordService<TUser> where TUser : User
    {
        private readonly byte[]? Pepper; // TODO: save as safe string

        public PasswordService(IOptions<PasswordServiceOptions> options)
        {
            Pepper = StringToByteArray(options.Value.Pepper); // should be stored in secrets.json
            if (Pepper is null)
                throw new ArgumentNullException(nameof(Pepper));
        }

        public byte[] GetRandomByteArray(int length)
        {
            return RandomNumberGenerator.GetBytes(length);
        }

        public byte[]? StringToByteArray(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            return Encoding.UTF8.GetBytes(str);
        }

        public string ByteArrayToBase64String(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public string GenerateSalt() // TODO: use better salt generator than Guid.NewGuid()
        {
            //byte[] buffer = RandomNumberGenerator.GetBytes(16);
            //string salt = ByteArrayToBase64String(buffer);
            //return salt;
            return Guid.NewGuid().ToString();
        }

        public Task<byte[]?> HashPasswordAsync(string password, string salt) // TODO: use IPasswordHasher
        {
            byte[]? arrSalt = StringToByteArray(salt);
            if (arrSalt is null)
                return Task.FromResult<byte[]?>(null);

            return HashPasswordAsync(password, arrSalt);
        }

        /// <summary>
        /// https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public async Task<byte[]?> HashPasswordAsync(string password, byte[] salt)
        {
            if (password.Length < 8 || password.Length > 64)
                return null;

            var argon2 = new Argon2id(StringToByteArray(password))
            {
                Salt = salt,
                DegreeOfParallelism = 1,
                Iterations = 3,
                MemorySize = 12288,
                KnownSecret = Pepper,
            };

            return await argon2.GetBytesAsync(128);
        }

        public Task<PasswordVerificationResult> VerifyHashedPasswordAsync(string hashedPassword, string password, string salt)
        {
            byte[]? arrSalt = StringToByteArray(salt);
            if (arrSalt is null)
                return Task.FromResult(PasswordVerificationResult.Failed);

            return VerifyHashedPasswordAsync(hashedPassword, password, arrSalt);
        }

        public async Task<PasswordVerificationResult> VerifyHashedPasswordAsync(string hashedPassword, string password, byte[] salt)
        {
            byte[]? arrPassHash = await HashPasswordAsync(password, salt);
            if (arrPassHash is null)
                return PasswordVerificationResult.Failed;

            string passHash = ByteArrayToBase64String(arrPassHash);
            if (passHash == hashedPassword)
                return PasswordVerificationResult.Success;

            return PasswordVerificationResult.Failed;
        }

        public string HashPassword(TUser user, string password)
        {
            Task<byte[]?> task = HashPasswordAsync(password, user.PassSalt);
            task.Wait();
            byte[]? hash = task.Result;
            if (hash is null)
                return string.Empty;

            return ByteArrayToBase64String(hash);
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string password)
        {
            Task<PasswordVerificationResult> task = VerifyHashedPasswordAsync(user.PassHash, hashedPassword, password);

            return task.GetAwaiter().GetResult();
        }
    }
}