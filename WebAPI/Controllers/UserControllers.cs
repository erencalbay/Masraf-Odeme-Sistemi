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

        [HttpGet("{id}")]
        public async Task<ApiResponse<UserResponse>> Get(int id)
        {
            var operation = new GetUserByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }

        // BURALARI SADECE ADMİN KULLANACAK VE PERSONEL OLUŞTURACAK
        [HttpPost]
        public async Task<ApiResponse<UserResponse>> CreatePersonel([FromBody] UserRequest user)
        {
            var operation = new CreateUserCommand(user);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse> Put(int id, [FromBody] UserRequest user)
        {
            var operation = new UpdateUserCommand(id, user);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteUserCommand(id); 
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
