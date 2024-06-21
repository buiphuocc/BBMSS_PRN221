using BusinessObjects;
using Repositories.Interfaces;
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
        private readonly IBookingReppository bookingReppository;

        public BookingService(IBookingReppository bookingReppository)
        {
            this.bookingReppository = bookingReppository;
        }

        public void AddBooking(Booking booking)
        {
            bookingReppository.AddBooking(booking);
        }

        public void DeleteBooking(int id)
        {
            bookingReppository.DeleteBooking(id);
        }

        public List<Booking> GetAllBookings()
        {
            return bookingReppository.GetAllBookings();
        }

        public Booking GetBookingById(int id)
        {
            return bookingReppository.GetBookingById(id);
        }

        public List<Booking> GetBookingsByBookingDate(DateTime bookingDate)
        {
            return bookingReppository.GetBookingsByBookingDate(bookingDate);
        }

        public void UpdateBooking(Booking booking)
        {
            bookingReppository.UpdateBooking(booking);
        }
        public IList<Booking> GetBookingsByUserId(int userId)
        {
            return bookingReppository.GetBookingsByUserId(userId);
        }
    }
}
