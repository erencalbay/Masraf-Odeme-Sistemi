using AutoMapper;
using Base.Response;
using Business.CQRS;
using Data.DbContextCon;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Entity;

namespace Business.Command
{
    public class UserCommandHandler :
    IRequestHandler<CreateUserCommand, ApiResponse<UserResponse>>,
    IRequestHandler<UpdateUserCommand, ApiResponse>,
    IRequestHandler<DeleteUserCommand, ApiResponse>

    {
        private readonly VdDbContext dbContext;
        private readonly IMapper mapper;

        public UserCommandHandler(VdDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var checkId = await dbContext.Set<User>().Where(x => x.IdentityNumber == request.Model.IdentityNumber)
            .FirstOrDefaultAsync(cancellationToken);

            if(checkId != null)
            {
                return new ApiResponse<UserResponse>($"{request.Model.IdentityNumber} is used by another user.");
            }
            var entity = mapper.Map<UserRequest, User>(request.Model);
            entity.UserNumber = new Random().Next(100000, 999999);

            var entityResult = await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            var mapped = mapper.Map<User, UserResponse>(entityResult.Entity);

            return new ApiResponse<UserResponse>(mapped);
        }

        public async Task<ApiResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<User>().Where(x => x.UserNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if(fromdb == null)
            {
                return new ApiResponse("Record Not Found");
            }

            fromdb.FirstName = request.Model.FirstName;
            fromdb.LastName = request.Model.LastName;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<User>().Where(x => x.UserNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (fromdb == null)
            {
                return new ApiResponse("Record Not Found");
            }
            //dbContext.Set<User>().Remove(fromdb);
            fromdb.isActive = false;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }
    }
}
