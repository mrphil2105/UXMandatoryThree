using System.ComponentModel.DataAnnotations;

namespace CafeAnalog.Models;

public class LoginModel
{
    [Required]
    public string? Email { get; init; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; init; }

    [Display(Name = "Keep me signed in")]
    public bool IsPersistent { get; init; }

    public string? ReturnUrl { get; init; }
}
