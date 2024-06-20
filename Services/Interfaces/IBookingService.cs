using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookingService
    {
        List<Booking> GetBookings();

        Booking? GetBookingById(int id);

        bool CreateBooking(Booking booking);

        bool UpdateBooking(Booking booking);
    }
}
