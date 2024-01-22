using FluentValidation;
using Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validator
{
    public class InfoUpdateRequestValidator : AbstractValidator<InfoUpdateRequest>
    {
        // InfoUpdate'in validatoru
        public InfoUpdateRequestValidator() 
        { 
            RuleFor(x => x.InfoNumber).NotEmpty().GreaterThan(999999).LessThan(10000000).WithMessage("InfoNumber is not valid");
            RuleFor(x => x.Information).MinimumLength(5).NotEmpty().WithMessage("Informatin cannot be null");
        }
    }
}
