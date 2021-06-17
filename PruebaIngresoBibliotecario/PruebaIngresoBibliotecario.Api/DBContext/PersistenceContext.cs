using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PruebaIngresoBibliotecario.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.DBContext
{
    public class PersistenceContext : DbContext
    {

        private readonly IConfiguration Config;

        public PersistenceContext(DbContextOptions<PersistenceContext> options, IConfiguration config) : base(options)
        {
            Config = config;
        }

        public PersistenceContext(DbContextOptions<PersistenceContext> options) : base(options)
        {
        }

        public async Task CommitAsync()
        {
            await SaveChangesAsync().ConfigureAwait(false);
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.HasDefaultSchema(Config.GetValue<string>("SchemaName"));

        //    base.OnModelCreating(modelBuilder);
        //}


        public DbSet<PrestamosE> Prestamos { get; set; }
    }
}
