using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class BookingDAO
    {
        //private readonly BadmintonBookingSystemContext _context;

        //public BookingDAO(BadmintonBookingSystemContext context)
        //{
        //    _context = context;
        //}

        public static List<Booking> GetAllBookings()
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.Bookings
                .Include(b=>b.Payment)
                .Include(b => b.User)
                .Include(b => b.Court)
                .ToList();
        }

        public static Booking GetBookingById(int id)
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Court)
                .Include(b => b.Payment)
                .FirstOrDefault(b => b.BookingId == id);
        }

        public static List<Booking> GetBookingsByBookingDate(DateTime bookingDate)
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.Bookings
                .Include(b => b.Payment)
                .Include(b => b.User)
                .Include(b => b.Court)
                .Where(b => b.BookingDate == bookingDate).ToList();
        }

        public static void AddBooking(Booking booking)
        {
            using var _context = new BadmintonBookingSystemContext();
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public static void AddBookingWithServices(Booking booking)
        {
            try
            {
                using var _context = new BadmintonBookingSystemContext();
                var bookingServices = booking.BookingServices;
                booking.BookingServices = new List<BookingService>();
                _context.Bookings.Add(booking);
                if(!bookingServices.IsNullOrEmpty())
                {
                    foreach(var bookingService in bookingServices)
                    {
                        _context.BookingServices.Add(bookingService);
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateBooking(Booking booking)
        {
            using var _context = new BadmintonBookingSystemContext();
            _context.Entry(booking).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public static void DeleteBooking(int id)
        {
            using var _context = new BadmintonBookingSystemContext();
            var booking = _context.Bookings.Find(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                _context.SaveChanges();
            }
        }

        public static IList<Booking> GetBookingsByUserId(int userId)
        {
            using var _context = new BadmintonBookingSystemContext();

            return _context.Bookings
                .Include(b => b.Court)
                .Include(b => b.Payment)
                .Include(b => b.BookingServices)
                .ThenInclude(bs => bs.Service)
                .Where(b => b.UserId == userId)
                .ToList();
        }

        public static IList<Booking> GetBookingsByCourtId(int? courtId)
        {
            using var _context = new BadmintonBookingSystemContext();

            return _context.Bookings
                .Include(b => b.Court)
                .Include(b => b.BookingServices)
                .ThenInclude(bs => bs.Service)
                .Where(b => b.CourtId == courtId)
                .ToList();
        }

        public static IList<Booking> GetBookingsByDateAndStartTimeAndEndTime(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            var bookings = new List<Booking>();
            try
            {
                using var _context = new BadmintonBookingSystemContext();
                bookings = _context.Bookings
                    .Include (b => b.Court)
                    .Where(b 
                        => (b.BookingDate == date) 
                            && (b.StartTime == startTime 
                            || b.EndTime == endTime 
                            || (b.StartTime >= startTime && b.StartTime < endTime) 
                            || (b.EndTime > startTime && b.EndTime <= endTime) 
                            || (b.StartTime < startTime && b.EndTime > endTime)) 
                            && (b.Status.Equals("Pending") || b.Status.Equals("Comfirm")))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return bookings;
        }

        public static Booking? GetBookingsByBookingDateAndCourtIdAndStartTimeAndEndTimeAndPaymentMethod(DateTime bookingDate, int courtId, TimeSpan startTime, TimeSpan endTime, string paymentMethod)
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.Bookings
                .Include(b => b.Court)
                .FirstOrDefault(b 
                => (b.BookingDate == bookingDate) 
                    && (b.CourtId == courtId) 
                    && (b.StartTime == startTime) 
                    && (b.EndTime == endTime) 
                    && (b.PaymentMethod.Equals(paymentMethod))
                    && (b.Status.Equals("Pending")));
        }
    }
}
