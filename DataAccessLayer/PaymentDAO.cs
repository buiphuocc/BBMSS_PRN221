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
                //_context.Pay
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
