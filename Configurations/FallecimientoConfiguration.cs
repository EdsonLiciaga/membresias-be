using membresias.be.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace membresias.be.Configurations
{
    public class FallecimientoConfiguration : IEntityTypeConfiguration<Fallecimiento>
    {
        public void Configure(EntityTypeBuilder<Fallecimiento> builder)
        {
            builder.ToTable("fallecimiento");

            builder.HasKey(f => f.IdFallecimiento);
            builder.Property(f => f.IdFallecimiento)
                .HasColumnName("id_fallecimiento");

            builder.HasOne(f => f.Miembro)
                .WithMany()
                .HasForeignKey(f => f.IdMiembro);
            builder.Property(f => f.IdMiembro)
                .HasColumnName("id_miembro"); 

            builder.Property(f => f.FechaFallecimiento)
                .IsRequired()
                .HasColumnName("fecha_fallecimiento");

            builder.Property(f => f.CreatedDate)
                .IsRequired()
                .HasColumnName("created_date");

            builder.Property(f => f.ModifiedDate)
                .IsRequired()
                .HasColumnName("modified_date");

            builder.Property(f => f.IsDeleted)
                .HasColumnName("is_deleted");
        }
    }
}
