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
    public class PaymentDAO
    {
        public static Payment GetPaymentByBookingId(int bookingId)
        {
            using var _context = new BadmintonBookingSystemContext();
            // Since Payment has a one-to-many relationship with Booking,
            // we need to find the Payment associated with the given BookingId.
            return _context.Payments
                .Include(p => p.Bookings) // Assuming Payment has a collection of Bookings
                .FirstOrDefault(p => p.Bookings.Any(b => b.BookingId == bookingId));
        }

        public static void UpdatePayment(Payment payment)
        {
            using var _context = new BadmintonBookingSystemContext();
            _context.Payments.Update(payment);
            _context.SaveChanges();
        }
        public static void SavePayment(Payment payment)
        {
            try
            {
                using var _context = new BadmintonBookingSystemContext();
                var bookings = payment.Bookings;
                payment.Bookings = new List<Booking>();
                if (!bookings.IsNullOrEmpty())
                {
                    foreach(var booking in bookings)
                    {
                        _context.Bookings.Add(booking);
                    }
                }
                _context.Payments.Add(payment);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void SavePaymentWithBookingIds(Payment payment, List<int> bookingIds)
        {
            try
            {
                using var _context = new BadmintonBookingSystemContext();
                var bookings = new List<Booking>();
                foreach(var bookingId in bookingIds)
                {
                    var booking = _context.Bookings.Find(bookingId);
                    if (booking == null)
                        throw new Exception($"Booking with id {bookingId} not found.");
                    bookings.Add(booking);
                }
                if (!bookings.IsNullOrEmpty())
                {
                    payment.Bookings = bookings;
                }
                _context.Payments.Add(payment);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public static async Task<int> GetIdForNewPayment()
        //{
        //    try
        //    {
        //        using var _context = new BadmintonBookingSystemContext();
        //        if (!await _context.Payments.AnyAsync())
        //        {
        //            return 1;
        //        }
        //        var payment = await _context.Payments.OrderByDescending(x => x.Id).FirstAsync();
        //        return payment.Id + 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
