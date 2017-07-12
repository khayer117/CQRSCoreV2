using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSCoreV2.PaymentGetway
{
    public interface IPaymentService
    {
        Task ProcessPayment(PaymentMethod paymentMethod, PaymentBilling paymentBilling);
    }
    public class PaymentService:IPaymentService
    {
        private Func<PaymentMethod, IPaymentClient> paymentClientFactory;

        public PaymentService(Func<PaymentMethod,IPaymentClient> paymentClientFactory)
        {
            this.paymentClientFactory = paymentClientFactory;
        }
        public async Task ProcessPayment(PaymentMethod paymentMethod, PaymentBilling paymentBilling)
        {
            var paymentGetwayClient = paymentClientFactory(paymentMethod);
            await paymentGetwayClient.MakePayment(paymentBilling);
        }
    }
}
