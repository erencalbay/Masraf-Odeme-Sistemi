using Base.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Consumers
{
    public class ReceiptsEventConsumer : IConsumer<ReceiptsEvent>
    {
        public Task Consume(ConsumeContext<ReceiptsEvent> context)
        {
            var message = context.Message;
            return Task.CompletedTask;
        }
    }
}
