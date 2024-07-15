using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects;

public partial class User
{
    public int UserId { get; set; }

    
    [StringLength(50, MinimumLength =2)]
    [Required(ErrorMessage = "Username is Required!")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Password is Required!")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Email is Required!")]
    [RegularExpression("^[^@\\s]+@[^@\\s]+\\.(com|net|org|gov|edu)$", ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [RegularExpression("^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Role is Required!")]
    public string Role { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
