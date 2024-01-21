using FileAPI.Business;
using FileAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly FileUploadAPIService _fileUploadAPIService;
        public FileController(FileUploadAPIService fileUploadAPIService)
        {
            _fileUploadAPIService = fileUploadAPIService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromQuery] FileUploadRequest fileUploadRequest)
        {
            fileUploadRequest.Receipts = Request.Form.Files;
            await _fileUploadAPIService.UploadAsync("Receipts", fileUploadRequest.Receipts);

            return Ok();
        }
    }
}
