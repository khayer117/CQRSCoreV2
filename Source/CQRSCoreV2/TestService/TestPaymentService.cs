using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CQRSCoreV2.PaymentGetway;

namespace CQRSCoreV2.TestService
{
    public class TestPaymentService
    {
        private IContainer container;
        private PaymentService paymentService;

        public TestPaymentService(IContainer container)
        {
            this.container = container;
            this.paymentService = container.Resolve<PaymentService>();
        }

        public async Task Run()
        {
            await paymentService.ProcessPayment(PaymentMethod.SecurePay,
                new PaymentBilling() {Amount = 1005});
        }
    }
}
