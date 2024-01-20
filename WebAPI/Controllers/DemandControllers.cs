using Base.Response;
using Business.CQRS;
using Business.Services.FileUploadService;
using Data.DbContextCon;
using Data.Entity;
using Data.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schema;
using System.Security.Claims;
using WebAPI.Entity;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DemandControllers : ControllerBase
    {
        private readonly IMediator mediator;
        public DemandControllers(IMediator mediator)
        {
            this.mediator = mediator;
        }
        // ADMİN KULLANACAK VE TÜM TALEPLERİ GÖRECEK
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<DemandResponse>>> GetAllActiveDemand()
        {
            var opr = new GetAllDemandQuery();
            var result = await mediator.Send(opr);
            return result;
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<DemandResponse>>> GetAllNotActiveDemand()
        {
            var opr = new GetAllNotActiveDemandQuery();
            var result = await mediator.Send(opr);
            return result;
        }
        // PERSONEL KULLANACAK
        [HttpGet]
        [ActionName("EmployeeDemands")]
        [Authorize(Roles = "employee")]
        public async Task<ApiResponse<List<DemandResponse>>> GetEmployeeDemand()
        {
            var UserNumber = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
            var opr = new GetEmployeeDemandQuery(int.Parse(UserNumber));

            var result = await mediator.Send(opr);
            return result;
        }
        // ADMİN KULLANACAK VE TÜM TALEPLERİ GÖRECEK DEMAND ID İLE
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<DemandResponse>> Get(int id)
        {
            var operation = new GetDemandByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }
        [HttpGet]
        public async Task<ApiResponse<List<DemandResponse>>> GetDemandsWithFiltering(string? Description, int? DemandNumber, DemandType? DemandType)
        {
            var opr = new GetDemandByParameterQuery(Description, DemandNumber, DemandType);
            var result = await mediator.Send(opr);
            return result;
        }
        // PERSONEL KULLANACAK talep oluşturacak
        [HttpPost]
        public async Task<ApiResponse<DemandResponse>> ExpenseEntryFromEmployee([FromQuery] Deneme Demand)
        {
            var userNumber = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
            var operation = new CreateDemandCommand(new DemandRequest { Description = Demand.Description, Receipt = Demand.Receipt, UserNumber = int.Parse(userNumber) });
            var result = await mediator.Send(operation);
            return result;
        }
        // ADMİN KULLANACAK ve cevap verecek
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> ExpenseResponseFromAdmin(int DemandNumber, [FromQuery] DemandRequestFromAdmin Demand)
        {
            var operation = new ResponseDemandCommandFromAdmin(DemandNumber, Demand);
            var result = await mediator.Send(operation);
            return result;
        }
        // ADMİN KULLANACAK
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> Delete(int DemandNumber)
        {
            var operation = new DeleteDemandCommand(DemandNumber);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
