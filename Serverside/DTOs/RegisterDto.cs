using System.ComponentModel.DataAnnotations;

namespace Serverside.DTOs;

public class RegisterDto {
	[Required, EmailAddress]
	public string Email { get; set; } = string.Empty;
	[Required, MinLength(2)]
	public string Name { get; set; } = string.Empty;
	[Required, MinLength(6, ErrorMessage = "Password skal være mindst 6 tegn")]
	public string Password { get; set; } = string.Empty;
}