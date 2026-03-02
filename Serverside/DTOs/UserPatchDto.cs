using System.ComponentModel.DataAnnotations;

namespace Serverside.DTOs;

public class UserPatchDto {
	[Required]
	[EmailAddress]
	public string? Email { get; set; }

	[Required]
	[MinLength(2)]
	public string? Name { get; set; }
}