using Serverside.DTOs;
using Serverside.Models;
using Serverside.Repositories;

namespace Serverside.Services;

public class UserService(IUserRepository userRepository) {
	public async Task<List<UserGetDto>> GetAllUsersAsync() {
		var users = await userRepository.GetAllAsync();
		return users.Select(MapToGetDto).ToList();
	}

	public async Task<UserGetDto?> GetUserByIdAsync(string id) {
		var user = await userRepository.GetByIdAsync(id);
		return user == null ? null : MapToGetDto(user);
	}

	public async Task<UserGetDto?> GetUserByEmailAsync(string email) {
		var user = await userRepository.GetByEmailAsync(email);
		return user == null ? null : MapToGetDto(user);
	}

	public async Task<UserGetDto> CreateUserAsync(UserPostDto user) {
		var newUser = new User {
			Name = user.Name,
			Email = user.Email
		};
		await userRepository.AddAsync(newUser);
		return MapToGetDto(newUser);
	}

	public async Task<bool> UpdateUserAsync(string id, UserUpdateDto user) {
		var existingUser = await userRepository.GetByIdAsync(id);
		if (existingUser == null) return false;
		if (user.Name != null)
			existingUser.Name = user.Name;
		if (user.Email != null)
			existingUser.Email = user.Email;
		return await userRepository.UpdateAsync(existingUser);
	}

	public async Task<bool> DeleteUserAsync(string id, UserDeleteDto deleteDto = null) {
		return await userRepository.DeleteAsync(id);
	}

	private static UserGetDto MapToGetDto(User user) {
		return new UserGetDto {
			Id = user.Id,
			Name = user.Name,
			Email = user.Email,
			Created = user.Created,
			Updated = user.Updated
		};
	}
}