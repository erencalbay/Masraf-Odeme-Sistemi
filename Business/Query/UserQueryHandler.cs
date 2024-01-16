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

namespace Business.Query
{
    public class UserQueryHandler :
    IRequestHandler<GetAllUserQuery, ApiResponse<List<UserResponse>>>,
    IRequestHandler<GetUserByIdQuery, ApiResponse<UserResponse>>,
    IRequestHandler<GetUserByParameterQuery, ApiResponse<List<UserResponse>>>
    {
        private readonly VdDbContext dbContext;
        private readonly IMapper mapper;

        public UserQueryHandler(VdDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<List<UserResponse>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var list = await dbContext.Set<User>()
                .Include(x => x.Infos)
                .Include(x => x.Demands)
                .ToListAsync(cancellationToken);
            var mappedList = mapper.Map<List<User>, List<UserResponse>>(list);
            return new ApiResponse<List<UserResponse>>(mappedList);
        }

        public async Task<ApiResponse<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Set<User>()
                .Include(x => x.Infos)
                .Include(x => x.Demands)
                .FirstOrDefaultAsync(x => x.UserNumber == request.Id, cancellationToken);
            var mapped = mapper.Map<User, UserResponse>(entity);
            return new ApiResponse<UserResponse>(mapped);
        }

        public async Task<ApiResponse<List<UserResponse>>> Handle(GetUserByParameterQuery request, CancellationToken cancellationToken)
        {
            var list =  await dbContext.Set<User>()
                .Include(x => x.Infos)
                .Include(x => x.Demands)
                .Where(x =>
                x.FirstName.ToUpper().Contains(request.FirstName.ToUpper()) ||
                x.LastName.ToUpper().Contains(request.LastName.ToUpper()) ||
                x.IdentityNumber.ToUpper().Contains(request.IdentityNumber.ToUpper())
                ).ToListAsync(cancellationToken);
            var mappedList = mapper.Map<List<User>, List<UserResponse>>(list);
            return new ApiResponse<List<UserResponse>>(mappedList);
        }
    }
}
