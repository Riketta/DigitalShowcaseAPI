using DigitalShowcaseAPIServer.Data.Interfaces;
using DigitalShowcaseAPIServer.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DigitalShowcaseAPIServer.Controllers
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-7.0
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LotsController : ControllerBase
    {
        private readonly ILotService _api;

        public LotsController(ILotService api)
        {
            _api = api;
        }

        /// <summary>
        /// Get list of lots <see cref="Lot"/> displayed by pages and filtered by query (TODO: filtering and lot querying)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<LotTransferObject>>> Get()
        {
            List<LotTransferObject> lotTransferObject = await _api.GetLotsAsync(0, 0);
            return Ok(lotTransferObject);
        }

        /// <summary>
        /// Get a single lot by int id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LotTransferObject>> GetLotById(int id)
        {
            LotTransferObject? lotTransferObject = await _api.GetLotAsync(id);

            if (lotTransferObject is null)
                return NotFound($"No lot found with id: {id}");

            return Ok(lotTransferObject);
        }

        /// <summary>
        /// Add lot by passing new <see cref="LotTransferObject"/> lot in body. Fill only one of many LotData field that matches assigned to lot <see cref="Lot.CategoryId"/>. Example: if <see cref="Lot.CategoryId"/> == <see cref="Category.CategoryId.Diablo4"/> then <see cref="LotTransferObject.Diablo4_LotsData"/> should be passed. Checks user authorization.
        /// </summary>
        /// <param name="lotTransferObject"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public async Task<ActionResult<LotTransferObject>> AddLot([FromBody] LotTransferObject? lotTransferObject)
        {
            if (lotTransferObject is null)
                return BadRequest();

            var addedLotTransferObject = await _api.AddLotAsync(lotTransferObject, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!));
            if (addedLotTransferObject is null)
                return NoContent();

            return Created($"/api/lots/{addedLotTransferObject.Id}", addedLotTransferObject);
        }

        /// <summary>
        /// Replace existing <see cref="Lot"/> lot with same object updated with new data. Receive existing data first using <see cref="GetLotById(int)"/> then pass it in body with new data keeping indexing fields with original data. Checks user authorization.
        /// </summary>
        /// <param name="lotTransferObject"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public async Task<ActionResult<LotTransferObject>> UpdateLot([FromBody] LotTransferObject? lotTransferObject)
        {
            LotTransferObject? updatedLotTransferObject = await _api.UpdateLotAsync(lotTransferObject);
            if (updatedLotTransferObject is null)
                return NotFound($"Can't update non-existent lot!");
            
            return updatedLotTransferObject;
        }

        /// <summary>
        /// Delete lot with specified id, checks user authorization
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            bool? isDeleted = await _api.DeleteLotAsync(id);
            
            if (isDeleted is null)
                return BadRequest();

            if (isDeleted == false)
                return NotFound();

            return Ok();
        }
    }
}
