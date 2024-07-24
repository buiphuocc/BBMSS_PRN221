using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Court
{
    public int CourtId { get; set; }

    public string CourtName { get; set; } = null!;

    public decimal PricePerHour { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
