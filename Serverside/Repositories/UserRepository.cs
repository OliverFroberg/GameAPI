using Microsoft.EntityFrameworkCore;
using Serverside.Data;
using Serverside.Models;
using Serverside.Services;

namespace Serverside.Repositories;

public class UserRepository(DbContextGameApi dbContext) : IUserRepository {
	public async Task<List<User>> GetAllAsync() {
		return await dbContext.Users.ToListAsync();
	}

	public async Task<User?> GetByIdAsync(string id) {
		return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
	}

	public async Task<User?> GetByEmailAsync(string email) {
		return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
	}

	public async Task<User> AddAsync(User user) {
		if (string.IsNullOrWhiteSpace(user.Id))
			user.Id = Guid.NewGuid().ToString();

		user.Created = DateTime.UtcNow;
		user.Updated = DateTime.UtcNow;

		dbContext.Users.Add(user);
		await dbContext.SaveChangesAsync();

		return user;
	}

	public async Task<bool> UpdateAsync(User user) {
		var existing = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
		if (existing == null) return false;

		existing.Email = user.Email;
		existing.Name = user.Name;
		existing.Updated = DateTime.UtcNow;

		await dbContext.SaveChangesAsync();
		return true;
	}

	public async Task<bool> DeleteAsync(string id) {
		var existing = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
		if (existing == null) return false;

		dbContext.Users.Remove(existing);
		await dbContext.SaveChangesAsync();
		return true;
	}
}