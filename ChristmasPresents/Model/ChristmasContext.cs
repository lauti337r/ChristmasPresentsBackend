using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

        public virtual DbSet<Kid> Kids { get; set; }
        public virtual DbSet<Present> Presents { get; set; }
        public virtual DbSet<PresentGiver> PresentGivers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("server=localhost;user=root;password=password123;database=Christmas");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kid>(entity =>
            {
                entity.ToTable("Kid");

                entity.Property(e => e.KidId).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Age)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Hidden)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(e => e.PictureUrl).HasMaxLength(600);
            });

            modelBuilder.Entity<Present>(entity =>
            {
                entity.ToTable("Present");

                entity.HasIndex(e => e.KidId, "KidId_idx");

                entity.HasIndex(e => e.PresentGiverId, "PRESENT_GIVER_ID_idx");

                entity.Property(e => e.PresentId).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Cost).HasColumnType("int(11)");

                entity.Property(e => e.KidId).HasColumnType("int(10) unsigned");

                entity.Property(e => e.ShopName).HasMaxLength(120);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.PresentGiverId).HasColumnType("int(10) unsigned");

                entity.HasOne<Kid>(p => p.Kid)
                    .WithMany()
                    .HasForeignKey(p => p.KidId)
                    .HasConstraintName("PRESENT_KID_ID");

                entity.HasOne<PresentGiver>(p => p.PresentGiver)
                    .WithMany()
                    .HasForeignKey(p => p.PresentGiverId)
                    .HasConstraintName("PRESENT_GIVER_ID");
            });

            modelBuilder.Entity<PresentGiver>(entity =>
            {
                entity.ToTable("PresentGiver");

                entity.Property(e => e.PresentGiverId).HasColumnType("int(10) unsigned");

                entity.Property(e => e.ContactEmail).HasMaxLength(64);

                entity.Property(e => e.ContactPhone).HasMaxLength(64);

                entity.Property(e => e.PaymentMethod).HasMaxLength(45);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
