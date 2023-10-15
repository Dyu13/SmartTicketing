using SmartTicketing.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartTicketing.Domain;

public class User
{
    public int UserId { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public EUserRole Role { get; set; }
}