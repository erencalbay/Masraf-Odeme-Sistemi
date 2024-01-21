using Base.Response;
using MediatR;
using Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Info Command Query Responsibility Segregation
namespace Business.CQRS
{
    public record CreateInfoCommand(InfoRequest Model) : IRequest<ApiResponse<InfoResponse>>;
    public record UpdateInfoCommand(int Id, InfoRequest Model) : IRequest<ApiResponse>;
    public record DeleteInfoCommand(int Id) : IRequest<ApiResponse>;

    public record GetAllInfoQuery() : IRequest<ApiResponse<List<InfoResponse>>>;
    public record GetInfoByIdQuery(int Id) : IRequest<ApiResponse<InfoResponse>>;
    public record GetUserInfosByIdQuery(int? Id) : IRequest<ApiResponse<List<InfoResponse>>>;
    public record GetInfoByParameterQuery(string IBAN, string InfoNumber, string Information) : IRequest<ApiResponse<List<InfoResponse>>>;
}
