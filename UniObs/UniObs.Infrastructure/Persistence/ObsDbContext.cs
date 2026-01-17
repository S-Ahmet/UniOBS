using Microsoft.EntityFrameworkCore;
using UniObs.Domain.Entities;

namespace UniObs.Infrastructure.Persistence
{
    public class ObsDbContext : DbContext
    {
        public ObsDbContext(DbContextOptions<ObsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Yonetici> Yoneticiler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Yonetici>(entity =>
            {
                entity.ToTable("yoneticiler");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Email)
                      .HasColumnName("email")
                      .HasMaxLength(200)
                      .IsRequired();

                entity.Property(x => x.Sifre)
                      .HasColumnName("sifre")
                      .HasMaxLength(500)
                      .IsRequired();

                entity.Property(x => x.FailedAccessCount)
                      .HasDefaultValue(0);

                entity.Property(x => x.LockoutEnd);

                entity.Property(x => x.Role)
                      .HasColumnName("role")
                      .HasMaxLength(50)
                      .IsRequired();
                entity.Property(x => x.TwoFactorCode)
                      .HasColumnName("two_factor_code")
                      .HasMaxLength(10);

                entity.Property(x => x.TwoFactorExpiresAt)
                      .HasColumnName("two_factor_expires_at");
            });
        }
    }
}
