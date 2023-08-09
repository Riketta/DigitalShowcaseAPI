using DigitalShowcaseAPIServer.Data.Interfaces;
using DigitalShowcaseAPIServer.Data.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using DigitalShowcaseAPIServer.DTOs;

namespace DigitalShowcaseAPIServer.Controllers
{
    [Route("api/[controller]")]
    [RequireHttps]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _api;

        public AuthController(IAuthService api)
        {
            _api = api;
        }

        /// <summary>
        /// Pass <see cref="UserCredential"/> object filled with user <see cref="UserCredential.Name"/> and <see cref="UserCredential.Password"/> to try to create new account.
        /// </summary>
        /// <param name="userCredential">contains user <see cref="UserCredential.Name"/> and <see cref="UserCredential.Password"/>: name case insensitive ("Riketta" and "riketta" represents same user), password should be at least 8 and at max 64 symbols long</param>
        /// <returns>true if new user successfully registered, false if user with such <see cref="User.Name"/> already exists</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "name": "Riketta",
        ///        "password": "gigapassword"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">If new user successfully registered or if failed due to user already exists</response>
        /// <response code="400">If passed user cretentials incorrect</response>
        [Route("register")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> UserRegister([FromBody] UserCredential? userCredential)
        {
            if (userCredential is null || string.IsNullOrEmpty(userCredential.Name) || string.IsNullOrEmpty(userCredential.Password))
                return BadRequest("The user ID or password was incorrect");

            bool result = await _api.Register(userCredential);
            return result;
        }

        /// <summary>
        /// Pass <see cref="UserCredential"/> object with user <see cref="UserCredential.Name"/> and <see cref="UserCredential.Password"/> in body to try to login
        /// </summary>
        /// <param name="userCredential"></param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserTransferObject>> UserLogin([FromBody] UserCredential? userCredential)
        {
            if (userCredential is null || string.IsNullOrEmpty(userCredential.Name) || string.IsNullOrEmpty(userCredential.Password))
                return BadRequest("The user ID or password was incorrect");

            User? user = await _api.Login(userCredential);
            if (user is null)
                return BadRequest("The user ID or password was incorrect");
            
            return Ok(UserTransferObject.FromUser(user));
        }

        /// <summary>
        /// Logout from user currently logged in
        /// </summary>
        /// <returns></returns>
        [Route("logout")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UserLogout()
        {
            await _api.Logout();

            return Ok();
        }

        /// <summary>
        /// Is a logged in user whether <see cref="Roles.Admin"/> or <see cref="Roles.MasterAdmin"/>
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, MasterAdmin")]
        [Route("isAdmin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<bool> IsAdmin()
        {
            return Ok(true);
        }

        /// <summary>
        /// Is a logged in user has User role
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "User")]
        [Route("isUser")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<bool> IsUser()
        {
            return Ok(true);
        }
    }
}
