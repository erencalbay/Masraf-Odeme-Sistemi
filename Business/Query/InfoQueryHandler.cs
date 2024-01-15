using AutoMapper;
using Base.Response;
using Business.CQRS;
using Data.DbContextCon;
using Data.Entity;
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
    public class InfoQueryHandler :
    IRequestHandler<GetAllInfoQuery, ApiResponse<List<InfoResponse>>>,
    IRequestHandler<GetInfoByIdQuery, ApiResponse<InfoResponse>>,
    IRequestHandler<GetInfoByParameterQuery, ApiResponse<List<InfoResponse>>>
    {
        private readonly VdDbContext dbContext;
        private readonly IMapper mapper;

        public InfoQueryHandler(VdDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<ApiResponse<List<InfoResponse>>> Handle(GetAllInfoQuery request, CancellationToken cancellationToken)
        {
            var list = await dbContext.Set<Info>()
                .ToListAsync(cancellationToken);
            var mappedList = mapper.Map<List<Info>, List<InfoResponse>>(list);
            return new ApiResponse<List<InfoResponse>>(mappedList);
        }
        public async Task<ApiResponse<InfoResponse>> Handle(GetInfoByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Set<Info>()
                .FirstOrDefaultAsync(x => x.InfoNumber == request.Id, cancellationToken);
            var mapped = mapper.Map<Info, InfoResponse>(entity);
            return new ApiResponse<InfoResponse>(mapped);
        }
        public async Task<ApiResponse<List<InfoResponse>>> Handle(GetInfoByParameterQuery request, CancellationToken cancellationToken)
        {
            var list = await dbContext.Set<Info>()
                .Where(x =>
                x.IBAN.ToUpper().Contains(request.IBAN.ToUpper()) ||
                x.InfoNumber.ToString().ToUpper().Contains(request.InfoNumber.ToString().ToUpper()) ||
                x.Information.ToUpper().Contains(request.Information.ToUpper())
                ).ToListAsync(cancellationToken);
            var mappedList = mapper.Map<List<Info>, List<InfoResponse>>(list);
            return new ApiResponse<List<InfoResponse>>(mappedList);
        }
    }
}
