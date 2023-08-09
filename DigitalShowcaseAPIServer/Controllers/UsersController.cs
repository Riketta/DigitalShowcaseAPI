using DigitalShowcaseAPIServer.Data.Interfaces;
using DigitalShowcaseAPIServer.Data.Models;
using DigitalShowcaseAPIServer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalShowcaseAPIServer.Controllers
{
    [RequireHttps]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _api;

        public UsersController(IUserService api)
        {
            _api = api;
        }

        /// <summary>
        /// Get user data for user found by name. User that trying to get roles has to be MasterAdmin or this user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "MasterAdmin")]
        public async Task<ActionResult<UserTransferObject>> GetUser(string username) // TODO: let to call this function to current user
        {
            User? user = await _api.FindUserByNameAsync(username);
            if (user is null)
                return BadRequest($"User with name \"{username}\" not found!");

            return Ok(UserTransferObject.FromUser(user));
        }

        /// <summary>
        /// Get roles for user found by name. User that trying to get roles has to be MasterAdmin.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>list of roles</returns>
        [Route("roles/get")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "MasterAdmin")] // TODO: move role checks into UserService to also let lower admins to modify lower roles
        public async Task<ActionResult<Roles>> GetUserRoles(string username)
        {
            var roles = await _api.GetUserRoles(username);
            if (roles is null)
                return BadRequest($"User with name \"{username}\" not found!");

            return Ok(roles);
        }

        /// <summary>
        /// Set roles for user found by name. User that trying to modify roles has to be MasterAdmin.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roles"></param>
        /// <returns>new list of roles</returns>
        [Route("roles/set")]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "MasterAdmin")] // TODO: move role checks into UserService to also let admins modify lower roles
        public async Task<ActionResult<Roles>> SetUserRoles(string username, Roles roles)
        {
            Roles newRoles = await _api.SetUserRoles(username, roles);
            return Ok(newRoles);
        }
    }
}
