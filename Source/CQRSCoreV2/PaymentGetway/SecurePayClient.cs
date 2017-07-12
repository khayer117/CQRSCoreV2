using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSCoreV2.PaymentGetway
{
    public class SecurePayClient:IPaymentClient
    {
        public PaymentMethod PaymentMethod  => PaymentMethod.SecurePay;
        public async Task MakePayment(PaymentBilling paymentBilling)
        {
            Console.WriteLine("Secure Client:" + paymentBilling.Amount); ;
        }
    }
}
