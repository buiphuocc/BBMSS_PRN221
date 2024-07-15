using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects;

public partial class Service
{
    public int ServiceId { get; set; }

    [Required]
    public string ServiceName { get; set; } = null!;

    [Required]
    public decimal ServicePrice { get; set; }

    public virtual ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();
}
