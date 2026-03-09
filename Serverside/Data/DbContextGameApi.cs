using Microsoft.EntityFrameworkCore;
using Serverside.Models;

namespace Serverside.Data;

public class DbContextGameApi(DbContextOptions<DbContextGameApi> options) : DbContext(options) {
	public DbSet<User> Users => Set<User>();
	public DbSet<Point> Points => Set<Point>();

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.Entity<Point>().ToTable("Point").HasKey(point => point.UserId);
	}
}