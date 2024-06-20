using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IBookingReppository
    {
        List<Booking> GetBookings();

        Booking? GetBookingById(int id);

        bool CreateBooking(Booking booking);

        bool UpdateBooking(Booking booking);
    }
}
