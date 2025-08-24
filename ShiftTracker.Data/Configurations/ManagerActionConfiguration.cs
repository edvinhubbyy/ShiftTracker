using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTracker.Data.Models;

namespace ShiftTracker.Data.Configurations
{
    public class ManagerActionConfiguration : IEntityTypeConfiguration<ManagerAction>
    {
        public void Configure(EntityTypeBuilder<ManagerAction> builder)
        {
            builder.HasKey(ma => ma.Id);

            builder.Property(ma => ma.Action)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(ma => ma.Timestamp)
                .IsRequired();

            builder.HasOne(ma => ma.Manager)
                .WithMany(e => e.ManagerActions)
                .HasForeignKey(ma => ma.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ma => ma.Employee)
                .WithMany()               
                .HasForeignKey(ma => ma.EmployeeId)
                .IsRequired(false)         
                .OnDelete(DeleteBehavior.SetNull);



        }
    }
}