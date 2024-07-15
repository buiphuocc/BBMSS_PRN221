using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects;

public partial class Court
{
    public int CourtId { get; set; }

    [Required]
    public string CourtName { get; set; } = null!;

    [Required]
    public decimal PricePerHour { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
