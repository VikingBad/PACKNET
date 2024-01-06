using Microsoft.EntityFrameworkCore;
using PACKNET.Models;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PACKNET
{
    public class PackNetDbContext : DbContext
    {
        public PackNetDbContext()
        {
        }

        public PackNetDbContext(DbContextOptions<PackNetDbContext> options)
        : base(options)
        {
        }

        public DbSet<Vehiculo> Vehiculo { get; set; }
        public DbSet<Paquete> Paquete { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.ToTable("Vehiculos");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Matricula).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Marca).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Modelo).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Latitud).IsRequired();
                entity.Property(e => e.Longitud).IsRequired();
                entity.Property(e => e.Ultima_Actualizacion).IsRequired();

                entity.HasMany(v => v.ListaPaquetes)
                .WithOne(p => p.Vehiculo)
                .HasForeignKey(p => p.Id_Vehiculo);
            });


            modelBuilder.Entity<Paquete>(entity =>
            {
                entity.ToTable("Paquetes");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Id_Vehiculo).IsRequired();

                entity.HasOne(p => p.Vehiculo)
                    .WithMany(v => v.ListaPaquetes)
                    .HasForeignKey(p => p.Id_Vehiculo);
            });


        }
    }
}
