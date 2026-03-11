using System.ComponentModel.DataAnnotations;

namespace Serverside.DTOs;

public class PostMazeLevelRequestDto {
	[Required]
	public int Level { get; set; } = 1;
}