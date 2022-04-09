using System.ComponentModel.DataAnnotations;

namespace CafeAnalog.Models;

public class RegisterModel
{
    [Required]
    [MaxLength(100)]
    [Display(Name = "First Name")]
    public string? FirstName { get; init; }

    [Required]
    [MaxLength(100)]
    [Display(Name = "Last Name")]
    public string? LastName { get; init; }

    [Required]
    [EmailAddress]
    public string? Email { get; init; }

    [Phone]
    [Display(Name = "Mobile Number")]
    public string? MobileNumber { get; init; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; init; }

    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    [Display(Name = "Confirm Password")]
    public string? PasswordConfirmation { get; init; }
}
