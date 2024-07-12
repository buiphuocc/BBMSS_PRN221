using BusinessObjects;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public void SavePayment(Payment payment)
        {
            try
            {
                paymentRepository.SavePayment(payment);
            }
            catch
            {
                throw;
            }
        }

        public void SavePaymentWithBookingIds(Payment payment, List<int> bookingIds)
        {
            try
            {
                paymentRepository.SavePaymentWithBookingIds(payment, bookingIds);
            }
            catch
            {
                throw;
            }
        }
    }
}
