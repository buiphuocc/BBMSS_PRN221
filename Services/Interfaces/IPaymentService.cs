using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPaymentService
    {
        void SavePayment(Payment payment);

        void SavePaymentWithBookingIds(Payment payment, List<int> bookingIds);
    }
}
