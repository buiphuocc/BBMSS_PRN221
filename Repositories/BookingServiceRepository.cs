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
        //private readonly BookingServiceDAO bookingServiceDAO;

        //public BookingServiceRepository(BookingServiceDAO bookingServiceDAO)
        //{
        //    this.bookingServiceDAO = bookingServiceDAO;
        //}

        public void AddBookingService(BookingService bookingService)
        {
            BookingServiceDAO.AddBookingService(bookingService);
        }

        public void DeleteBookingService(int id)
        {
            BookingServiceDAO.DeleteBookingService(id);
        }

        public List<BookingService> GetAllBookingServices()
        {
            return BookingServiceDAO.GetAllBookingServices();
        }

        public BookingService GetBookingServiceById(int id)
        {
            return BookingServiceDAO.GetBookingServiceById(id);
        }

        public List<BookingService> GetBookingServicesByBookingId(int? id)
        {
            return BookingServiceDAO.GetBookingServicesByBookingId(id);
        }

        public List<BookingService> GetBookingServicesByServiceId(int? id)
        {
            return BookingServiceDAO.GetBookingServicesByServiceId(id);
        }

        public void UpdateBookingService(BookingService bookingService)
        {
            BookingServiceDAO.UpdateBookingService(bookingService);
        }
    }
}
