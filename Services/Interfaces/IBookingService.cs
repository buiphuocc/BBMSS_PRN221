﻿using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookingService
    {
        List<Booking> GetAllBookings();
        Booking GetBookingById(int id);
        void AddBooking(Booking booking);
        void UpdateBooking(Booking booking);
        void DeleteBooking(int id);
        List<Booking> GetBookingsByBookingDate(DateTime bookingDate);
        public IList<Booking> GetBookingsByUserId(int userId);
        IList<Booking> GetBookingsByCourtId(int? courtId);
        List<Booking> GetBookingsByDateAndStartTimeAndEndTime(DateTime date, TimeSpan startTime, TimeSpan endTime);
        void AddBookingWithServices(Booking booking);
        Booking? GetBookingsByBookingDateAndCourtIdAndStartTimeAndEndTimeAndPaymentMethod(DateTime bookingDate, int courtId, TimeSpan startTime, TimeSpan endTime, string paymentMethod);
        void UpdateBookingsStatusBasedOnCurrentTime();
    }
}
 
