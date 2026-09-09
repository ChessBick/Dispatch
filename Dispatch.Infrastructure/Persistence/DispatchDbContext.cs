using Dispatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dispatch.Infrastructure.Persistence
{
    public class DispatchDbContext(DbContextOptions<DispatchDbContext> options) : DbContext(options)
    {
        public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
        public DbSet<WorkOrderActivity> Activities => Set<WorkOrderActivity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkOrder>(e =>
            {
                e.ToTable("WorkOrders");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).UseIdentityColumn(1001, 1);
                e.Property(x => x.Title).IsRequired().HasMaxLength(200);
                e.Property(x => x.Description).HasMaxLength(2000);
                e.Property(x => x.AssignedTo).HasMaxLength(200);
                e.Property(x => x.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
                e.HasMany(x => x.Activities)
                    .WithOne(a => a.WorkOrder)
                    .HasForeignKey(a => a.WorkOrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(x => x.Status);
                e.Metadata.FindNavigation(nameof(WorkOrder.Activities))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<WorkOrderActivity>(e =>
            {
                e.ToTable("Activities");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).UseIdentityColumn(4001, 1);
                e.Property(x => x.Type).HasConversion<string>().HasMaxLength(30).IsRequired();
                e.Property(x => x.OldStatus).HasConversion<string?>().HasMaxLength(20);
                e.Property(x => x.NewStatus).HasConversion<string?>().HasMaxLength(20);
                e.Property(x => x.Note).HasMaxLength(1000);
                e.HasIndex(x => new { x.WorkOrderId, x.CreatedAt });
            });
        }
    }
}