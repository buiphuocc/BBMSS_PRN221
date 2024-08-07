﻿using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Payment
{
    public string Id { get; set; } = null!;

    public DateTime Date { get; set; }

    public long Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? Description { get; set; }

    public bool Success { get; set; }

    public string? TransactionId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
