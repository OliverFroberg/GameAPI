using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Serverside.Data;
using Serverside.Models;

namespace Serverside.Repositories;

public class PointsRepository(DbContextGameApi db) : IPointsRepository {
	public async Task<long> GetTotalAsync(string userId) {
		var row = await db.Points
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.UserId == userId);
		return row?.Total ?? 0;
	}

	public async Task<long> PostPointAsync(string userId, int amount) {
		var row = await db.Points.FirstOrDefaultAsync(x => x.UserId == userId);
		if (row == null) {
			row = new Point {UserId = userId, Total = amount};
			db.Points.Add(row);
		} else {
			row.Total += amount;
		}
		await db.SaveChangesAsync();
		return row.Total;
	}

	public async Task<long> PutTotalAsync(string userId, long total) {
		var row = await db.Points.FirstOrDefaultAsync(x => x.UserId == userId);
		if (row == null) {
			row = new Point {UserId = userId, Total = total};
			db.Points.Add(row);
		} else {
			row.Total = total;
		}
		await db.SaveChangesAsync();
		return row.Total;
	}

	public async Task<IReadOnlyList<(string UserId, long Total)>> GetAllOrderedByTotalDescAsync() {
		var list = await db.Points
			.AsNoTracking()
			.OrderByDescending(x => x.Total)
			.Select(x => new { x.UserId, x.Total })
			.ToListAsync();
		return list.Select(x => (x.UserId, x.Total)).ToList();
	}
}