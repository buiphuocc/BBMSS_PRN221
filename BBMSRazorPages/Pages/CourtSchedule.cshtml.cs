﻿using BusinessObjects;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services;
using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace BBMSRazorPages.Pages
{
    public class CourtScheduleModel : PageModel
    {
        private readonly IBookingService bookingService;
        private readonly ICourtService courtService;
        private readonly IServiceService serviceService;
        private readonly IBookingServiceService bookingServiceService;
        private readonly IUserService userService;
        private readonly IEmailSender emailSender;
        private readonly IMomoService momoService;
        private readonly IVnPayService vnPayService;
        private readonly IPaymentService paymentService;

        public List<BusinessObjects.Booking> Bookings { get; set; }
        public List<Court> Courts { get; set; }
        public string Message { get; set; }
        public int? UserId { get; set; }
        public List<Service> Services { get; set; }

        public class PricingViewModel
        {
            public List<Service> ?Services { get; set; }
            public List<Court> ?Courts { get; set; }
        }
        public PricingViewModel Pricing { get; set; }

        [BindProperty]
        public DateTime SelectedDate { get; set; }

        public string BookingDate { get; set; }

        [BindProperty]
        public string PaymentMethod { get; set; }

        [BindProperty]
        public int CourtId { get; set; }

        [BindProperty]
        public TimeSpan StartTime { get; set; }

        [BindProperty]
        public TimeSpan EndTime { get; set; }

        [BindProperty]
        public DateTime DateForm { get; set; }

        [BindProperty]
        public string? Note { get; set; }

        public CourtScheduleModel(IBookingService bookingService, ICourtService courtService, IServiceService serviceService, IBookingServiceService bookingServiceService, IUserService userService, IEmailSender emailSender, IMomoService momoService, IVnPayService vnPayService, IPaymentService paymentService)
        {
            this.bookingService = bookingService;
            this.courtService = courtService;
            this.serviceService = serviceService;
            this.bookingServiceService = bookingServiceService;
            this.userService = userService;
            this.emailSender = emailSender;
            this.momoService = momoService;
            this.vnPayService = vnPayService;
            this.paymentService = paymentService;
        }

        public void OnGet(DateTime bookingDate, string message)
        {
            Message = message;
            BookingDate = bookingDate.ToString("yyyy-MM-dd");
            SelectedDate = bookingDate;
            Bookings = bookingService.GetBookingsByBookingDate(SelectedDate);
            Courts = courtService.GetAllActiveCourts();
            UserId = HttpContext.Session.GetInt32("UserId");
            Services = serviceService.GetAllActiveServices();
            //dataa for menu
            var services = serviceService.GetAllServices();
            var courts = courtService.GetAllCourts();
            Pricing = new PricingViewModel
            {
                Services = services,
                Courts = courts
            };
        }

        public bool IsTimeSlotBooked(Court court, TimeSpan slot1, TimeSpan slot2)
        {
            List<BusinessObjects.Booking> bookings = bookingService.GetBookingsByBookingDate(SelectedDate);
            return Bookings.Any(b => slot1 >= b.StartTime && slot2 <= b.EndTime && b.Court.CourtId == court.CourtId && b.Status!="Cancelled");
        }
        public string GetBookingStatusClass(Court court, TimeSpan slot1, TimeSpan slot2)
        {
            var booking = Bookings.FirstOrDefault(b => slot1 >= b.StartTime && slot2 <= b.EndTime && b.Court.CourtId == court.CourtId && b.Status != "Cancelled");

            if (booking != null)
            {
                if (booking.Status == "Pending" || booking.Status==null)
                {
                    return "Pending";
                }
                else if (booking.Status == "Confirmed")
                {
                    return "Confirmed";
                }
                else if (booking.Status == "Completed")
                {
                    return "Completed";
                }
            }

            return ""; // Default class if no booking or status doesn't match
        }

        public IActionResult OnPostChangeStatus(int BookingId, string NewStatus)
        {
            var booking = bookingService.GetBookingById(BookingId);
            if (booking != null)
            {
                booking.Status = NewStatus;
                bookingService.UpdateBooking(booking);
                // Update the payment with the note
                var payment = paymentService.GetPaymentByBookingId(BookingId);
                if (payment != null)
                {
                    if (Note != null)
                    {
                    payment.Success = true;
                    }
                    payment.TransactionId = Note;
                    paymentService.UpdatePayment(payment);
                }
            }

            // Reload the bookings
            BookingDate= DateTime.Now.ToString("yyyy-MM-dd");
            SelectedDate = DateTime.Now;
            Bookings = bookingService.GetBookingsByBookingDate(SelectedDate);
            Courts = courtService.GetAllCourts();
            UserId = HttpContext.Session.GetInt32("UserId");
            Services = serviceService.GetAllServices();

            return RedirectToPage("/CourtSchedule", new { bookingDate = DateTime.UtcNow, message = "" });
        }
        public async Task<IActionResult> OnPostAsync()
        {
            UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId != null)
            {
                Bookings = bookingService.GetBookingsByBookingDate(DateForm);
                List<BusinessObjects.Booking> bookingsNotCancelled = Bookings.Where(b => b.Status != "Cancelled").ToList();
                
                TimeSpan workingStart = new TimeSpan(5, 0, 0);
                TimeSpan workingEnd = new TimeSpan(23, 0, 0);
                UserId = HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    bool isStartTimeInRange = workingStart <= StartTime && StartTime < workingEnd;
                    bool isEndTimeInRange = workingStart < EndTime && EndTime <= workingEnd;
                    if (!(isStartTimeInRange && isEndTimeInRange) || (StartTime > EndTime))
                    {
                        ModelState.AddModelError(string.Empty, "Not a valid time range");
                        return RedirectToPage("/CourtSchedule", new { bookingDate = DateForm, message = "Not a valid time range" });
                    }

                    TimeSpan currentTime = DateTime.Now.TimeOfDay;
                    if (DateForm < DateTime.Now && (StartTime < currentTime || EndTime < currentTime))
                    {
                        return RedirectToPage("/CourtSchedule", new { bookingDate = DateForm, message = "Cannot book in the past" });
                    }
                    // Additional validation logic for 30-minute intervals
                    if ((EndTime - StartTime).TotalMinutes < 30 ||
                        StartTime.Minutes % 30 != 0 ||
                        EndTime.Minutes % 30 != 0)
                    {
                        ModelState.AddModelError(string.Empty, "Time slots must be in 30-minute intervals.");
                        return RedirectToPage("/CourtSchedule", new { bookingDate = DateForm, message = "Time slots must be in 30-minute intervals." });
                    }


                    bool inTimeRange = bookingsNotCancelled.Any(b => ((b.StartTime <= StartTime && StartTime < b.EndTime) || (b.StartTime < EndTime && EndTime <= b.EndTime)) && b.Court.CourtId == CourtId);
                    Court court = courtService.GetCourtById(CourtId);
                    //bool isBooked = bookingsNotCancelled.Any(b => inTimeRange && b.Court.CourtId == CourtId);
                    if (inTimeRange)
                    {
                        ModelState.AddModelError(string.Empty, "This time range is booked");
                        return RedirectToPage("/CourtSchedule", new { bookingDate = DateForm, message = "This time range and court is booked" });
                    }
                    if(courtService.GetCourtById(CourtId).IsActive == false)
                    {
                        ModelState.AddModelError(string.Empty, "This court is currently inactive");
                        return RedirectToPage("/CourtSchedule", new { bookingDate = DateForm, message = "This court is currently inactive" });
                    }

                    List<int> bookings = new List<int>();
                    TimeSpan slotDuration = new TimeSpan(0, 30, 0); // 30 minutes
                    int totalSlots = (int)Math.Ceiling((EndTime - StartTime).TotalMinutes / slotDuration.TotalMinutes);
                    var newBooking = new BusinessObjects.Booking
                    {
                        CourtId = CourtId,
                        StartTime = StartTime,
                        EndTime = EndTime,
                        BookingDate = DateForm,
                        PaymentMethod = PaymentMethod,
                        UserId = UserId,
                        TotalPrice = totalSlots * (court.PricePerHour/2),
                        Status = "Pending", // Set the default status as Pending
                        BookingType = "Normal Booking"
                    };

                    if (PaymentMethod.Equals("Pay at Place"))
                    {
                        bookingService.AddBooking(newBooking);
                    }
                    
                    bookings.Add(newBooking.BookingId);

                    

                    // Handle selected services
                    var selectedServicesString = Request.Form["SelectedServices"];

                    if(!selectedServicesString.IsNullOrEmpty())
                    {
                        // Convert the selected services to a list of integers
                        var selectedServices = selectedServicesString
                                .ToString()
                                .Split(',')
                                .Select(int.Parse)
                                .ToList();

                        // Convert the service quantities to a dictionary<int, int>
                        var serviceQuantities = new Dictionary<int, int>();

                        foreach (var key in Request.Form.Keys)
                        {
                            if (key.StartsWith("ServiceQuantities["))
                            {
                                var serviceIdStr = key.Substring(18, key.Length - 19);
                                if (int.TryParse(serviceIdStr, out int serviceId))
                                {
                                    var quantityStr = Request.Form[key];
                                    if (int.TryParse(quantityStr, out int quantity))
                                    {
                                        serviceQuantities[serviceId] = quantity;
                                    }
                                }
                            }
                        }

                        // Log the service quantities for debugging
                        foreach (var entry in serviceQuantities)
                        {
                            System.Diagnostics.Debug.WriteLine($"Service ID: {entry.Key}, Quantity: {entry.Value}");
                        }

                        var bookingServices = new List<BusinessObjects.BookingService>();
                        if (!selectedServices.IsNullOrEmpty())
                        {
                            decimal totalServicePrice = 0;

                            foreach (var serviceId in selectedServices)
                            {
                                if (serviceQuantities.TryGetValue(serviceId, out int quantity))
                                {
                                    if(serviceService.GetServiceById(serviceId).IsActive == false)
                                    {
                                        ModelState.AddModelError(string.Empty, "This service is currently inactive");
                                        return RedirectToPage("/CourtSchedule", new { bookingDate = DateForm, message = "This service is currently inactive" });
                                    }
                                    var bookingService = new BusinessObjects.BookingService
                                    {
                                        BookingId = newBooking.BookingId,
                                        ServiceId = serviceId,
                                        Quantity = quantity
                                    };
                                    totalServicePrice += (quantity * serviceService.GetServiceById(serviceId).ServicePrice);
                                    if(PaymentMethod.Equals("Pay at Place"))
                                    {
                                        bookingServiceService.AddBookingService(bookingService);
                                    }
                                    else
                                    {
                                        var service = serviceService.GetServiceById(serviceId);
                                        bookingService.Service = service;
                                        bookingServices.Add(bookingService);
                                    }
                                    
                                }

                            }
                            newBooking.TotalPrice += totalServicePrice;
                            if (PaymentMethod.Equals("Pay at Place"))
                            {
                                bookingService.UpdateBooking(newBooking);
                            }
                            else
                            {
                                newBooking.BookingServices = bookingServices;
                            }
                            
                        }

                    }


                    //BusinessObjects.Booking forPayment = bookingService.GetBookingById(newBooking.BookingId);
                    //// Check newBooking
                    //if (forPayment == null)
                    //{
                    //    throw new NullReferenceException("newBooking is null");
                    //}

                    //// Check newBooking.User
                    //if (forPayment.User == null)
                    //{
                    //    throw new NullReferenceException("newBooking.User is null");
                    //}

                    //// Check newBooking.User.Username
                    //if (forPayment.User.Username == null)
                    //{
                    //    throw new NullReferenceException("newBooking.User.Username is null");
                    //}

                    //// Check newBooking.PaymentMethod
                    //if (forPayment.PaymentMethod == null)
                    //{
                    //    throw new NullReferenceException("newBooking.PaymentMethod is null");
                    //}

                    //Add payment for booking
                    //var payment = new Payment()
                    //{
                    //    Id = Guid.NewGuid().ToString(),
                    //    Date = DateTime.UtcNow,
                    //    Amount = (long)forPayment.TotalPrice,
                    //    PaymentMethod = forPayment.PaymentMethod,
                    //    Description = forPayment.User.Username + "Paid" + forPayment.TotalPrice,
                    //    Success = false,
                    //    TransactionId = null
                    //};
                    //paymentService.SavePaymentWithBookingIds(payment,bookings);

                    // Send email to user  

                    if (PaymentMethod.Equals("Pay at Place"))
                    {
                        User user = userService.GetUserById((int)UserId);

                        if (user != null)
                        {
                            var bookedCourt = courtService.GetCourtById((int)newBooking.CourtId);
                            var bookingServices = bookingServiceService.GetBookingServicesByBookingId(newBooking.BookingId);

                            string subject = "Badminton Court Booking Confirmation";
                            string message =
                                $"Dear {user.Email}, your badminton court booking has been confirmed!<br><br>" +
                                $"<strong>Booking Details:</strong><br>" +
                                $"Court name: {bookedCourt.CourtName}<br>" +
                                $"On date: {newBooking.BookingDate.ToShortDateString()}, from {newBooking.StartTime} to {newBooking.EndTime}<br>";

                            if (bookingServices != null && bookingServices.Any())
                            {
                                message += "<br><strong>Additional Services:</strong><br>";
                                foreach (var bookingService in bookingServices)
                                {
                                    Service s = serviceService.GetServiceById((int)bookingService.ServiceId);
                                    message += $"- {s.ServiceName}, quantity: {bookingService.Quantity}<br>";
                                }
                            }
                            else
                            {
                                message += "<br><strong>Additional Services:</strong> None.<br>";
                            }

                            message +=
                                $"<br>Total booking price: {newBooking.TotalPrice}<br>" +
                                $"Payment method: {newBooking.PaymentMethod}<br><br>" +
                                "In case the information is not correct, please contact us by replying to this email to make adjustments as soon as possible.";

                            await emailSender.SendEmailAsync(user.Email, subject, message);

                            Console.WriteLine("Sent email to " + user.Email);
                        }
                        return RedirectToPage("/CourtSchedule", new { bookingDate = DateForm, message = "Booked Successfully" });
                    }

                    // Send to booking information for payment
                    var bookingCourt = courtService.GetCourtById((int)newBooking.CourtId);
                    var bookingUser = userService.GetUserById((int)newBooking.UserId);
                    newBooking.Court = bookingCourt;
                    newBooking.User = bookingUser;
                    //newBooking.BookingDate.Hour(0);
                    var bookingJsonString = JsonSerializer.Serialize(newBooking);
                    var routeValue = new
                    {
                        bookingJsonString = bookingJsonString
                    };
                    TempData["BookingSuccessful"] = "Your booking has executed successfully.";
                    return RedirectToPage("/BookingSuccessful", routeValue);

                    // Create payment
                    //var orderInfo = new OrderInfoModel
                    //{
                    //    Amount = (double)newBooking.TotalPrice,
                    //    OrderInfo = newBooking.BookingId.ToString(),
                    //    UserId = user.UserId
                    //};

                    // Momo
                    //var response = await momoService.CreatePaymentAsync(orderInfo, null);
                    //return Redirect(response.PayUrl);

                    // VnPay
                    //var response = vnPayService.CreatePaymentUrlForBooking(new List<BusinessObjects.Booking> { newBooking }, HttpContext);
                    //return Redirect(response);

                }
                // If we got this far, something failed; redisplay form
                Bookings = bookingService.GetAllBookings();
                Courts = courtService.GetAllCourts();
                return Page();
            }
            return RedirectToPage("/Authentication/Login");
        }

        public bool isUserRoleAllowed()
        {
            string[] AllowedRoles = { "Admin", "Staff", "Manager" };
            string userRole = HttpContext.Session.GetString("UserRole");

            return userRole != null && AllowedRoles.Contains(userRole);
        }
    }
}
