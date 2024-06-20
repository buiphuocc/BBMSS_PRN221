using BusinessObjects;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookingServiceService : IBookingServiceService
    {
        private readonly IBookingServiceRepository bookingServiceRepository;

        public BookingServiceService(IBookingServiceRepository bookingServiceRepository)
        {
            this.bookingServiceRepository = bookingServiceRepository;
        }

        public void AddBookingService(BusinessObjects.BookingService bookingService)
        {
            bookingServiceRepository.AddBookingService(bookingService);
        }

        public void DeleteBookingService(int id)
        {
            bookingServiceRepository.DeleteBookingService(id);
        }

        public List<BusinessObjects.BookingService> GetAllBookingServices()
        {
            return bookingServiceRepository.GetAllBookingServices();
        }

        public BusinessObjects.BookingService GetBookingServiceById(int id)
        {
            return bookingServiceRepository.GetBookingServiceById(id);
        }

        public void UpdateBookingService(BusinessObjects.BookingService bookingService)
        {
            bookingServiceRepository.UpdateBookingService(bookingService);
        }
    }
}
