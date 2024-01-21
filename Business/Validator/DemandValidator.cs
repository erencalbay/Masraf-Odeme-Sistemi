using FluentValidation;
using Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validator
{
    public class DemandValidator : AbstractValidator<DemandRequest>
    {
        public DemandValidator()
        {
            RuleFor(x => x.UserNumber).NotEmpty().ExclusiveBetween(6,8);
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description of the Demand");
        }
    }
}
