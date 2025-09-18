using membresias.be.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace membresias.be.Configurations
{
    public class CargoConfiguration : IEntityTypeConfiguration<Cargo>
    {
        public void Configure(EntityTypeBuilder<Cargo> builder)
        {
            builder.ToTable("cargo");

            builder.HasKey(c => c.IdCargo);
            builder.Property(c => c.IdCargo)
                .HasColumnName("id_cargo");

            builder.HasOne(c => c.Miembro)
                .WithMany()
                .HasForeignKey(c => c.IdMiembro);
            builder.Property(c => c.IdMiembro)
                .HasColumnName("id_miembro"); 

            builder.Property(c => c.Monto)
                .IsRequired()
                .HasColumnName("monto");

            builder.Property(c => c.FechaCargo)
                .IsRequired()
                .HasColumnName("fecha_cargo");

            builder.Property(c => c.IsPagado)
                .IsRequired()
                .HasColumnName("is_pagado");

            builder.Property(c => c.CreatedDate)
                .IsRequired()
                .HasColumnName("created_date");

            builder.Property(c => c.ModifiedDate)
                .IsRequired()
                .HasColumnName("modified_date");

            builder.Property(c => c.IsDeleted)
                .HasColumnName("is_deleted");

            builder.Property(c => c.ConceptoCodigo)
                .HasColumnName("concepto_codigo");

            builder.Property(c => c.Descripcion)
                .HasMaxLength(100)
                .HasColumnName("descripcion"); 
        }
    }
}
