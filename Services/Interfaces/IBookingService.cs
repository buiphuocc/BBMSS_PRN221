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
        IList<Booking> GetAllBookings();
        Booking GetBookingById(int id);
        void AddBooking(Booking booking);
        void UpdateBooking(Booking booking);
        void DeleteBooking(int id);
        List<Booking> GetBookingsByBookingDate(DateTime bookingDate);
        public IList<Booking> GetBookingsByUserId(int userId);

    }
}
 
