using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSCoreV2.PaymentGetway
{
    public interface IPaymentClient
    {
        PaymentMethod PaymentMethod { get;}
        Task MakePayment(PaymentBilling paymentBilling);
    }
}
