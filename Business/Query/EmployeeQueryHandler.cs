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
    public class EmployeeQueryHandler :
    IRequestHandler<GetAllEmployeeQuery, ApiResponse<List<EmployeeResponse>>>,
    IRequestHandler<GetEmployeeByIdQuery, ApiResponse<EmployeeResponse>>,
    IRequestHandler<GetEmployeeByParameterQuery, ApiResponse<List<EmployeeResponse>>>
    {
        private readonly VdDbContext dbContext;
        private readonly IMapper mapper;

        public EmployeeQueryHandler(VdDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<List<EmployeeResponse>>> Handle(GetAllEmployeeQuery request, CancellationToken cancellationToken)
        {
            var list = await dbContext.Set<Employee>()
                .Include(x => x.Infos)
                .Include(x => x.Demands)
                .ToListAsync(cancellationToken);
            var mappedList = mapper.Map<List<Employee>, List<EmployeeResponse>>(list);
            return new ApiResponse<List<EmployeeResponse>>(mappedList);
        }

        public async Task<ApiResponse<EmployeeResponse>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Set<Employee>()
                .Include(x => x.Infos)
                .Include(x => x.Demands)
                .FirstOrDefaultAsync(x => x.EmployeeNumber == request.Id, cancellationToken);
            var mapped = mapper.Map<Employee, EmployeeResponse>(entity);
            return new ApiResponse<EmployeeResponse>(mapped);
        }

        public async Task<ApiResponse<List<EmployeeResponse>>> Handle(GetEmployeeByParameterQuery request, CancellationToken cancellationToken)
        {
            var list =  await dbContext.Set<Employee>()
                .Include(x => x.Infos)
                .Include(x => x.Demands)
                .Where(x =>
                x.FirstName.ToUpper().Contains(request.FirstName.ToUpper()) ||
                x.LastName.ToUpper().Contains(request.LastName.ToUpper()) ||
                x.IdentityNumber.ToUpper().Contains(request.IdentityNumber.ToUpper())
                ).ToListAsync(cancellationToken);
            var mappedList = mapper.Map<List<Employee>, List<EmployeeResponse>>(list);
            return new ApiResponse<List<EmployeeResponse>>(mappedList);
        }
    }
}
