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

namespace Business.Command
{
    public class InfoCommandHandler :
    IRequestHandler<CreateInfoCommand, ApiResponse<InfoResponse>>,
    IRequestHandler<UpdateInfoCommand, ApiResponse>,
    IRequestHandler<DeleteInfoCommand, ApiResponse>

    {
        // dependency injection
        private readonly VdDbContext dbContext;
        private readonly IMapper mapper;

        public InfoCommandHandler(VdDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<InfoResponse>> Handle(CreateInfoCommand request, CancellationToken cancellationToken)
        {
            
            var entity = mapper.Map<InfoRequest, Info>(request.Model);

            var infoNumber = new Random().Next(1000000, 9999999);
            var checkInfoNumber = await dbContext.Set<Info>().Where(x => x.InfoNumber == infoNumber)
            .FirstOrDefaultAsync(cancellationToken);

            if (checkInfoNumber != null)
            {
                Handle(request, cancellationToken);
            }

            //entity.InfoNumber = infoNumber;

            var entityResult = await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            var mapped = mapper.Map<Info, InfoResponse>(entityResult.Entity);

            return new ApiResponse<InfoResponse>(mapped);
        }

        public async Task<ApiResponse> Handle(UpdateInfoCommand request, CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<Info>().Where(x => x.InfoNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (fromdb == null)
            {
                return new ApiResponse("Record Not Found");
            }

            fromdb.Information = request.Model.Information;
            fromdb.IBAN = request.Model.IBAN;
            fromdb.isDefault = request.Model.isDefault;
            fromdb.InfoType = request.Model.InfoType;
            fromdb.UpdateDate = DateTime.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(DeleteInfoCommand request, CancellationToken cancellationToken)
        {
            var fromdb = await dbContext.Set<Info>().Where(x => x.InfoNumber == request.Id)
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
