using membresias.be.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace membresias.be.Configurations
{
    public class TarifaConfiguration : IEntityTypeConfiguration<Tarifa>
    {
        public void Configure(EntityTypeBuilder<Tarifa> builder)
        {
            builder.ToTable("tarifa");

            builder.HasKey(t => t.IdTarifa);
            builder.Property(t => t.IdTarifa)
                .HasColumnName("id_tarifa");

            builder.Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("nombre");

            builder.Property(t => t.Monto)
                .IsRequired()
                .HasColumnName("monto");

            builder.Property(t => t.CreatedDate)
                .IsRequired()
                .HasColumnName("created_date");

            builder.Property(t => t.ModifiedDate)
                .IsRequired()
                .HasColumnName("modified_date");

            builder.Property(t => t.MembresiaCodigo)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("membresia_codigo");

            builder.Property(t => t.ConceptoCodigo)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("concepto_codigo");

            builder.Property(t => t.IsDeleted)
                .HasColumnName("is_deleted");
        }
    }
}
