using BusinessObjects;
using DataAccessLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class BookingDAO
    {
        public static List<Booking> GetBookings()
        {
            var bookings = new List<Booking>();
            try
            {
                using var dbContext = new BBMSDbContext();
                bookings = dbContext.Bookings.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return bookings;
        }

        public static Booking? GetBookingById(int id)
        {
            using var dbContext = new BBMSDbContext();
            return dbContext.Bookings.FirstOrDefault(b => b.Id == id);
        }

        public static bool CreateBooking(Booking booking)
        {
            try
            {
                using var dbContext = new BBMSDbContext();
                dbContext.Add(booking);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool UpdateBooking(Booking booking)
        {
            try
            {
                using var dbContext = new BBMSDbContext();
                dbContext.Entry(booking).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
