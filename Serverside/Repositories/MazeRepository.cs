using Microsoft.EntityFrameworkCore;
using Serverside.Data;
using Serverside.Models;

namespace Serverside.Repositories;

public class MazeRepository(DbContextGameApi db) : IMazeRepository {
	public async Task<int> GetHighestCompletedLevelAsync(string userId) {
		var row = await db.MazeLevels
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.UserId == userId);
		return row?.Level ?? 0;
	}

	public async Task<int> PostCompletedLevelAsync(string userId, int level) {
		var row = await db.MazeLevels.FirstOrDefaultAsync(x => x.UserId == userId);
		if (row == null) {
			row = new MazeLevel {UserId = userId, Level = level};
			db.MazeLevels.Add(row);
		} else {
			row.Level += level;
		}
		await db.SaveChangesAsync();
		return row.Level;
	}
}