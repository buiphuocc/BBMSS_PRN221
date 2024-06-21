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
        //private readonly BookingDAO bookingDAO;

        //public BookingRepository(BookingDAO bookingDAO)
        //{
        //    this.bookingDAO = bookingDAO;
        //}

        public void AddBooking(Booking booking)
        {
            BookingDAO.AddBooking(booking);
        }

        public void DeleteBooking(int id)
        {
            BookingDAO.DeleteBooking(id);
        }

        public List<Booking> GetAllBookings()
        {
            return BookingDAO.GetAllBookings();
        }

        public Booking GetBookingById(int id)
        {
            return BookingDAO.GetBookingById(id);
        }

        public void UpdateBooking(Booking booking)
        {
            BookingDAO.UpdateBooking(booking);
        }
    }
}
