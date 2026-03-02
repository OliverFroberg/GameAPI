using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Serverside.DTOs;
using Serverside.Models;
using Serverside.Repositories;

namespace Serverside.Services;

public class AuthService(IUserRepository userRepository, IConfiguration config) {
	public async Task<LoginResponseDto?> RegisterAsync(RegisterDto dto) {
		if (await userRepository.GetByEmailAsync(dto.Email) != null)
			return null;

		var existingUsers = await userRepository.GetAllAsync();
		var role = existingUsers.Count == 0 ? "Admin" : "User";

		var user = new User {
			Name = dto.Name,
			Email = dto.Email,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
			Role = role,
		};

		await userRepository.AddAsync(user);
		var token = GenerateJwt(user);
		return new LoginResponseDto { Token = token, User = ToUserInfo(user) };
	}

	public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
	{
		var user = await userRepository.GetByEmailAsync(dto.Email);
		if (user == null)
			return null;
		if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
			return null;
		var token = GenerateJwt(user);
		return new LoginResponseDto
		{
			Token = token,
			User = ToUserInfo(user)
		};
	}
	private string GenerateJwt(User user)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, user.Id),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Name, user.Name),
			new Claim(ClaimTypes.Role, user.Role)
		};
		var token = new JwtSecurityToken(
			issuer: config["Jwt:Issuer"],
			audience: config["Jwt:Audience"],
			claims: claims,
			expires: DateTime.UtcNow.AddDays(30),
			signingCredentials: creds
		);
		return new JwtSecurityTokenHandler().WriteToken(token);
	}
	private static UserInfoDto ToUserInfo(User user)
	{
		return new UserInfoDto
		{
			Id = user.Id,
			Email = user.Email,
			Name = user.Name,
			Role = user.Role
		};
	}
}