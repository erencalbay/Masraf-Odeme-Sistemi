using AutoMapper;
using Base.Events;
using Base.Response;
using Business.Command;
using Business.CQRS;
using Data.DbContextCon;
using Data.Entity;
using Data.Enum;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Schema;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Entity;

namespace Business.Consumers
{
    public class ReceiptsEventConsumer : IConsumer<ReceiptsEvent>
    {
        // Dependency Injection
        private readonly IMediator mediator;
        private readonly VdDbContext dbContext;
        private readonly IMapper mapper;
      
        public ReceiptsEventConsumer(IMediator mediator, VdDbContext dbContext, IMapper mapper)
        {
            this.mediator = mediator;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }


        // RabbitMQ ile Consume edilmesi
        public async Task Consume(ConsumeContext<ReceiptsEvent> context)
        {
            // Verinin alınması
            var message = context.Message;
            DemandRequest demandRequest = new DemandRequest { Description = message.Description, Receipt = message.Path, UserNumber = message.UserNumber };

            var demandNumber = new Random().Next(1000000, 9999999);

            var entity = mapper.Map<DemandRequest, Demand>(demandRequest);
            entity.DemandNumber = demandNumber;
            entity.DemandResponseType = DemandResponseType.Pending;

            Log.Information($"Demand is with Number: {demandNumber} created by {message.UserNumber}");

            var entityResult = await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            var mapped = mapper.Map<Demand, DemandResponse>(entityResult.Entity);
        }
    }
}
