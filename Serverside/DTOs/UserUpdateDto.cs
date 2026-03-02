using System.ComponentModel.DataAnnotations;

namespace Serverside.DTOs;

public class UserUpdateDto {
	[EmailAddress]
	public string? Email { get; set; }

	[MinLength(2)]
	public string? Name { get; set; }
}