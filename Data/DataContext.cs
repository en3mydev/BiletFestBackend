using BiletFest.Models;
using Microsoft.EntityFrameworkCore;

namespace BiletFest.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Festival> Festivals { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderTicket> OrderTickets { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>()
                .Property(t => t.Description)
                .HasMaxLength(500); // sau orice dimensiune necesară

            modelBuilder.Entity<Festival>()
                    .HasMany(f => f.Tickets)
                    .WithOne()
                    .HasForeignKey(t => t.FestivalID)
                    .OnDelete(DeleteBehavior.Cascade);

            // Configurare pentru Order
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderTickets)
                .WithOne(ot => ot.Order)
                .HasForeignKey(ot => ot.OrderId);

            // Configurare pentru OrderTicket
            modelBuilder.Entity<OrderTicket>()
                .HasKey(ot => ot.OrderTicketId);

            modelBuilder.Entity<OrderTicket>()
                .HasOne(ot => ot.Order)
                .WithMany(o => o.OrderTickets)
                .HasForeignKey(ot => ot.OrderId);

            modelBuilder.Entity<OrderTicket>()
                .HasOne(ot => ot.Ticket)
                .WithMany()
                .HasForeignKey(ot => ot.TicketId);
        }
    }
}