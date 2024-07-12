using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(Payment paymentModel, HttpContext context, string currentPath);

        string CreatePaymentUrlForBooking(List<Booking> bookings, HttpContext context);

        VnPayPaymentModel PaymentExecute(IQueryCollection collections);

        VnPayPaymentModel BookingPaymentExecute(IQueryCollection collections);
    }
}
