﻿using Base.Response;
using Data.Enum;
using MediatR;
using Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CQRS
{
    public record CreateDemandCommand(DemandRequest Model) : IRequest<ApiResponse<DemandResponse>>;
    public record UpdateDemandCommandFromEmployee(int Id, DemandRequest Model) : IRequest<ApiResponse>;
    public record ResponseDemandCommandFromAdmin(int Id, DemandRequest Model) : IRequest<ApiResponse>;
    public record DeleteDemandCommand(int Id) : IRequest<ApiResponse>;

    public record GetAllDemandQuery() : IRequest<ApiResponse<List<DemandResponse>>>;
    public record GetDemandByIdQuery(int Id) : IRequest<ApiResponse<DemandResponse>>;
    public record GetDemandByParameterQuery(string Description, int DemandNumber, DemandType DemandType) : IRequest<ApiResponse<List<DemandResponse>>>;
}
