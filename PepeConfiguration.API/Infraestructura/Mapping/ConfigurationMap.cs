using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PepeConfiguration.API.Infraestructura.Entities;

namespace PepeConfiguration.API.Infraestructura.Mapping
{
    public class ConfigurationMap : IEntityTypeConfiguration<Configuracion>
    {
        public void Configure(EntityTypeBuilder<Configuracion> builder)
        {
            builder.ToTable("Configurations");
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Section);
            builder.Property(x => x.Key);
            builder.Property(x => x.Value);
        }
    }
}
