using Base.Response;
using Business.CQRS;
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
        // admin kullanacak ve aktif olan talepleri görecek
        [HttpGet]
        [ActionName("GetAllActiveDemand")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<DemandResponse>>> GetAllActiveDemand()
        {
            var opr = new GetAllDemandQuery();
            var result = await mediator.Send(opr);
            return result;
        }
        // admin kullanacak ve aktif olmayan talepleri görecek
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<DemandResponse>>> GetAllNotActiveDemand()
        {
            var opr = new GetAllNotActiveDemandQuery();
            var result = await mediator.Send(opr);
            return result;
        }
        // personel kullanacak ve taleplerini görecek
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

        // admin kullanacak ve id ile talebi görecek
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<DemandResponse>> Get(int id)
        {
            var operation = new GetDemandByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }
        // Talepleri filtreleme ile kullanıcının görmesi sağlanacak aynı zamanda adminin de.
        [HttpGet]
        public async Task<ApiResponse<List<DemandResponse>>> GetDemandsWithFiltering(string? Description, int? DemandNumber, DemandResponseType? DemandType)
        {
            
            var opr = new GetDemandByParameterQuery(Description, DemandNumber, DemandType);
            var result = await mediator.Send(opr);
            return result;
        }

        // admin kullanacak ve talebi cevaplayacak
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> ExpenseResponseFromAdmin(int DemandNumber, [FromQuery] DemandRequestFromAdmin Demand)
        {
            var operation = new ResponseDemandCommandFromAdmin(DemandNumber, Demand);
            var result = await mediator.Send(operation);
            return result;
        }
        // admin kullanacak ve talebi silecek
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
