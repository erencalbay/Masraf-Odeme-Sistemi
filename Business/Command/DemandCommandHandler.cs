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

        // dependency injection
        private readonly VdDbContext dbContext;
        private readonly IMapper mapper;

        public DemandCommandHandler(VdDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        // Talep Güncelleme işlemi
        public async Task<ApiResponse> Handle(UpdateDemandCommandFromUser request, CancellationToken cancellationToken)
        {
            // Talebin db'den alınması
            var fromdb = await dbContext.Set<Demand>().Where(x => x.DemandNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            // Talebin varlığının kontrol edilmesi
            if (fromdb == null)
            {
                var message = "Record Not Found";
                Log.Error(message);
                return new ApiResponse(message);
            }

            // Güncellenen değerlerleverinin güncellenmesi ve save edilmesi
            fromdb.Description = request.Model.Description;
            fromdb.DemandResponseType = DemandResponseType.Pending;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        // Talebe Cevap Verme
        public async Task<ApiResponse> Handle(ResponseDemandCommandFromAdmin request, CancellationToken cancellationToken)
        {
            // Talebin alınması

            var fromdb = await dbContext.Set<Demand>().Where(x => x.DemandNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            // Cevap verilecek kayıt yoksa hata verir

            if (fromdb == null)
            {
                var message = "Record Not Found";
                Log.Error(message);
                return new ApiResponse(message);
            }

            // Request ile gelen değerlerin atanması 

            fromdb.RejectionResponse = request.Model.RejectionResponse;
            fromdb.DemandResponseType = request.Model.DemandResponseType;
            fromdb.isActive = false;
            
            // Eğer talep onaylanırsa eft ile info'ya erişilecek ve iban yoluyla miktar yatırılacak
            if (fromdb.DemandResponseType == DemandResponseType.Approval)
            {
                EftPaymentService eftPaymentService = new EftPaymentService(dbContext);
                eftPaymentService.PaymentAfterApproval(request);
            }

            // Her şey okeyse save edilmesi.
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        // Talebi Silme
        public async Task<ApiResponse> Handle(DeleteDemandCommand request, CancellationToken cancellationToken)
        {
            // Talebin database'den alınması ve varlığının kontrol edilmesi
            var fromdb = await dbContext.Set<Demand>().Where(x => x.DemandNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (fromdb == null)
            {
                var message = "Record Not Found";
                Log.Error(message);
                return new ApiResponse(message);
            }
            // Talebi deactive etme ve save edilmesi.
            fromdb.isActive = false;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }
    }
}
