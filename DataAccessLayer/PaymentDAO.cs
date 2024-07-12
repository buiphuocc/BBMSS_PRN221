using BusinessObjects;
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
    }
}
