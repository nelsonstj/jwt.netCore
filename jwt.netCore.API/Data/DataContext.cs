using jwt.netCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace jwt.netCore.API.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{ }

		public DbSet<Usuario> DD_Usuarios { get; set; }
	}
}
