using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;
using Services.Libraries;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;

        private readonly IPaymentRepository paymentRepository;

        private readonly IBookingReppository bookingReppository;

        private readonly IServiceRepository serviceRepository;

        public VnPayService(IConfiguration configuration, IPaymentRepository paymentRepository, IBookingReppository bookingReppository, IServiceRepository serviceRepository)
        {
            _configuration = configuration;
            this.paymentRepository = paymentRepository;
            this.bookingReppository = bookingReppository;
            this.serviceRepository = serviceRepository;
        }

        public VnPayPaymentModel BookingPaymentExecute(IQueryCollection collections)
        {
            try
            {
                var pay = new VnPayLibrary();
                var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

                var payment = new Payment
                {
                    Id = response.OrderId,
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
                for (int i = 0; i < bookingIds.Length; i++)
                {
                    bookingIdsInt.Add(int.Parse(bookingIds[i]));
                }
                if (!response.Success)
                {
                    throw new Exception("Your payment executed unsuccessfully.:" + bookingIdsString);
                }
                paymentRepository.SavePaymentWithBookingIds(payment, bookingIdsInt);
                foreach (var bookingId in bookingIdsInt)
                {
                    var booking = bookingReppository.GetBookingById(bookingId);
                    booking.Status = "Confirmed";
                    bookingReppository.UpdateBooking(booking);
                }

                return response;
            }
            catch
            {
                throw;
            }
        }

        public string CreatePaymentUrl(int userId, ScheduleBookingsModel scheduleBookingModel, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var guid = Guid.NewGuid().ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"] + "&userId=" + userId + "&bookingDatesString=" + scheduleBookingModel.BookingDates
                + "&daysOfWeekString=" + scheduleBookingModel.DaysOfWeek + "&courtId=" + scheduleBookingModel.Court.CourtId + "&startTime=" + scheduleBookingModel.StartTime.ToString()
                + "&endTime=" + scheduleBookingModel.EndTime.ToString();

            ulong price = (ulong)scheduleBookingModel.TotalPrice * 100;

            

            var bookingServicesJsonString = JsonSerializer.Serialize(scheduleBookingModel.BookingServices);
            var bookingServicesString = "";
            for(int i = 0; i < scheduleBookingModel.BookingServices.Count; i++)
            {
                if(i == scheduleBookingModel.BookingServices.Count - 1)
                {
                    bookingServicesString += scheduleBookingModel.BookingServices[i].Service.ServiceId + ":" + scheduleBookingModel.BookingServices[i].Quantity;
                    continue;
                }
                bookingServicesString += scheduleBookingModel.BookingServices[i].Service.ServiceId + ":" + scheduleBookingModel.BookingServices[i].Quantity + "; ";
            }
            if(string.IsNullOrEmpty(bookingServicesString) || string.IsNullOrWhiteSpace(bookingServicesString))
            {
                bookingServicesString = "No service";
            }
            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", (price).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{bookingServicesString}");
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", guid);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public string CreatePaymentUrlDailyBooking(Booking booking, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var guid = Guid.NewGuid().ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"] + "&userId=" + booking.UserId + "&bookingDatesString=" + booking.BookingDate.ToString("MM/dd/yyyy")
                + "&courtId=" + booking.Court.CourtId + "&startTime=" + booking.StartTime.ToString()
                + "&endTime=" + booking.EndTime.ToString();

            ulong price = (ulong)booking.TotalPrice * 100;



            var bookingServicesJsonString = JsonSerializer.Serialize(booking.BookingServices);
            var bookingServicesList = booking.BookingServices.ToList();
            var bookingServicesString = "";
            for (int i = 0; i < bookingServicesList.Count; i++)
            {
                if (i == bookingServicesList.Count - 1)
                {
                    bookingServicesString += bookingServicesList[i].Service.ServiceId + ":" + bookingServicesList[i].Quantity;
                    continue;
                }
                bookingServicesString += bookingServicesList[i].Service.ServiceId + ":" + bookingServicesList[i].Quantity + "; ";
            }
            if (string.IsNullOrEmpty(bookingServicesString) || string.IsNullOrWhiteSpace(bookingServicesString))
            {
                bookingServicesString = "No service";
            }
            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", (price).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{bookingServicesString}");
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", guid);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public string CreatePaymentUrlForBooking(List<Booking> bookings, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var guid = Guid.NewGuid().ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];
            if (bookings[0].UserId != null)
            {
                urlCallBack += "&userId=" + bookings[0].UserId;
            }
            var totalPrice = (decimal)0;
            var bookingIdsString = "";
            for (int i = 0; i < bookings.Count; i++)
            {
                totalPrice += bookings[i].TotalPrice;
                if (i == bookings.Count - 1)
                {
                    bookingIdsString += bookings[i].BookingId;
                    continue;
                }
                bookingIdsString += bookings[i].BookingId + ",";
            }

            ulong price = (ulong)totalPrice * 100;
            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", (price).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"Payment for booking:" + bookingIdsString);
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", guid);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public VnPayPaymentModel PaymentExecute(IQueryCollection collections)
        {
            try
            {
                var pay = new VnPayLibrary();
                var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);
                collections.TryGetValue("userId", out var userIdString);
                var userId = int.Parse(userIdString);
                collections.TryGetValue("courtId", out var courtIdString);
                var courtId = int.Parse(courtIdString);
                collections.TryGetValue("bookingDatesString", out var bookingDatesString);
                var bookingDateStrings = bookingDatesString.ToString().Trim().Split(", ");
                var bookingDates = new List<DateTime>();
                for (int i = 0; i < bookingDateStrings.Length; i++)
                {
                    DateTime date = DateTime.ParseExact(bookingDateStrings[i], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    bookingDates.Add(date);
                }
                collections.TryGetValue("daysOfWeekString", out var daysOfWeekString);
                var bookingType = "Schedule Booking";
                if(string.IsNullOrEmpty(daysOfWeekString) || string.IsNullOrWhiteSpace(daysOfWeekString))
                {
                    bookingType = "Normal Booking";
                }
                collections.TryGetValue("startTime", out var startTimeString);
                var startTime = TimeSpan.Parse(startTimeString);
                collections.TryGetValue("endTime", out var endTimeString);
                var endTime = TimeSpan.Parse(endTimeString);



                var payment = new Payment
                {
                    Id = response.OrderId,
                    Amount = response.Amount,
                    TransactionId = response.TransactionId,
                    Date = response.PayDate,
                    Success = response.Success,
                    PaymentMethod = "VnPay",
                    Description = "Payment for " + bookingType + "."
                };
                
                if (!response.Success)
                {
                    throw new Exception("Your payment executed unsuccessfully.");
                }
                paymentRepository.SavePayment(payment);

                //List<BusinessObjects.BookingService> bookingServices = JsonSerializer.Deserialize<List<BusinessObjects.BookingService>>(response.Description);
                var bookingServicesString = response.Description;
                var bookingServices = new List<BusinessObjects.BookingService>();
                if (!bookingServicesString.Equals("No service"))
                {
                    var bookingServiceStrings = bookingServicesString.Trim().Split("; ");
                    
                    for (int i = 0; i < bookingServiceStrings.Length; i++)
                    {
                        var serviceQuantity = bookingServiceStrings[i].Trim().Split(':');
                        var serviceId = int.Parse(serviceQuantity[0]);
                        var service = serviceRepository.GetServiceById(serviceId);
                        bookingServices.Add(new BusinessObjects.BookingService
                        {
                            Service = service,
                            ServiceId = serviceId,
                            Quantity = int.Parse(serviceQuantity[1])
                        });
                    }
                }
                decimal pricePerBooking = 0;
                if (bookingType.Equals("Schedule Booking"))
                {
                    pricePerBooking = (decimal)((double)response.Amount / (double)bookingDates.Count);
                }
                else
                {
                    pricePerBooking = response.Amount;
                }
                

                foreach(var bookingDate in bookingDates)
                {
                    var booking = new BusinessObjects.Booking
                    {
                        UserId = userId,
                        CourtId = courtId,
                        BookingDate = bookingDate,
                        StartTime = startTime,
                        EndTime = endTime,
                        TotalPrice = (decimal) pricePerBooking,
                        PaymentMethod = "Online payment",
                        Status = "Confirmed",
                        PaymentId = payment.Id,
                        BookingType = bookingType
                    };
                    var newBookingServices = new List<BusinessObjects.BookingService>();
                    if (!bookingServices.IsNullOrEmpty())
                    {
                        
                        foreach (var bookingService in bookingServices)
                        {
                            var newBookingService = new BusinessObjects.BookingService
                            {
                                ServiceId = bookingService.ServiceId,
                                Quantity = bookingService.Quantity,
                                Booking = booking
                            };
                            newBookingServices.Add(newBookingService);
                            //var price = bookingService.Service.ServicePrice;
                            //booking.TotalPrice += (price * bookingService.Quantity);
                        }
                        
                    }
                    booking.BookingServices = newBookingServices;
                    bookingReppository.AddBookingWithServices(booking);
                }

                return response;
            }
            catch
            {
                throw;
            }
        }
    }
}
