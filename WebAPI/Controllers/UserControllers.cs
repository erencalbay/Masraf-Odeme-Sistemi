using AutoMapper;
using Base.Response;
using Business.CQRS;
using Data.DbContextCon;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schema;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserControllers : ControllerBase
    {
        //Dependency injection
        private readonly IMediator mediator;
        public UserControllers(IMediator mediator)
        {
            this.mediator = mediator;
        }


        // Kullanıcıların hepsini getirme
        [HttpGet]
        public async Task<ApiResponse<List<UserResponse>>> GetAllEmployees()
        {
            var opr = new GetAllUserQuery();
            var result = await mediator.Send(opr);
            return result;
            
        }

        // Kullanıcıları usernumber'a göre getirme
        [HttpGet]
        [Route("GetEmployeeWithID")]
        public async Task<ApiResponse<UserResponse>> GetEmployeeWithID(int UserNumber)
        {
            var operation = new GetUserByIdQuery(UserNumber);
            var result = await mediator.Send(operation);
            return result;
        }

        // Kullanıcılar admin tarafından oluşturulacak
        [HttpPost]
        public async Task<ApiResponse<UserResponse>> CreateEmployee([FromBody] UserRequest user)
        {
            var operation = new CreateUserCommand(user);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut]
        public async Task<ApiResponse> UpdateEmployee(int UserNumber, [FromBody] UserRequest user)
        {
            var operation = new UpdateUserCommand(UserNumber, user);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete]
        public async Task<ApiResponse> DeleteEmployee(int UserNumber)
        {
            var operation = new DeleteUserCommand(UserNumber); 
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
