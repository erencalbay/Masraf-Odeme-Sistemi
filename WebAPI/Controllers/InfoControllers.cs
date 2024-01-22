using Base.Response;
using Business.CQRS;
using Data.DbContextCon;
using Data.Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schema;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class InfoControllers : ControllerBase
    {
        // Dependency Injection
        private readonly IMediator mediator;

        public InfoControllers(IMediator mediator)
        {
            this.mediator = mediator;
        }

        //admin görücek ve bütün infoları listeleyecek
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<InfoResponse>>> GetAllInfos()
        {
            var opr = new GetAllInfoQuery();
            var result = await mediator.Send(opr);
            return result;
        }
        
        // admin infoid ile info alacak
        [HttpGet]
        [Route("GetInfoWithId")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<InfoResponse>> GetInfoWithId(int InfoNumber)
        {
            var operation = new GetInfoByIdQuery(InfoNumber);
            var result = await mediator.Send(operation);
            return result;
        }

        // admin userid ye ait olan infoları alacak
        [HttpGet]
        [Route("GetInfosWithUserId")]
        [Authorize(Roles = "employee, admin")]
        public async Task<ApiResponse<List<InfoResponse>>> GetInfosWithUserId(int? UserNumber)
        {
            // null control
            if(UserNumber == null)
            {
                UserNumber = int.Parse(User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value);
            }
            var operation = new GetUserInfosByIdQuery(UserNumber);
            var result = await mediator.Send(operation);
            return result;
        }
        // personel infosunu ekleyebilecek  
        [HttpPost]
        [Authorize(Roles = "employee")]
        public async Task<ApiResponse<InfoResponse>> InfoCreate([FromBody] InfoRequest info)
        {
            var operation = new CreateInfoCommand(info);
            var result = await mediator.Send(operation);
            return result;
        }
        // personel infosunu değiştirebilecek
        [Authorize(Roles = "employee")]
        [HttpPut]
        public async Task<ApiResponse> InfoUpdate([FromBody] InfoUpdateRequest info)
        {
            var operation = new UpdateInfoCommand(info);
            var result = await mediator.Send(operation);
            return result;
        }

        // admin infoları silebilecek
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> InfoDelete(int InfoId)
        {
            var operation = new DeleteInfoCommand(InfoId);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
