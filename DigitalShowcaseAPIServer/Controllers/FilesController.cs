using DigitalShowcaseAPIServer.Data.Interfaces;
using DigitalShowcaseAPIServer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalShowcaseAPIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _api;

        public FilesController(IFileService api)
        {
            _api = api;
        }

        /// <summary>
        /// Upload file with white-listed content type (see <see cref="Data.Models.File.FileType"/>)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Route("upload")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public async Task<ActionResult<string?>> UploadFile([FromForm] FileTransferObject file)
        {
            return Ok(HttpContext.Request.PathBase.Add(await _api.UploadFileAsync(file)).ToUriComponent());
        }

        /// <summary>
        /// Restore file from database to file system if required
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [Route("download")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PhysicalFileResult>> DownloaadFile(string filename)
        {
            Data.Models.File? file = await _api.DownloadFileByNameAsync(filename);
            if (file is null)
                return NotFound();

            var filePath = Path.Combine(_api.GetStaticContentPath(), file.Name);
            return PhysicalFile(filePath, file.Type.GetDescription());
        }

        /// <summary>
        /// Delete file both from database and file system
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin, MasterAdmin")]
        public async Task<ActionResult<string?>> DeleteFile(string filename)
        {
            await _api.DeleteFileByNameAsync(filename);
            return Ok();
        }
    }
}
