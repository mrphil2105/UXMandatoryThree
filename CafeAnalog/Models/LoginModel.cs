using System.ComponentModel.DataAnnotations;

namespace CafeAnalog.Models;

public class LoginModel
{
    [Required]
    [Display(Prompt = "Email")]
    public string? Email { get; init; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Prompt = "Password")]
    public string? Password { get; init; }

    [Display(Name = "Keep me signed in")]
    public bool IsPersistent { get; init; }

    public string? ReturnUrl { get; init; }
}
