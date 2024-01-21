using Base.Events;
using FileAPI.Business;
using FileAPI.Dtos;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly FileUploadAPIService _fileUploadAPIService;
        private readonly IPublishEndpoint _publishEndpoint;
        public FileController(FileUploadAPIService fileUploadAPIService, IPublishEndpoint publishEndpoint)
        {
            _fileUploadAPIService = fileUploadAPIService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromQuery] FileUploadRequest fileUploadRequest)
        {
            fileUploadRequest.Receipts = Request.Form.Files;
            var result = await _fileUploadAPIService.UploadAsync("Receipts", fileUploadRequest.Receipts);
            var UserNumber = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
            ReceiptsEvent @event = new ReceiptsEvent { Description = fileUploadRequest.Description, Path = result.First().pathOrContainerName, UserNumber = int.Parse(UserNumber)};
            await _publishEndpoint.Publish(@event);

            return Ok();
        }
    }
}
