using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? UserId { get; set; }

    public int? CourtId { get; set; }

    public DateTime BookingDate { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public decimal TotalPrice { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public virtual ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();

    public virtual Court? Court { get; set; }

    public virtual User? User { get; set; }
}
