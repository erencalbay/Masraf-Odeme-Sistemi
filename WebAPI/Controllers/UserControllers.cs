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
    [Authorize(Roles = "admin")]
    public class UserControllers : ControllerBase
    {
        private readonly IMediator mediator;
        public UserControllers(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<List<UserResponse>>> Get()
        {
            var opr = new GetAllUserQuery();
            var result = await mediator.Send(opr);
            return result;
            
        }

        [HttpGet]
        public async Task<ApiResponse<UserResponse>> Get(int UserNumber)
        {
            var operation = new GetUserByIdQuery(UserNumber);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        public async Task<ApiResponse<UserResponse>> CreatePersonel([FromBody] UserRequest user)
        {
            var operation = new CreateUserCommand(user);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut]
        public async Task<ApiResponse> Put(int UserNumber, [FromBody] UserRequest user)
        {
            var operation = new UpdateUserCommand(UserNumber, user);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete]
        public async Task<ApiResponse> Delete(int UserNumber)
        {
            var operation = new DeleteUserCommand(UserNumber); 
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
