using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace IceVault.IdentityProvider.Models;

public class ApplicationUser : IdentityUser
{
    [MaxLength(50), Required]
    public string FirstName { get; set; }

    [MaxLength(100), Required]
    public string LastName { get; set; }

    [MaxLength(50)]
    public string TimeZone { get; set; }

    [MaxLength(3)]
    public string Currency { get; set; }

    [MaxLength(5)]
    public string Language { get; set; }
}