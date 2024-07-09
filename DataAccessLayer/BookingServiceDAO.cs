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

        //private readonly BadmintonBookingSystemContext _context;

        //public BookingServiceDAO(BadmintonBookingSystemContext context)
        //{
        //    _context = context;
        //}

        public static List<BookingService> GetAllBookingServices()
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.BookingServices
                .Include(bs => bs.Booking)
                .Include(bs => bs.Service)
                .ToList();
        }

        public static BookingService GetBookingServiceById(int id)
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.BookingServices
                .Include(bs => bs.Booking)
                .Include(bs => bs.Service)
                .FirstOrDefault(bs => bs.BookingServiceId == id);
        }

        public static List<BookingService> GetBookingServicesByBookingId(int? id)
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.BookingServices
                .Include(bs => bs.Booking)
                .Include(bs => bs.Service)
                .Where(bs => bs.BookingId == id).ToList();
        }

        public static List<BookingService> GetBookingServicesByServiceId(int? id)
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.BookingServices
                .Include(bs => bs.Booking)
                .Include(bs => bs.Service)
                .Where(bs => bs.ServiceId == id).ToList();
        }

        public static void AddBookingService(BookingService bookingService)
        {
            using var _context = new BadmintonBookingSystemContext();
            _context.BookingServices.Add(bookingService);
            _context.SaveChanges();
        }

        public static void UpdateBookingService(BookingService bookingService)
        {
            using var _context = new BadmintonBookingSystemContext();
            _context.Entry(bookingService).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public static void DeleteBookingService(int id)
        {
            using var _context = new BadmintonBookingSystemContext();
            var bookingService = _context.BookingServices.Find(id);
            if (bookingService != null)
            {
                _context.BookingServices.Remove(bookingService);
                _context.SaveChanges();
            }
        }
    }
}
