using Business.Query;
using FluentValidation;
using Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validator
{
    public class CreateUserValidator : AbstractValidator<UserRequest>
    {
        // Kullanıcının oluşturulması validasyonu
        public CreateUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(30).MinimumLength(2).WithName("User firstname");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(30).MinimumLength(2).WithName("User surname");
            RuleFor(x => x.Email).EmailAddress().MaximumLength(50).MinimumLength(5).WithMessage("Must be valid email");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithName("Birthday of the user");
            RuleFor(x => x.IdentityNumber).NotEmpty().Length(11).WithName("User TC");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        }
    }
}
