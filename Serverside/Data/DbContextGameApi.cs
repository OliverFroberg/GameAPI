using Microsoft.EntityFrameworkCore;
using Serverside.Models;

namespace Serverside.Data;

public class DbContextGameApi(DbContextOptions<DbContextGameApi> options) : DbContext(options) {
	public DbSet<User> Users => Set<User>();
}