namespace Serverside.Repositories;

public interface IPointsRepository {
	Task<long> GetTotalAsync(string userId);
	Task<long> PostPointAsync(string userId, int amount);
	Task<long> PutTotalAsync(string userId, long total);
	Task<IReadOnlyList<(string UserId, long Total)>> GetAllOrderedByTotalDescAsync();
}