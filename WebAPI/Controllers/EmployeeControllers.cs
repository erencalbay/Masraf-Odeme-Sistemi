using AutoMapper;
using Base.Response;
using Business.CQRS;
using Data.DbContextCon;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeControllers : ControllerBase
    {
        private readonly IMediator mediator;
        public EmployeeControllers(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<List<EmployeeResponse>>> Get()
        {
            var opr = new GetAllEmployeeQuery();
            var result = await mediator.Send(opr);
            return result;
            
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<EmployeeResponse>> Get(int id)
        {
            var operation = new GetEmployeeByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        public async Task<ApiResponse<EmployeeResponse>> Post([FromBody] EmployeeRequest employee)
        {
            var operation = new CreateEmployeeCommand(employee);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse> Put(int id, [FromBody] EmployeeRequest employee)
        {
            var operation = new UpdateEmployeeCommand(id, employee);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteEmployeeCommand(id); 
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
