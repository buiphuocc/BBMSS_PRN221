using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class VnPayPaymentModel
    {
        public string OrderId { get; set; }
        public string Description { get; set; }
        public string TransactionId { get; set; }
        public string PaymentMethodName { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }

        public long Amount { get; set; }

        public DateTime PayDate { get; set; }

    }
}
