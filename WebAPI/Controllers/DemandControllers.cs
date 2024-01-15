using Base.Response;
using Business.CQRS;
using Data.DbContextCon;
using Data.Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schema;
using WebAPI.Entity;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandControllers : ControllerBase
    {
        private readonly IMediator mediator;
        public DemandControllers(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<List<DemandResponse>>> Get()
        {
            var opr = new GetAllDemandQuery();
            var result = await mediator.Send(opr);
            return result;

        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<DemandResponse>> Get(int id)
        {
            var operation = new GetDemandByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        public async Task<ApiResponse<DemandResponse>> Post([FromBody] DemandRequest Demand)
        {
            var operation = new CreateDemandCommand(Demand);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse> Put(int id, [FromBody] DemandRequest Demand)
        {
            var operation = new UpdateDemandCommandFromEmployee(id, Demand);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteDemandCommand(id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
