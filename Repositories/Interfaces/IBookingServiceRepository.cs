using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IBookingServiceRepository
    {
        List<BookingService> GetAllBookingServices();
        BookingService GetBookingServiceById(int id);
        List<BookingService> GetBookingServicesByBookingId(int? id);
        void AddBookingService(BookingService bookingService);
        void UpdateBookingService(BookingService bookingService);
        void DeleteBookingService(int id);
    }
}
