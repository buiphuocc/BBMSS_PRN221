using BusinessObjects;
using DataAccessLayer;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        //public Task<int> GetIdForNewPayment()
        //    => PaymentDAO.GetIdForNewPayment();
        public void UpdatePayment(Payment payment)=> PaymentDAO.UpdatePayment(payment);
        public Payment GetPaymentByBookingId(int bookingId)=> PaymentDAO.GetPaymentByBookingId(bookingId);

        public void SavePayment(Payment payment)
            => PaymentDAO.SavePayment(payment);

        public void SavePaymentWithBookingIds(Payment payment, List<int> bookingIds)
            => PaymentDAO.SavePaymentWithBookingIds(payment, bookingIds);
    }
}
