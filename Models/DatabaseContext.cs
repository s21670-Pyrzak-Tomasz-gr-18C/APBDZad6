﻿using Microsoft.EntityFrameworkCore;

#nullable disable

namespace trips.Models
{
	public partial class DatabaseContext : DbContext
	{
		public DatabaseContext()
		{
		}

		public DatabaseContext(DbContextOptions<DatabaseContext> options)
			: base(options)
		{
		}

		public virtual DbSet<Client> Clients { get; set; }
		public virtual DbSet<Client_Trip> ClientTrips { get; set; }
		public virtual DbSet<Country> Countries { get; set; }
		public virtual DbSet<Country_Trip> CountryTrips { get; set; }
		public virtual DbSet<Trip> Trips { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

			modelBuilder.Entity<Client>(entity =>
			{
				entity.HasKey(e => e.IdClient)
					.HasName("Client_pk");

				entity.ToTable("Client");

				entity.Property(e => e.Email)
					.IsRequired()
					.HasMaxLength(120);

				entity.Property(e => e.FirstName)
					.IsRequired()
					.HasMaxLength(120);

				entity.Property(e => e.LastName)
					.IsRequired()
					.HasMaxLength(120);

				entity.Property(e => e.Pesel)
					.IsRequired()
					.HasMaxLength(120);

				entity.Property(e => e.Telephone)
					.IsRequired()
					.HasMaxLength(120);
			});

			modelBuilder.Entity<Client_Trip>(entity =>
			{
				entity.HasKey(e => new { e.IdClient, e.IdTrip })
					.HasName("Client_Trip_pk");

				entity.ToTable("Client_Trip");

				entity.Property(e => e.PaymentDate).HasColumnType("datetime");

				entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

				entity.HasOne(d => d.IdClientNavigation)
					.WithMany(p => p.ClientTrips)
					.HasForeignKey(d => d.IdClient)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("Table_5_Client");

				entity.HasOne(d => d.IdTripNavigation)
					.WithMany(p => p.ClientTrips)
					.HasForeignKey(d => d.IdTrip)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("Table_5_Trip");
			});

			modelBuilder.Entity<Country>(entity =>
			{
				entity.HasKey(e => e.IdCountry)
					.HasName("Country_pk");

				entity.ToTable("Country");

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(120);
			});

			modelBuilder.Entity<Country_Trip>(entity =>
			{
				entity.HasKey(e => new { e.IdCountry, e.IdTrip })
					.HasName("Country_Trip_pk");

				entity.ToTable("Country_Trip");

				entity.HasOne(d => d.IdCountryNavigation)
					.WithMany(p => p.CountryTrips)
					.HasForeignKey(d => d.IdCountry)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("Country_Trip_Country");

				entity.HasOne(d => d.IdTripNavigation)
					.WithMany(p => p.CountryTrips)
					.HasForeignKey(d => d.IdTrip)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("Country_Trip_Trip");
			});

			modelBuilder.Entity<Trip>(entity =>
			{
				entity.HasKey(e => e.IdTrip)
					.HasName("Trip_pk");

				entity.ToTable("Trip");

				entity.Property(e => e.DateFrom).HasColumnType("datetime");

				entity.Property(e => e.DateTo).HasColumnType("datetime");

				entity.Property(e => e.Description)
					.IsRequired()
					.HasMaxLength(220);

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(120);
			});

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
