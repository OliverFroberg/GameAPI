using Serverside.DTOs;
using Serverside.Repositories;

namespace Serverside.Services;

public class PointsService(IPointsRepository pointsRepository, IUserRepository userRepository) {
	public async Task<GetPointDto> GetPointsAsync(string userId) {
		var user = await userRepository.GetByIdAsync(userId);
		var name = user?.Name ?? userId;
		var total = await pointsRepository.GetTotalAsync(userId);
		return new GetPointDto(userId, name, total);
	}

	public async Task<PostPointResponseDto> PostPointAsync(string userId, string source, int amount) {
		var safeAmount = amount is >= 1 and <= 100 ? amount : 1;
		var safeSource = string.IsNullOrWhiteSpace(source) ? "unknown" : source.Trim();
		var newTotal = await pointsRepository.PostPointAsync(userId,
			safeAmount);
		return new PostPointResponseDto(userId, newTotal,
			safeAmount, safeSource);
	}

	public async Task<List<GetPointDto>> GetAllPointsAsync() {
		var pointsList = await pointsRepository.GetAllOrderedByTotalDescAsync();
		var result = new List<GetPointDto>();
		foreach (var (userId, total) in pointsList) {
			var user = await userRepository.GetByIdAsync(userId);
			var name = user?.Name ?? userId;
			result.Add(new GetPointDto(userId, name, total));
		}

		return result;
	}

	public async Task<GetPointDto?> GetPointsForUserAsync(string userId) {
		var user = await userRepository.GetByIdAsync(userId);
		var name = user?.Name ?? userId;
		if (user == null)
			return null;
		var total = await pointsRepository.GetTotalAsync(userId);
		return new GetPointDto(userId, name, total);
	}

	public async Task<GetPointDto?> PutPointAsync(string userId, long total) {
		var user = await userRepository.GetByIdAsync(userId);
		var name = user?.Name ?? userId;
		if (user == null)
			return null;
		var newTotal = await pointsRepository.PutTotalAsync(userId, total);
		return new GetPointDto(userId, name, newTotal);
	}
}