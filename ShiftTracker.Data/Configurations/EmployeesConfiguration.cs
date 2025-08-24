using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTracker.Data.Models;

namespace ShiftTracker.Data.Configurations
{
    
    public class EmployeesConfiguration : IEntityTypeConfiguration<Employees>
    {
        public void Configure(EntityTypeBuilder<Employees> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.CardId).IsUnique();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.CardId)
                .IsRequired();

            builder.Property(e => e.Pin)
                .IsRequired();

            builder.HasMany(e => e.Shifts)
                .WithOne(s => s.Employee)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.AuditLogs)
                .WithOne(al => al.Employee)
                .HasForeignKey(al => al.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.ManagerActions)
                .WithOne(ma => ma.Manager)
                .HasForeignKey(ma => ma.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }

}
