using Business.CQRS;
using Data.DbContextCon;
using Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.EftPaymentService
{
    
    public class EftPaymentService
    {
        // Dependency Injection
        public readonly VdDbContext _vdDbContext;

        public EftPaymentService(VdDbContext vdDbContext)
        {
            _vdDbContext = vdDbContext;
        }

        // IBANa göre personelin hesabına paranın atılma işlemi
        public async Task PaymentAfterApproval(ResponseDemandCommandFromAdmin request)
        {
            // Kontrol işlemleri
            if(request == null || request.Model.Amount == null || request.Model.DemandResponseType != Data.Enum.DemandResponseType.Approval)
            {
                throw new Exception("Invalid request");
            }
            // Usernumber ile personelin infosunun alınması
            var userNumber = request.Model.UserNumber;
            var userInfo = await _vdDbContext.Set<Info>().Where(x => x.UserNumber == request.Model.UserNumber && x.isDefault).FirstOrDefaultAsync();
            
            // Userinfo varlığının kontrol edilmesi
            if(userInfo == null)
            {
                throw new Exception("User info not found");
            }

            // iban ve amount ataması
            var IBAN = userInfo.IBAN;
            var Amount = request.Model.Amount;
            //Buradan sonra bankanın API'sine? veya bankaya iletilecek bir komut ile IBAN ve miktar bilgileri ile personelin ana hesabına para akışı sağlanabilir.
            //sendToBankForPayment()
        }
    }
}
