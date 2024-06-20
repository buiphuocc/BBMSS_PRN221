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
        private readonly BadmintonBookingSystemContext _context;

        public BookingDAO(BadmintonBookingSystemContext context)
        {
            _context = context;
        }

        public List<Booking> GetAllBookings()
        {
            return _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Court)
                .ToList();
        }

        public Booking GetBookingById(int id)
        {
            return _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Court)
                .FirstOrDefault(b => b.BookingId == id);
        }

        public void AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public void UpdateBooking(Booking booking)
        {
            _context.Entry(booking).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteBooking(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                _context.SaveChanges();
            }
        }
    }
}
