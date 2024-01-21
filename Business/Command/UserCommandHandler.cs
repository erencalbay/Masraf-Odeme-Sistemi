using AutoMapper;
using Base.Response;
using Business.CQRS;
using Data.DbContextCon;
using Data.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
    public class UserCommandHandler :
    IRequestHandler<CreateUserCommand, ApiResponse<UserResponse>>,
    IRequestHandler<UpdateUserCommand, ApiResponse>,
    IRequestHandler<DeleteUserCommand, ApiResponse>
        
    {

        // dependency injection
        private readonly VdDbContext dbContext;
        private readonly IMapper mapper;
        public UserCommandHandler(VdDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        // Admin tarafından kullanıcı oluşturma
        public async Task<ApiResponse<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcı verinin alınması ve kontrol edilmesi
            var checkId = await dbContext.Set<User>().Where(x => x.IdentityNumber == request.Model.IdentityNumber)
            .FirstOrDefaultAsync(cancellationToken);

            if(checkId != null)
            {
                return new ApiResponse<UserResponse>($"{request.Model.IdentityNumber} is used by another user.");
            }

            // Userrequest ile user map edilmesi
            var entity = mapper.Map<UserRequest, User>(request.Model);
            entity.UserNumber = new Random().Next(100000, 999999);
            
            // Oluşturulacak olan personeller için employee rolünün alınması
            var role = dbContext.Roles.Where(x => x.Id == 2).FirstOrDefault();

            // Personellerin rolünün atınması
            entity.Roles.Add(new RoleUser { RoleId = role.Id, UserNumber = entity.UserNumber});
            var entityResult = await dbContext.Set<User>().AddAsync(entity, cancellationToken);
            
            // Save
            await dbContext.SaveChangesAsync(cancellationToken);
            var mapped = mapper.Map<User, UserResponse>(entity);

            return new ApiResponse<UserResponse>(mapped);
        }

        // Kullanıcının güncellenmesi
        public async Task<ApiResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcı verisinin alınması ve varlığının kontrol edilmesi
            var fromdb = await dbContext.Set<User>().Where(x => x.UserNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if(fromdb == null)
            {
                return new ApiResponse("Record Not Found");
            }

            // Verilerin güncellenmesi ve save edilmesi
            fromdb.FirstName = request.Model.FirstName;
            fromdb.LastName = request.Model.LastName;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        // Kullanıcı Silme
        public async Task<ApiResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcı verisinin alınması ve varlığının kontrol edilmesi
            var fromdb = await dbContext.Set<User>().Where(x => x.UserNumber == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (fromdb == null)
            {
                return new ApiResponse("Record Not Found");
            }

            // Verinin silinmesi ve save edilmesi
            fromdb.isActive = false;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }
    }
}
