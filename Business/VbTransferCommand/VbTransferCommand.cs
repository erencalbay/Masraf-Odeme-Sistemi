using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Entity;

namespace Business.VbTransferCommand
{
    public record VbTransferCommand : IRequest<Employee>
    {


    }
}
