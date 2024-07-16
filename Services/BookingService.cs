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
        private readonly IBookingReppository bookingRepository;

        public BookingService(IBookingReppository bookingReppository)
        {
            this.bookingRepository = bookingReppository;
        }

        public void AddBooking(Booking booking)
        {
            bookingRepository.AddBooking(booking);
        }

        public void DeleteBooking(int id)
        {
            bookingRepository.DeleteBooking(id);
        }

        public List<Booking> GetAllBookings()
        {
            return bookingRepository.GetAllBookings();
        }

        public Booking GetBookingById(int id)
        {
            return bookingRepository.GetBookingById(id);
        }

        public List<Booking> GetBookingsByBookingDate(DateTime bookingDate)
        {
            return bookingRepository.GetBookingsByBookingDate(bookingDate);
        }

        public void UpdateBooking(Booking booking)
        {
            bookingRepository.UpdateBooking(booking);
        }
        public IList<Booking> GetBookingsByUserId(int userId)
        {
            return bookingRepository.GetBookingsByUserId(userId);
        }

        public IList<Booking> GetBookingsByCourtId(int? courtId)
        {
            return bookingRepository.GetBookingsByCourtId(courtId);
        }

        public List<Booking> GetBookingsByDateAndStartTimeAndEndTime(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            try
            {
                return bookingRepository.GetBookingsByDateAndStartTimeAndEndTime(date, startTime, endTime).ToList();
            }catch
            {
                throw;
            }
        }

        public void UpdateBookingsStatusBasedOnCurrentTime()
        {
            var bookings = bookingRepository.GetAllBookings();
            foreach (var booking in bookings)
            {
                if ((booking.Status == null || !booking.Status.Equals("Completed"))
                    && (
                        booking.BookingDate.Date < DateTime.Now.Date 
                        || (booking.BookingDate.Date == DateTime.Now.Date && booking.EndTime <= DateTime.Now.TimeOfDay)
                        )
                    )
                {
                    bookingRepository.UpdateBookingStatusBasedOnRealTime(booking);
                    Console.WriteLine($"Booking with ID {booking.BookingId} status has changed to {booking.Status}");
                }
            }
        }

        public void AddBookingWithServices(Booking booking)
        {
            try
            {
                bookingRepository.AddBookingWithServices(booking);
            }
            catch
            {
                throw;
            }
        }

        public Booking? GetBookingsByBookingDateAndCourtIdAndStartTimeAndEndTimeAndPaymentMethod(DateTime bookingDate, int courtId, TimeSpan startTime, TimeSpan endTime, string paymentMethod)
        {
            try
            {
                return bookingRepository.GetBookingsByBookingDateAndCourtIdAndStartTimeAndEndTimeAndPaymentMethod(bookingDate, courtId, startTime, endTime, paymentMethod);
            }
            catch
            {
                throw;
            }
        }
    }
}
