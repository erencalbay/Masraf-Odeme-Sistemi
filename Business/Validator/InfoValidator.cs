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
        // Kullanıcı infolarının-banka bilgilerinin validasyonu
        public InfoValidator()
        {
            RuleFor(x => x.InfoType).NotEmpty().WithName("Type of info like a account type");
            RuleFor(x => x.Information).NotEmpty().MaximumLength(300).WithName("Information can not be empty and greater than 300");
            RuleFor(x => x.IBAN).NotEmpty().Length(26).WithName("IBAN of the account");
            RuleFor(x => x.UserNumber).NotEmpty();
        }
    }
}
