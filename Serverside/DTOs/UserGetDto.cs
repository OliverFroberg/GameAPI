namespace Serverside.DTOs;

public class UserGetDto {
	public string Id { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public DateTime Created { get; set; }
	public DateTime Updated { get; set; }
}