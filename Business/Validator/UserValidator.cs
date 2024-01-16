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
        public CreateUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(30);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Email).EmailAddress().MaximumLength(50);
            RuleFor(x => x.DateOfBirth).NotEmpty();
            RuleFor(x => x.IdentityNumber).NotEmpty().MaximumLength(11);

            RuleFor(x => x.Infos).ForEach(x =>
            {
                //x.SetValidator(new CreateInfosValidator());
            });
        }
    }
}
