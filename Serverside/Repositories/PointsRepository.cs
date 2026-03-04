using System.Collections.Concurrent;

namespace Serverside.Repositories;

public class PointsRepository : IPointsRepository {
	private static readonly ConcurrentDictionary<string, long> _store = new();
	
	public long GetTotal(string userId) {
		return _store.GetValueOrDefault(userId, 0);
	}

	public long AddPoints(string userId, int amount) {
		return _store.AddOrUpdate(userId, amount, (_, prev) => prev + amount);
	}

	public IReadOnlyList<(string UserId, long Total)> GetAllOrderedByTotalDesc() {
		return _store
			.Select(kvp => (kvp.Key, kvp.Value))
			.OrderByDescending(x => x.Value)
			.ToList();
	}
}