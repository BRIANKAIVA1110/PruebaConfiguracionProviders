using Microsoft.EntityFrameworkCore;
using PepeConfiguration.API.Infraestructura.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PepeConfiguration.API.Infraestructura
{
    public class ApplicationContext:DbContext
    {
        public DbSet<Configuracion> Configuraciones { get; set; }
        public ApplicationContext(DbContextOptions configure):base(configure)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
