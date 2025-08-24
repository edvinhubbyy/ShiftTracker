using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTracker.Data.Models;

namespace ShiftTracker.Data.Configurations
{
    public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
    {
        public void Configure(EntityTypeBuilder<Shift> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasOne(s => s.Employee)
                .WithMany(e => e.Shifts)
                .HasForeignKey(s => s.EmployeeId)
                .IsRequired(false)                
                .OnDelete(DeleteBehavior.Restrict); 


            builder.HasMany(s => s.Breaks)
                .WithOne(b => b.Shift)
                .HasForeignKey(b => b.ShiftId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.AuditLogs)
                .WithOne(al => al.Shift)
                .HasForeignKey(al => al.ShiftId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
