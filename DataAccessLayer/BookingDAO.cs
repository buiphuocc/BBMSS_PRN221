using BusinessObjects;
using Microsoft.EntityFrameworkCore;
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
                .FirstOrDefault(b => b.BookingId == id);
        }

        public static void AddBooking(Booking booking)
        {
            using var _context = new BadmintonBookingSystemContext();
            _context.Bookings.Add(booking);
            _context.SaveChanges();
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
    }
}
