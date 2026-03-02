namespace Serverside.DTOs;

public class LoginPostDto {
	public string Token { get; set; } = string.Empty;
	public UserInfoDto UserInfo { get; set; } = new();
}