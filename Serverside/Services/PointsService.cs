using Serverside.DTOs;
using Serverside.Repositories;

namespace Serverside.Services;

public class PointsService(IPointsRepository pointsRepository) {
	public UserPointsDto GetUserPoints(string userId) {
		var total = pointsRepository.GetTotal(userId);
		return new UserPointsDto(userId, total);
	}
	
	public AddPointsResultDto AddPoints(string userId, string source, int amount) {
		var safeAmount = Math.Max(amount, 1);
		var safeSource = string.IsNullOrWhiteSpace(source) ? "unknown" : source.Trim();
		var newTotal = pointsRepository.AddPoints(userId, safeAmount);
		return new AddPointsResultDto(userId, newTotal, safeAmount, safeSource);
	}

	public List<UserPointsDto> GetAllUserPoints() {
		return pointsRepository
			.GetAllOrderedByTotalDesc()
			.Select(x => new UserPointsDto(x.UserId, x.Total))
			.ToList();
	}
}