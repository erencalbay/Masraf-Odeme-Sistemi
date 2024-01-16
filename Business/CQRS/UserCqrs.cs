using Base.Response;
using MediatR;
using Schema;


namespace Business.CQRS
{
    public record CreateUserCommand(UserRequest Model) : IRequest<ApiResponse<UserResponse>>;
    public record UpdateUserCommand(int Id, UserRequest Model) : IRequest<ApiResponse>;
    public record DeleteUserCommand(int Id) : IRequest<ApiResponse>;

    public record GetAllUserQuery() : IRequest<ApiResponse<List<UserResponse>>>;
    public record GetUserByIdQuery(int Id) : IRequest<ApiResponse<UserResponse>>;
    public record GetUserByParameterQuery(string FirstName, string LastName, string IdentityNumber) : IRequest<ApiResponse<List<UserResponse>>>;
}
