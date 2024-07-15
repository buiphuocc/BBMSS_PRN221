using BusinessObjects;

namespace BBMSRazorPages.Models
{
    public class ScheduleBookingModel
    {
        public string DaysOfWeek {  get; set; } 

        public Court Court { get; set; }

        public User User { get; set; }  

        public string BookingDates { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
