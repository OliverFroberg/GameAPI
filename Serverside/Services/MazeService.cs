using Serverside.DTOs;
using Serverside.Repositories;

namespace Serverside.Services;

public class MazeService(IMazeRepository mazeRepository, IUserRepository userRepository) {
	public async Task<GetMazeLevelDto> GetHighestCompletedLevelAsync(string userId) {
		var user = await userRepository.GetByIdAsync(userId);
		var name = user?.Name ?? userId;
		var total = await mazeRepository.GetHighestCompletedLevelAsync(userId);
		return new GetMazeLevelDto(userId, name, total);
	}

	public async Task<PostMazeLevelResponseDto> PostCompletedLevelAsync(string userId, int level) {
		var newHighestCompletedLevel = await mazeRepository.PostCompletedLevelAsync(userId, level);
		return new PostMazeLevelResponseDto(userId, newHighestCompletedLevel);
	}
}