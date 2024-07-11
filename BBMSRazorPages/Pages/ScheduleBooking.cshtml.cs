using BBMSRazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BBMSRazorPages.Pages
{
    public class ScheduleBookingModel : PageModel
    {
        [BindProperty]
        public int Month { get; set; }

        [BindProperty]
        public int Year { get; set; }

        public TimeOnly StartTime { get; set; } = new TimeOnly(5, 0);

        public TimeOnly EndTime { get; set; } = new TimeOnly(23, 0);

        public List<string> TimesList { get; set; } = new List<string>();

        [BindProperty]
        public string FromTime { get; set; }

        [BindProperty]
        public string ToTime { get; set; }

        [BindProperty]
        public List<string> DaysOfWeek { get; set; } = new List<string>
        {
            DayOfWeek.Monday.ToString(),
            DayOfWeek.Tuesday.ToString(),
            DayOfWeek.Wednesday.ToString(),
            DayOfWeek.Thursday.ToString(),
            DayOfWeek.Friday.ToString(),
            DayOfWeek.Saturday.ToString(),
            DayOfWeek.Sunday.ToString(),
        };

        [BindProperty]
        public List<string> AvailableDaysOfWeek { get; set; } = new List<string>();

        [BindProperty]
        public List<string> SelectedDaysOfWeek { get; set; } = new List<string>();

        [BindProperty]
        public List<int> SelectedServices { get; set; } = new List<int>();

        [BindProperty]
        public List<int> ServicesAmount { get; set; } = new List<int>();

        public void OnGet()
        {
            var currentDate = DateTime.Now;
            var currentTime = TimeOnly.FromDateTime(currentDate);
            Month = currentDate.Month;
            Year = currentDate.Year;
            var timesList = new List<string>();
            for (int i = StartTime.Hour; i <= EndTime.Hour; i++)
            {
                var time = new TimeOnly(i, 0);
                timesList.Add(time.ToString("HH:mm"));
                if (i == EndTime.Hour)
                    continue;
                var halfTime = new TimeOnly(i, 30);
                timesList.Add(halfTime.ToString("HH:mm"));
            }
            TimesList = timesList;

        }

        public IActionResult OnGetUpdateDaysOfWeek(int month, int year)
        {
            // Update the DaysOfWeek property
            AvailableDaysOfWeek.Add("Thursday");
            AvailableDaysOfWeek.Add("Tuesday");
            AvailableDaysOfWeek.Add("Saturday");

            int selectedMonth = month;
            int selectedYear = year;    

            // Return the updated partial view
            return Partial("_DaysOfWeekPartial", this);
        }

        public IActionResult OnGetResetDaysOfWeek()
        {
            // Update the DaysOfWeek property
            AvailableDaysOfWeek = new List<string>();

            // Return the updated partial view
            return Partial("_DaysOfWeekPartial", this);
        }

        public IActionResult OnPost()
        {
            var selectedDaysOfWeek = SelectedDaysOfWeek;
            var selectedServices = SelectedServices;
            var servicesAmount = ServicesAmount;
            var month = Month;
            var year = Year;
            var fromTime = FromTime;
            var toTime = ToTime;
            return RedirectToPage();
        }

        public IActionResult OnGetSetTimes(string fromTime, string toTime)
        {
            FromTime = fromTime;
            ToTime = toTime;
            return new JsonResult(new { success = true });
        }

        public IActionResult OnGetServiceOptions()
        {
            var options = new List<ServiceOption>
            {
                new ServiceOption { Id = 1, Name = "Service 1" },
                new ServiceOption { Id = 2, Name = "Service 2" },
                new ServiceOption { Id = 3, Name = "Service 3" }
                // Add more options as needed
            };

            return new JsonResult(options);
        }
    }
}
