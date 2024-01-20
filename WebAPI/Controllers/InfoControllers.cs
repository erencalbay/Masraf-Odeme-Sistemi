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
        private readonly IMediator mediator;

        public InfoControllers(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<InfoResponse>>> GetAllInfos()
        {
            var opr = new GetAllInfoQuery();
            var result = await mediator.Send(opr);
            return result;
        }
        
        [HttpGet]
        [Route("GetInfoWithId")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<InfoResponse>> GetInfoWithId(int InfoNumber)
        {
            var operation = new GetInfoByIdQuery(InfoNumber);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet]
        [Route("GetInfosWithUserId")]
        [Authorize(Roles = "employee, admin")]
        public async Task<ApiResponse<List<InfoResponse>>> GetInfosWithUserId(int? UserNumber)
        {
            if(UserNumber == null)
            {
                UserNumber = int.Parse(User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value);
            }
            var operation = new GetUserInfosByIdQuery(UserNumber);
            var result = await mediator.Send(operation);
            return result;
        }
        //PERSONEL DEĞİŞTİRECEK     
        [HttpPost]
        [Authorize(Roles = "employee")]
        public async Task<ApiResponse<InfoResponse>> Post([FromBody] InfoRequest info)
        {
            var operation = new CreateInfoCommand(info);
            var result = await mediator.Send(operation);
            return result;
        }
        //PERSONEL DEĞİŞTİRECEK
        [Authorize(Roles = "employee")]
        [HttpPut]
        public async Task<ApiResponse> Put(int InfoId, [FromBody] InfoRequest info)
        {
            var operation = new UpdateInfoCommand(InfoId, info);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> Delete(int InfoId)
        {
            var operation = new DeleteInfoCommand(InfoId);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
