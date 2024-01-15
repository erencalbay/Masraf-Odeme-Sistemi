using Base.Response;
using MediatR;
using Schema;


namespace Business.CQRS
{
    public record CreateEmployeeCommand(EmployeeRequest Model) : IRequest<ApiResponse<EmployeeResponse>>;
    public record UpdateEmployeeCommand(int Id, EmployeeRequest Model) : IRequest<ApiResponse>;
    public record DeleteEmployeeCommand(int Id) : IRequest<ApiResponse>;

    public record GetAllEmployeeQuery() : IRequest<ApiResponse<List<EmployeeResponse>>>;
    public record GetEmployeeByIdQuery(int Id) : IRequest<ApiResponse<EmployeeResponse>>;
    public record GetEmployeeByParameterQuery(string FirstName, string LastName, string IdentityNumber) : IRequest<ApiResponse<List<EmployeeResponse>>>;
}
