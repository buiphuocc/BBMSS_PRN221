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

        public List<Booking> GetBookingsByBookingDate(DateTime bookingDate)
        {
            return BookingDAO.GetBookingsByBookingDate(bookingDate);
        }

        public void UpdateBooking(Booking booking)
        {
            BookingDAO.UpdateBooking(booking);
        }

        public IList<Booking> GetBookingsByUserId(int userId)
        {
            return BookingDAO.GetBookingsByUserId(userId);
        }

        public IList<Booking> GetBookingsByCourtId(int? courtId)
        {
            return BookingDAO.GetBookingsByCourtId(courtId);
        }

        public IList<Booking> GetBookingsByDateAndStartTimeAndEndTime(DateTime date, TimeSpan startTime, TimeSpan endTime)
            => BookingDAO.GetBookingsByDateAndStartTimeAndEndTime(date, startTime, endTime);

        public void AddBookingWithServices(Booking booking)
            => BookingDAO.AddBookingWithServices(booking);

        public Booking? GetBookingsByBookingDateAndCourtIdAndStartTimeAndEndTimeAndPaymentMethod(DateTime bookingDate, int courtId, TimeSpan startTime, TimeSpan endTime, string paymentMethod)
            => BookingDAO.GetBookingsByBookingDateAndCourtIdAndStartTimeAndEndTimeAndPaymentMethod(bookingDate, courtId, startTime, endTime, paymentMethod);
    
        public void UpdateBookingStatusBasedOnRealTime(Booking booking)
        {
            if (booking != null && !booking.Status.Equals("Completed"))
            {
                booking.Status = "Completed";
                BookingDAO.UpdateBooking(booking);
            }
        }
    }
}
