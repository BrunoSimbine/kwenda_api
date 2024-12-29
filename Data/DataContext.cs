using Microsoft.EntityFrameworkCore;
using bilhete24.Models;

namespace bilhete24.Data;

public class DataContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseInMemoryDatabase("Database");
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Auth> Auth { get; set; }
} 