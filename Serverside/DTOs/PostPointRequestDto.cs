using System.ComponentModel.DataAnnotations;

namespace Serverside.DTOs;

public class PostPointRequestDto {
	[Required(ErrorMessage = "Source is required.")]
	public string? Source { get; set; }

	[Range(1, 100, ErrorMessage = "Amount must be between 1 and 100.")]
	public int Amount { get; set; } = 1;
}