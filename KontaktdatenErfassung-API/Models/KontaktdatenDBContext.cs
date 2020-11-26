using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KontaktdatenErfassung_API.Models
{
    public partial class KontaktdatenDBContext : DbContext
    {
        public KontaktdatenDBContext()
        {
        }

        public KontaktdatenDBContext(DbContextOptions<KontaktdatenDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aufenthalt> Aufenthalt { get; set; }
        public virtual DbSet<Ort> Ort { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Unternehmen> Unternehmen { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=software-projekt.database.windows.net;Database=Kontaktdaten-DB;User ID=wiuser;Password=K0ntaktdat3n");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aufenthalt>(entity =>
            {
                entity.ToTable("aufenthalt");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DatumBis).HasColumnType("datetime");

                entity.Property(e => e.DatumVon).HasColumnType("datetime");

                entity.Property(e => e.OrtId).HasColumnName("OrtID");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.HasOne(d => d.Ort)
                    .WithMany(p => p.Aufenthalt)
                    .HasForeignKey(d => d.OrtId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_aufenthalt_ort");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Aufenthalt)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_aufenthalt_person");
            });

            modelBuilder.Entity<Ort>(entity =>
            {
                entity.ToTable("ort");

                entity.Property(e => e.OrtId)
                    .HasColumnName("OrtID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Bezeichnung)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Hausnummer)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Stadt)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Straße)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UnternehmenId).HasColumnName("UnternehmenID");

                entity.HasOne(d => d.Unternehmen)
                    .WithMany(p => p.Ort)
                    .HasForeignKey(d => d.UnternehmenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ort_unternehmen");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("person");

                entity.Property(e => e.PersonId)
                    .HasColumnName("PersonID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Hausnummer)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Nachname)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Plz)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Stadt)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Straße)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Telefon)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Vorname)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Unternehmen>(entity =>
            {
                entity.ToTable("unternehmen");

                entity.Property(e => e.UnternehmenId).HasColumnName("UnternehmenID");

                entity.Property(e => e.Email)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Passwort)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Telefon)
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
