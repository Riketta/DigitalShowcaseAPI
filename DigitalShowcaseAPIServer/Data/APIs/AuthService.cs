using DigitalShowcaseAPIServer.Data.Contexts;
using DigitalShowcaseAPIServer.Data.Interfaces;
using DigitalShowcaseAPIServer.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using static DigitalShowcaseAPIServer.Data.Interfaces.IAuthService;

namespace DigitalShowcaseAPIServer.Data.APIs
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _api;
        private readonly IHttpContextAccessor _accessor;

        public AuthService(IUserService api, IHttpContextAccessor accessor)
        {
            _api = api;
            _accessor = accessor;
        }

        public async Task<bool> Register(UserCredential userCredential)
        {
            return await _api.CreateUserAsync(userCredential);
        }

        public async Task<User?> Login(UserCredential userCredential)
        {
            User? user = await _api.FindUserByNameAsync(userCredential.Name);
            if (user is null)
                return null;

            bool canLogin = await _api.ValidateUserCredentialAsync(userCredential);
            if (!canLogin)
                return null;

            ClaimsPrincipal userPrincipal = UserService.GenerateClaimsPrincipalFromUser(user);
            await _accessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties() { IsPersistent = true });

            return user;
        }

        public async Task Logout()
        {
            await _accessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
