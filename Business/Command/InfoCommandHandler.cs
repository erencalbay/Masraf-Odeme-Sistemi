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

        // UserId verilmesi ile yeni bir personel info açılması
        public async Task<ApiResponse<InfoResponse>> Handle(CreateInfoCommand request, CancellationToken cancellationToken)
        {
            
            // info duplicate number kontrolü
            var entity = mapper.Map<InfoRequest, Info>(request.Model);

            var infoNumber = new Random().Next(1000000, 9999999);
            var checkInfoNumber = await dbContext.Set<Info>().Where(x => x.InfoNumber == infoNumber)
            .FirstOrDefaultAsync(cancellationToken);

            if (checkInfoNumber != null)
            {
                Handle(request, cancellationToken);
            }

            //info number ataması
            entity.InfoNumber = infoNumber;

            var entityResult = await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            var mapped = mapper.Map<Info, InfoResponse>(entityResult.Entity);

            return new ApiResponse<InfoResponse>(mapped);
        }

        // Personelin infosunun güncellenmesi
        public async Task<ApiResponse> Handle(UpdateInfoCommand request, CancellationToken cancellationToken)
        {
            //info var mı yok mu kontrolü
            var fromdb = await dbContext.Set<Info>().Where(x => x.InfoNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (fromdb == null)
            {
                return new ApiResponse("Record Not Found");
            }

            //infonun yeni değerlerle güncellenmesi
            fromdb.Information = request.Model.Information;
            fromdb.IBAN = request.Model.IBAN;
            fromdb.isDefault = request.Model.isDefault;
            fromdb.InfoType = request.Model.InfoType;
            fromdb.UpdateDate = DateTime.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        // Personelin infosunun deactive duruma çevrilmesi-silinmesi
        public async Task<ApiResponse> Handle(DeleteInfoCommand request, CancellationToken cancellationToken)
        {
            // info var mı yok mu kontrolü
            var fromdb = await dbContext.Set<Info>().Where(x => x.InfoNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (fromdb == null)
            {
                return new ApiResponse("Record Not Found");
            }
            //infonun silinmek yerine deactive duruma çekilmesi
            fromdb.isActive = false;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }
    }
}
