using Microsoft.EntityFrameworkCore;

namespace Models.Context
{
	public class MainContext : DbContext
	{

		public MainContext(DbContextOptions<MainContext> options)
										: base(options)
		{ }

		public virtual DbSet<Users> Users { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			
		}
	}
}
