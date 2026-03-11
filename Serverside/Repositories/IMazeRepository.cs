namespace Serverside.Repositories;

public interface IMazeRepository {
	Task<int> GetHighestCompletedLevelAsync(string userId);
	Task<int> PostCompletedLevelAsync(string userId, int level);
}