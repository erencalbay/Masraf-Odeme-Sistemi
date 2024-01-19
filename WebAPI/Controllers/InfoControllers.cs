using Base.Response;
using Business.CQRS;
using Data.DbContextCon;
using Data.Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InfoControllers : ControllerBase
    {
        private readonly IMediator mediator;

        public InfoControllers(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<List<InfoResponse>>> Get()
        {
            var opr = new GetAllInfoQuery();
            var result = await mediator.Send(opr);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<InfoResponse>> Get(int id)
        {
            var operation = new GetInfoByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }

        //PERSONEL DEĞİŞTİRECEK     
        [HttpPost]
        public async Task<ApiResponse<InfoResponse>> Post([FromBody] InfoRequest info)
        {
            var operation = new CreateInfoCommand(info);
            var result = await mediator.Send(operation);
            return result;
        }
        //PERSONEL DEĞİŞTİRECEK

        [HttpPut("{id}")]
        public async Task<ApiResponse> Put(int id, [FromBody] InfoRequest info)
        {
            var operation = new UpdateInfoCommand(id, info);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteInfoCommand(id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
