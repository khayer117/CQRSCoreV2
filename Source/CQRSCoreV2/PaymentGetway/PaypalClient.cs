using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSCoreV2.PaymentGetway
{
    public class PaypalClient:IPaymentClient
    {
        public PaymentMethod PaymentMethod => PaymentMethod.PayPal;

        public async Task MakePayment(PaymentBilling paymentBilling)
        {
            Console.WriteLine("Paypal Client:" + paymentBilling.Amount);
        }
    }
}
