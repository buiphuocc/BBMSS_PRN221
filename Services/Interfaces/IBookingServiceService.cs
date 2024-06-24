using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookingServiceService
    {
        List<BusinessObjects.BookingService> GetAllBookingServices();
        BusinessObjects.BookingService GetBookingServiceById(int id);
        List<BusinessObjects.BookingService> GetBookingServicesByBookingId(int? id);
        void AddBookingService(BusinessObjects.BookingService bookingService);
        void UpdateBookingService(BusinessObjects.BookingService bookingService);
        void DeleteBookingService(int id);
    }
}
