﻿using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Repositories.Interfaces;
using Services.Interfaces;
using Services.Libraries;
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
        private readonly IConfiguration _configuration;

        private readonly IPaymentRepository paymentRepository;

        public VnPayService(IConfiguration configuration, IPaymentRepository paymentRepository)
        {
            _configuration = configuration;
            this.paymentRepository = paymentRepository;
        }

        public VnPayPaymentModel BookingPaymentExecute(IQueryCollection collections)
        {
            try
            {
                var pay = new VnPayLibrary();
                var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);
                var payment = new Payment
                {
                    Amount = response.Amount,
                    TransactionId = response.TransactionId,
                    Date = response.PayDate,
                    Success = response.Success,
                    PaymentMethod = "VnPay",
                    Description = response.Description
                };
                var bookingIdsString = response.Description.Split(':')[1];
                var bookingIds = bookingIdsString.Split(',');
                var bookingIdsInt = new List<int>();
                for(int i = 0; i < bookingIds.Length; i++)
                {
                    bookingIdsInt.Add(int.Parse(bookingIds[i]));
                }
                paymentRepository.SavePaymentWithBookingIds(payment, bookingIdsInt);
                return response;
            }
            catch
            {
                throw;
            }
        }

        public string CreatePaymentUrl(Payment paymentModel, HttpContext context, string currentPath)
        {
            throw new NotImplementedException();
        }

        public string CreatePaymentUrlForBooking(List<Booking> bookings, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];
            if (bookings[0].UserId != null)
            {
                urlCallBack += "&userId=" + bookings[0].UserId;
            }
            var totalPrice = (decimal)0;
            var bookingIdsString = "";
            for(int i = 0; i < bookings.Count; i++)
            {
                totalPrice += bookings[i].TotalPrice;
                if(i == bookings.Count - 1)
                {
                    bookingIdsString += bookings[i].BookingId;
                    continue;
                }
                bookingIdsString += bookings[i].BookingId + ",";
            }
            
            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)totalPrice * 100000).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"Payment for schedule booking:" + bookingIdsString);
            pay.AddRequestData("vnp_OrderType", bookings[0].PaymentMethod);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public VnPayPaymentModel PaymentExecute(IQueryCollection collections)
        {
            throw new NotImplementedException();
        }
    }
}
