using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Data.MetaData
{
    public class CodigoDeVerificaiconMetaData : IEntityTypeConfiguration<CodigosDeVerificacion>
    {
        public void Configure(EntityTypeBuilder<CodigosDeVerificacion> builder)
        {
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.Código).IsRequired();
        }
    }
}
