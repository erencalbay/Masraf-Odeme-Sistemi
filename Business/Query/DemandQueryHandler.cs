﻿using AutoMapper;
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
    public class DemandQueryHandler :
    IRequestHandler<GetAllDemandQuery, ApiResponse<List<DemandResponse>>>,
    IRequestHandler<GetDemandByIdQuery, ApiResponse<DemandResponse>>,
    IRequestHandler<GetDemandByParameterQuery, ApiResponse<List<DemandResponse>>>,
    IRequestHandler<GetEmployeeDemandQuery, ApiResponse<List<DemandResponse>>>,
    IRequestHandler<GetAllNotActiveDemandQuery, ApiResponse<List<DemandResponse>>>
    {

        // Dependency Injection
        private readonly VdDbContext dbContext;
        private readonly IMapper mapper;

        public DemandQueryHandler(VdDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        // Bütün Aktif Taleplerin admin tarafından cevaplanmak üzere alınması
        public async Task<ApiResponse<List<DemandResponse>>> Handle(GetAllDemandQuery request, CancellationToken cancellationToken)
        {
            var list = await dbContext.Set<Demand>()
                .Where(x => x.isActive == true)
                .ToListAsync(cancellationToken);
            var mappedList = mapper.Map<List<Demand>, List<DemandResponse>>(list);
            return new ApiResponse<List<DemandResponse>>(mappedList);
        }

        // Bütün Pasif Taleplerin admin tarafından cevaplanmak üzere alınması
        public async Task<ApiResponse<List<DemandResponse>>> Handle(GetAllNotActiveDemandQuery request, CancellationToken cancellationToken)
        {
            var list = await dbContext.Set<Demand>()
                .Where(x => x.isActive == false)
                .ToListAsync(cancellationToken);
            var mappedList = mapper.Map<List<Demand>, List<DemandResponse>>(list);
            return new ApiResponse<List<DemandResponse>>(mappedList);
        }

        // Bütün Aktif Taleplerin arasından Talep numarası ile alınması
        public async Task<ApiResponse<DemandResponse>> Handle(GetDemandByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Set<Demand>()
                .FirstOrDefaultAsync(x => x.DemandNumber == request.Id, cancellationToken);
            var mapped = mapper.Map<Demand, DemandResponse>(entity);
            return new ApiResponse<DemandResponse>(mapped);
        }

        // Taleplerin kullanıcı tarafından filtrelenmesi
        public async Task<ApiResponse<List<DemandResponse>>> Handle(GetDemandByParameterQuery request, CancellationToken cancellationToken)
        {
            var list = await dbContext.Set<Demand>()
                .Where(x =>
                x.Description.Contains(request.Description) ||
                x.DemandNumber.ToString().Contains(request.DemandNumber.ToString()) ||
                x.DemandResponseType.Equals(request.DemandType)
                ).ToListAsync(cancellationToken);
            var mappedList = mapper.Map<List<Demand>, List<DemandResponse>>(list);
            return new ApiResponse<List<DemandResponse>>(mappedList);
        }

        // Personelin taleplerinin alınması
        public async Task<ApiResponse<List<DemandResponse>>> Handle(GetEmployeeDemandQuery request, CancellationToken cancellationToken)
        {
            var list = await dbContext.Set<Demand>()
                .Where(x => x.User.UserNumber == request.UserNumber).ToListAsync(cancellationToken);

            var mappedList = mapper.Map<List<Demand>, List<DemandResponse>>(list);
            return new ApiResponse<List<DemandResponse>>(mappedList);
                
        }
    }
}
