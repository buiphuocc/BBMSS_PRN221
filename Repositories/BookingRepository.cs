using BusinessObjects;
using DataAccessLayer;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BookingRepository : IBookingReppository
    {
        private readonly BookingDAO bookingDAO;

        public BookingRepository(BookingDAO bookingDAO)
        {
            this.bookingDAO = bookingDAO;
        }

        public void AddBooking(Booking booking)
        {
            bookingDAO.AddBooking(booking);
        }

        public void DeleteBooking(int id)
        {
            bookingDAO.DeleteBooking(id);
        }

        public List<Booking> GetAllBookings()
        {
            return bookingDAO.GetAllBookings();
        }

        public Booking GetBookingById(int id)
        {
            return bookingDAO.GetBookingById(id);
        }

        public void UpdateBooking(Booking booking)
        {
            bookingDAO.UpdateBooking(booking);
        }
    }
}
