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
    public class EmployeeCommandHandler :
    IRequestHandler<CreateEmployeeCommand, ApiResponse<EmployeeResponse>>,
    IRequestHandler<UpdateEmployeeCommand, ApiResponse>,
    IRequestHandler<DeleteEmployeeCommand, ApiResponse>

    {
        private readonly VdDbContext dbContext;
        private readonly IMapper mapper;

        public EmployeeCommandHandler(VdDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<EmployeeResponse>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var checkId = await dbContext.Set<Employee>().Where(x => x.IdentityNumber == request.Model.IdentityNumber)
            .FirstOrDefaultAsync(cancellationToken);

            if(checkId != null)
            {
                return new ApiResponse<EmployeeResponse>($"{request.Model.IdentityNumber} is used by another employee.");
            }
            var entity = mapper.Map<EmployeeRequest, Employee>(request.Model);
            entity.EmployeeNumber = new Random().Next(100000, 999999);

            var entityResult = await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            var mapped = mapper.Map<Employee, EmployeeResponse>(entityResult.Entity);

            return new ApiResponse<EmployeeResponse>(mapped);
        }

        public async Task<ApiResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<Employee>().Where(x => x.EmployeeNumber == request.Id)
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

        public async Task<ApiResponse> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<Employee>().Where(x => x.EmployeeNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (fromdb == null)
            {
                return new ApiResponse("Record Not Found");
            }
            //dbContext.Set<Customer>().Remove(fromdb);
            fromdb.isActive = false;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }
    }
}
