using System.ComponentModel.DataAnnotations;

namespace Serverside.DTOs;

public class LoginResponseDto {
	[Required]
	public string Token { get; set; } = string.Empty;
	[Required]
	public UserInfoDto User { get; set; } = new();
}