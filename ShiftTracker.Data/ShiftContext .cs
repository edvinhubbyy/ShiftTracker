using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ShiftTracker.Data.Models;

namespace ShiftTracker.Data
{
    public class ShiftContext : DbContext
    {

        public ShiftContext(DbContextOptions<ShiftContext> options) : base(options)
        {

        }

        public DbSet<Employees> Employees { get; set; } = null!;

        public DbSet<Position> Positions { get; set; } = null!;

        public DbSet<Role> Roles { get; set; } = null!;

        public DbSet<Shift> Shifts { get; set; } = null!;

        public DbSet<Break> Breaks { get; set; } = null!;

        public DbSet<AuditLog> AuditLogs { get; set; } = null!;

        public DbSet<ManagerAction> ManagerActions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
