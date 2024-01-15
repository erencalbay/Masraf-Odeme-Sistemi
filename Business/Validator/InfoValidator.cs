using FluentValidation;
using Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validator
{
    public class InfoValidator : AbstractValidator<InfoRequest>
    {
        public InfoValidator()
        {
            RuleFor(x => x.InfoType).NotEmpty();
            RuleFor(x => x.InfoNumber).NotEmpty();
            RuleFor(x => x.Information).NotEmpty();
        }
    }
}
