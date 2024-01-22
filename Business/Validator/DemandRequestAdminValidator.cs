using Data.Enum;
using FluentValidation;
using Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validator
{
    public class DemandRequestAdminValidator : AbstractValidator<DemandRequestFromAdmin>
    {
        // Talep requestin validasyonu
        public DemandRequestAdminValidator()
        {
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0).WithMessage("Amount cannot be null");
            RuleFor(x => x.UserNumber).NotNull();
            RuleFor(x => x.RejectionResponse).MinimumLength(15).WithMessage("Must be lenght of digit bigger than 15").NotEmpty().WithMessage("It cant be null");
            RuleFor(x => x.DemandResponseType).IsInEnum().NotEmpty().WithMessage("Response type");
        }
    }
}
