using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class ScheduleBookingsModel
    {
        public string DaysOfWeek { get; set; }

        public Court Court { get; set; }

        public User User { get; set; }

        public string BookingDates { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public decimal TotalPrice { get; set; }

        public List<BusinessObjects.BookingService> BookingServices { get; set; }
    }
}
