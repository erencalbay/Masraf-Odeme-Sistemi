using Base.Response;
using Business.CQRS;
using Business.Services.FileUploadService;
using Data.DbContextCon;
using Data.Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schema;
using WebAPI.Entity;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DemandControllers : ControllerBase
    {
        private readonly IMediator mediator;
        public DemandControllers(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // ADMİN KULLANACAK VE TÜM TALEPLERİ GÖRECEK
        [HttpGet]
        public async Task<ApiResponse<List<DemandResponse>>> Get()
        {
            var opr = new GetAllDemandQuery();
            var result = await mediator.Send(opr);
            return result;
        }
        // PERSONEL KULLANACAK
        [HttpGet]
        [ActionName("EmployeeDemands")]
        public async Task<ApiResponse<List<DemandResponse>>> GetEmployeeDemand(int UserNumber)
        {
            var opr = new GetEmployeeDemandQuery(UserNumber);
            var result = await mediator.Send(opr);
            return result;
        }

        // ADMİN KULLANACAK VE TÜM TALEPLERİ GÖRECEK DEMAND ID İLE
        [HttpGet("{id}")]
        public async Task<ApiResponse<DemandResponse>> Get(int id)
        {
            var operation = new GetDemandByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }
        // PERSONEL KULLANACAK talep oluşturacak
        [HttpPost]
        public async Task<ApiResponse<DemandResponse>> ExpenseEntryFromEmployee([FromQuery] DemandRequest Demand)
        {
            var operation = new CreateDemandCommand(Demand);
            var result = await mediator.Send(operation);
            return result;
        }


        // ADMİN KULLANACAK ve cevap verecek
        [HttpPut("{id}")]
        public async Task<ApiResponse> ExpenseResponseFromAdmin(int id, [FromBody] DemandRequestFromAdmin Demand)
        {
            var operation = new ResponseDemandCommandFromAdmin(id, Demand);
            var result = await mediator.Send(operation);
            return result;
        }

        // ADMİN KULLANACAK
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteDemandCommand(id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
