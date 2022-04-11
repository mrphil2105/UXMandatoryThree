using System.ComponentModel.DataAnnotations;

namespace CafeAnalog.Models;

public class RegisterModel
{
    [Required]
    [MaxLength(25)]
    [Display(Prompt = "First Name")]
    public string? FirstName { get; init; }

    [Required]
    [MaxLength(25)]
    [Display(Prompt = "Last Name")]
    public string? LastName { get; init; }

    [Required]
    [EmailAddress]
    [Display(Prompt = "Email")]
    public string? Email { get; init; }

    [Phone]
    [Display(Prompt = "Mobile Number")]
    public string? MobileNumber { get; init; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Prompt = "Password")]
    public string? Password { get; init; }

    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    [Display(Prompt = "Confirm Password")]
    public string? PasswordConfirmation { get; init; }
}
