using AutoMapper;
using Base.Response;
using Business.Consumers;
using Business.CQRS;
using Business.Services.EftPaymentService;
using Data.DbContextCon;
using Data.Entity;
using Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Entity;

namespace Business.Command
{
    public class DemandCommandHandler :
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

        //old demand create
        /*
        public async Task<ApiResponse<DemandResponse>> Handle(CreateDemandCommand request, CancellationToken cancellationToken)
        {
            var demandNumber = new Random().Next(1000000, 9999999);
            var checkDemandNumber = await dbContext.Set<Demand>().Where(x => x.DemandNumber == demandNumber)
            .FirstOrDefaultAsync(cancellationToken);

            if (checkDemandNumber != null)
            {
                Handle(request, cancellationToken);
            }
            var entity = mapper.Map<DemandRequest, Demand>(request.Model);
            entity.DemandNumber = demandNumber;
            entity.DemandResponseType = DemandResponseType.Pending;
            //entity.Receipt = receipt;

            Log.Information($"Demand is with Number: {demandNumber} created by {request.Model.UserNumber}");

            var entityResult = await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
            var mapped = mapper.Map<Demand, DemandResponse>(entityResult.Entity);

            return new ApiResponse<DemandResponse>(mapped);
        }
        */

        public async Task<ApiResponse> Handle(UpdateDemandCommandFromUser request, CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<Demand>().Where(x => x.DemandNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (fromdb == null)
            {
                var message = "Record Not Found";
                Log.Error(message);
                return new ApiResponse(message);
            }

            fromdb.Description = request.Model.Description;
            fromdb.DemandResponseType = DemandResponseType.Pending;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(ResponseDemandCommandFromAdmin request, CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<Demand>().Where(x => x.DemandNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (fromdb == null)
            {
                var message = "Record Not Found";
                Log.Error(message);
                return new ApiResponse(message);
            }

            fromdb.RejectionResponse = request.Model.RejectionResponse;
            fromdb.DemandResponseType = request.Model.DemandResponseType;
            fromdb.isActive = false;
            
            if (fromdb.DemandResponseType == DemandResponseType.Approval)
            {
                EftPaymentService eftPaymentService = new EftPaymentService(dbContext);
                eftPaymentService.PaymentAfterApproval(request);
            }
            // todo: eğer demandtype approval ise if durumuyla eft yollanacak

            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(DeleteDemandCommand request, CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<Demand>().Where(x => x.DemandNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (fromdb == null)
            {
                var message = "Record Not Found";
                Log.Error(message);
                return new ApiResponse(message);
            }
            //dbContext.Set<User>().Remove(fromdb);
            fromdb.isActive = false;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }
    }
}
