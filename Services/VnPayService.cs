using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class VnPayService : IVnPayService 
    {
        //private readonly IConfiguration _configuration;

        public Task<VnPayPaymentModel> BookingPaymentExecute(int id, IQueryCollection collections)
        {
            throw new NotImplementedException();
        }

        public string CreatePaymentUrl(Payment paymentModel, HttpContext context, string currentPath)
        {
            throw new NotImplementedException();
        }

        public string CreatePaymentUrlForBooking(Booking bookingModel, HttpContext context, string currentPath)
        {
            throw new NotImplementedException();
        }

        public VnPayPaymentModel PaymentExecute(IQueryCollection collections)
        {
            throw new NotImplementedException();
        }
    }
}
