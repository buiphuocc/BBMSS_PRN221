using BusinessObjects;
using DataAccessLayer;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BookingServiceRepository : IBookingServiceRepository
    {
        private readonly BookingServiceDAO bookingServiceDAO;

        public BookingServiceRepository(BookingServiceDAO bookingServiceDAO)
        {
            this.bookingServiceDAO = bookingServiceDAO;
        }

        public void AddBookingService(BookingService bookingService)
        {
            bookingServiceDAO.AddBookingService(bookingService);
        }

        public void DeleteBookingService(int id)
        {
            bookingServiceDAO.DeleteBookingService(id);
        }

        public List<BookingService> GetAllBookingServices()
        {
            return bookingServiceDAO.GetAllBookingServices();
        }

        public BookingService GetBookingServiceById(int id)
        {
            return bookingServiceDAO.GetBookingServiceById(id);
        }

        public void UpdateBookingService(BookingService bookingService)
        {
            bookingServiceDAO.UpdateBookingService(bookingService);
        }
    }
}
