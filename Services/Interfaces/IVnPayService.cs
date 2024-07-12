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

        string CreatePaymentUrlForBooking(Booking bookingModel, HttpContext context, string currentPath);

        VnPayPaymentModel PaymentExecute(IQueryCollection collections);

        Task<VnPayPaymentModel> BookingPaymentExecute(int id, IQueryCollection collections);
    }
}
