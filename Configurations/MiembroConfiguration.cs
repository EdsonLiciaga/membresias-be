using membresias.be.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace membresias.be.Configurations
{
    public class MiembroConfiguration : IEntityTypeConfiguration<Miembro>
    {
        public void Configure(EntityTypeBuilder<Miembro> builder)
        {
            builder.ToTable("miembro");

            builder.HasKey(m => m.IdMiembro);
            builder.Property(m => m.IdMiembro)
                .HasColumnName("id_miembro");

            builder.Property(m => m.Clave)
                .HasMaxLength(100)
                .HasColumnName("clave"); 

            builder.Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("nombre");

            builder.Property(m => m.PrimerApellido)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("primer_apellido");

            builder.Property(m => m.SegundoApellido)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("segundo_apellido");

            builder.Property(m => m.Curp)
                .IsRequired()
                .HasMaxLength(18)
                .HasColumnName("curp");

            builder.Property(m => m.FechaNacimiento)
                .IsRequired()
                .HasColumnName("fecha_nacimiento");

            builder.Property(m => m.Edad)
                .HasColumnName("edad");

            builder.Property(m => m.FechaIngreso)
                .HasColumnName("fecha_ingreso"); 

            builder.Property(m => m.FechaReingreso)
                .HasColumnName("fecha_reingreso");

            builder.Property(m => m.CreatedDate)
                .IsRequired()
                .HasColumnName("created_date");

            builder.Property(m => m.ModifiedDate)
                .IsRequired()
                .HasColumnName("modified_date");

            builder.Property(m => m.IsDeleted)
                .HasColumnName("is_deleted");

            builder.Property(m => m.MembresiaCodigo)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("membresia_codigo");

            builder.Property(m => m.MiembroEstatusCodigo)
                .HasMaxLength(100)
                .HasColumnName("miembro_estatus_codigo"); 
        }
    }
}
