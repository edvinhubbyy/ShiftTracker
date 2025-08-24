using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTracker.Data.Models;

namespace ShiftTracker.Data.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.HasKey(al => al.Id);

            builder.HasOne(al => al.Employee)
                .WithMany(e => e.AuditLogs)
                .HasForeignKey(al => al.EmployeeId)
                .IsRequired(false)              
                .OnDelete(DeleteBehavior.SetNull);


            builder.HasOne(al => al.Shift)
                .WithMany(s => s.AuditLogs)
                .HasForeignKey(al => al.ShiftId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
