using Serverside.Models;

namespace Serverside.Repositories;

public interface IUserRepository {
	Task<List<User>> GetAllAsync();
	Task<User?> GetByIdAsync(string id);
	Task<User?> GetByEmailAsync(string email);
	Task<User> AddAsync(User user);
	Task<bool> UpdateAsync(User user);
	Task<bool> DeleteAsync(string id);
}