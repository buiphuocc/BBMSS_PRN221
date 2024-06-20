using BusinessObjects;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingService bookingService;
        public BookingService(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }
        public bool CreateBooking(Booking booking)
        {
            return bookingService.CreateBooking(booking);
        }

        public Booking? GetBookingById(int id)
        {
            return bookingService.GetBookingById(id);
        }

        public List<Booking> GetBookings()
        {
            return bookingService.GetBookings();
        }

        public bool UpdateBooking(Booking booking)
        {
            return bookingService.UpdateBooking(booking);   
        }
    }
}
