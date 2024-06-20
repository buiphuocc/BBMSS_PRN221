using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class BookingServiceDAO
    {

        private readonly BadmintonBookingSystemContext _context;

        public BookingServiceDAO(BadmintonBookingSystemContext context)
        {
            _context = context;
        }

        public List<BookingService> GetAllBookingServices()
        {
            return _context.BookingServices
                .Include(bs => bs.Booking)
                .Include(bs => bs.Service)
                .ToList();
        }

        public BookingService GetBookingServiceById(int id)
        {
            return _context.BookingServices
                .Include(bs => bs.Booking)
                .Include(bs => bs.Service)
                .FirstOrDefault(bs => bs.BookingServiceId == id);
        }

        public void AddBookingService(BookingService bookingService)
        {
            _context.BookingServices.Add(bookingService);
            _context.SaveChanges();
        }

        public void UpdateBookingService(BookingService bookingService)
        {
            _context.Entry(bookingService).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteBookingService(int id)
        {
            var bookingService = _context.BookingServices.Find(id);
            if (bookingService != null)
            {
                _context.BookingServices.Remove(bookingService);
                _context.SaveChanges();
            }
        }
    }
}
