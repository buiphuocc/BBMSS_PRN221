﻿using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IBookingReppository
    {
        List<Booking> GetAllBookings();
        Booking GetBookingById(int id);
        void AddBooking(Booking booking);
        void UpdateBooking(Booking booking);
        void DeleteBooking(int id);
        List<Booking> GetBookingsByBookingDate(DateTime bookingDate);
        IList<Booking> GetBookingsByUserId(int userId);
        IList<Booking> GetBookingsByCourtId(int? courtId);
        IList<Booking> GetBookingsByDateAndStartTimeAndEndTime(DateTime date, TimeSpan startTime, TimeSpan endTime);
        void AddBookingWithServices(Booking booking);
        Booking? GetBookingsByBookingDateAndCourtIdAndStartTimeAndEndTimeAndPaymentMethod(DateTime bookingDate, int courtId, TimeSpan startTime, TimeSpan endTime, string paymentMethod);
        void UpdateBookingStatusBasedOnRealTime(Booking booking);
    }
}
