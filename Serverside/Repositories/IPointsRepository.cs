namespace Serverside.Repositories;

public interface IPointsRepository {
	long GetTotal(string userId);
	long AddPoints(string userId, int amount);
	IReadOnlyList<(string UserId, long Total)> GetAllOrderedByTotalDesc();
}