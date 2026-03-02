namespace Serverside.Models;

public class User {
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Name { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;
	public string Role { get; set; } = "User";
	public DateTime Created { get; set; } = DateTime.UtcNow;
	public DateTime Updated { get; set; } = DateTime.UtcNow;
}