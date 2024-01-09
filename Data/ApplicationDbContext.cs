using _Morafiq.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace _Morafiq.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartCompanion> CartCompanions { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Companion> Companions { get; set; }
        public DbSet<CompanionImage> CompanionImages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Visa> Visa { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CompanionSchedule> CompanionSchedule { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OrderCompanion>()
                .HasKey(os => new { os.OrderId, os.CompanionId });

            builder.Entity<CartCompanion>()
                          .HasKey(os => new { os.CartId, os.CompanionId });
			builder.Entity<Companion>()
		    .HasOne(c => c.User)
		    .WithMany()
		    .HasForeignKey(c => c.UserId)
		    .IsRequired();
		}

        public DbSet<_Morafiq.Models.OrderCompanion>? OrderCompanion { get; set; }
    }
}
