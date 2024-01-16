using AutoMapper;
using Base.Response;
using Business.CQRS;
using Data.DbContextCon;
using Data.Entity;
using Data.Enum;
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
    public class DemandCommandHandler :
    IRequestHandler<CreateDemandCommand, ApiResponse<DemandResponse>>,
    IRequestHandler<UpdateDemandCommandFromUser, ApiResponse>,
    IRequestHandler<ResponseDemandCommandFromAdmin, ApiResponse>,
    IRequestHandler<DeleteDemandCommand, ApiResponse>

    {
        private readonly VdDbContext dbContext;
        private readonly IMapper mapper;

        public DemandCommandHandler(VdDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<DemandResponse>> Handle(CreateDemandCommand request, CancellationToken cancellationToken)
        {
            var checkId = await dbContext.Set<Demand>().Where(x => x.DemandId == request.Model.DemandId)
            .FirstOrDefaultAsync(cancellationToken);

            if (checkId != null)
            {
                return new ApiResponse<DemandResponse>($"{request.Model.DemandId} is exist.");
            }
            var entity = mapper.Map<DemandRequest, Demand>(request.Model);
            entity.DemandNumber = new Random().Next(1000000, 9999999);

            var entityResult = await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            var mapped = mapper.Map<Demand, DemandResponse>(entityResult.Entity);

            return new ApiResponse<DemandResponse>(mapped);
        }

        public async Task<ApiResponse> Handle(UpdateDemandCommandFromUser request, CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<Demand>().Where(x => x.DemandNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (fromdb == null)
            {
                return new ApiResponse("Record Not Found");
            }

            fromdb.Description = request.Model.Description;
            fromdb.DemandType = DemandType.Pending;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(ResponseDemandCommandFromAdmin request, CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<Demand>().Where(x => x.DemandNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (fromdb == null)
            {
                return new ApiResponse("Record Not Found");
            }

            fromdb.RejectionResponse = request.Model.RejectionResponse;
            fromdb.DemandType = request.Model.DemandType;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(DeleteDemandCommand request, CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<Demand>().Where(x => x.DemandNumber == request.Id)
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
