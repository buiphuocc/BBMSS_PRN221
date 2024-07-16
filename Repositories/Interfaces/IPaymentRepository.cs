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

        Payment GetPaymentByBookingId(int bookingId);
        void UpdatePayment(Payment payment);


        void SavePayment(Payment payment);

        void SavePaymentWithBookingIds(Payment payment, List<int> bookingIds);

        //Task<int> GetIdForNewPayment();
    }
}
