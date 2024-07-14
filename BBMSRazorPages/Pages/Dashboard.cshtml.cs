using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.Linq;

namespace BBMSRazorPages.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly IBookingServiceService bookingServiceService;
        private readonly ICourtService _courtService;
        private readonly IUserService _userService;

        public DashboardModel(IBookingService bookingService, IBookingServiceService bookingServiceService, ICourtService courtService, IUserService userService)
        {
            _bookingService = bookingService;
            this.bookingServiceService = bookingServiceService;
            _courtService = courtService;
            _userService = userService;
        }

        public IList<BusinessObjects.Booking> Bookings { get; set; } = new List<BusinessObjects.Booking>();
        public IList<BusinessObjects.Booking> AllBookings { get; set; } = new List<BusinessObjects.Booking>();

        public IList<BusinessObjects.BookingService> BookingServices { get; set; } = new List<BusinessObjects.BookingService>();
        public IList<BusinessObjects.Court> Courts { get; set; } = new List<BusinessObjects.Court>();
        public IList<User> Users { get; set; } = new List<User>();
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }


        public IActionResult OnGet(int? pageNumber)
        {
            const int pageSize = 10;
            PageNumber = pageNumber ?? 1;

            var bookings = _bookingService.GetAllBookings();
            AllBookings = bookings;
            TotalPages = (int)Math.Ceiling(bookings.Count() / (double)pageSize);

            Bookings = bookings
                        .Skip((PageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            Courts = _courtService.GetAllCourts();
            BookingServices = bookingServiceService.GetAllBookingServices();
            Users = _userService.GetAllUsers();

            return Page();
        }

        public List<BookingService> GetBookingServicesByBookingId(int? id)
        {
            return bookingServiceService.GetBookingServicesByBookingId(id);
        }

        public User GetUserByBookingId(int? id)
        {
            return _userService.GetUserById((int)_bookingService.GetBookingById((int)id).UserId);
        }
    }
}
