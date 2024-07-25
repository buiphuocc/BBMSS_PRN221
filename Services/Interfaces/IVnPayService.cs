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
        string CreatePaymentUrl(int userId, ScheduleBookingsModel scheduleBookingModel, HttpContext context);

        string CreatePaymentUrlDailyBooking(BusinessObjects.Booking booking, HttpContext context);

        string CreatePaymentUrlForBooking(List<Booking> bookings, HttpContext context);

        Task<VnPayPaymentModel> PaymentExecute(IQueryCollection collections);

        VnPayPaymentModel BookingPaymentExecute(IQueryCollection collections);
    }
}
