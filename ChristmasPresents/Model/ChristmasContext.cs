using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace ChristmasPresents.Model
{
    public partial class ChristmasContext : DbContext
    {
        public ChristmasContext()
        {
        }

        public ChristmasContext(DbContextOptions<ChristmasContext> options)
            : base(options)
        {
        }

        public static string GetConnectionString()
        {
            return Startup.ConnectionString;
        }

        public virtual DbSet<Kid> Kids { get; set; }
        public virtual DbSet<Present> Presents { get; set; }
        public virtual DbSet<PresentGiver> PresentGivers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL(GetConnectionString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kid>(entity =>
            {
                entity.ToTable("Kid");

                entity.Property(e => e.KidId).HasColumnType("int(10) unsigned").HasColumnName("KidId");

                entity.Property(e => e.Age)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasDefaultValueSql("'0'").HasColumnName("Age");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(45).HasColumnName("Area");

                entity.Property(e => e.Hidden)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'").HasColumnName("Hidden");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(120)
                    .HasColumnName("Name");

                entity.Property(e => e.Number)
                    .HasDefaultValueSql("'0'")
                    .HasColumnName("Number");

                entity.Property(e => e.PictureUrl).HasMaxLength(600).HasColumnName("PictureUrl");
            });

            modelBuilder.Entity<Present>(entity =>
            {
                entity.ToTable("Present");

                entity.HasIndex(e => e.KidId, "KidId_idx");

                entity.HasIndex(e => e.PresentGiverId, "PRESENT_GIVER_ID_idx");

                entity.Property(e => e.PresentId).HasColumnType("int(10) unsigned").HasColumnName("PresentId");

                entity.Property(e => e.Cost).HasColumnType("int(11)").HasColumnName("Cost");

                entity.Property(e => e.KidId).HasColumnType("int(10) unsigned").HasColumnName("KidId");

                entity.Property(e => e.ShopName).HasMaxLength(120).HasColumnName("ShopName");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64).HasColumnName("Name");

                entity.Property(e => e.PresentGiverId).HasColumnType("int(10) unsigned").HasColumnName("PresentGiverId");

                entity.HasOne<Kid>(p => p.Kid)
                    .WithOne(k => k.Present)
                    .HasForeignKey<Present>(p => p.KidId)
                    .HasConstraintName("PRESENT_KID_ID");

                entity.HasOne<PresentGiver>(p => p.PresentGiver)
                    .WithOne(pg => pg.Present)
                    .HasForeignKey<Present>(p => p.PresentGiverId)
                    .HasConstraintName("PRESENT_GIVER_ID");
            });

            modelBuilder.Entity<PresentGiver>(entity =>
            {
                entity.ToTable("PresentGiver");

                entity.Property(e => e.PresentGiverId).HasColumnType("int(10) unsigned");

                entity.Property(e => e.ContactEmail).HasMaxLength(64);

                entity.Property(e => e.ContactPhone).HasMaxLength(64);

                entity.Property(e => e.PaymentMethod).HasMaxLength(45);

                entity.Property(e => e.PaymentMade)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'").HasColumnName("PaymentMade");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
