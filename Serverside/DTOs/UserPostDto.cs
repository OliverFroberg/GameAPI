using System.ComponentModel.DataAnnotations;

namespace Serverside.DTOs;

public class UserPostDto {
	[Required]
	[EmailAddress(ErrorMessage = "Invalid email address")]
	public string Email { get; set; } = string.Empty;

	[Required]
	[MinLength(2)]
	public string Name { get; set; } = string.Empty;
}