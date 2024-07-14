using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        void SavePayment(Payment payment);

        void SavePaymentWithBookingIds(Payment payment, List<int> bookingIds);

        //Task<int> GetIdForNewPayment();
    }
}
