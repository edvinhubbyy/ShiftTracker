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
    public class BreakConfiguration : IEntityTypeConfiguration<Break>
    {
        public void Configure(EntityTypeBuilder<Break> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Shift)
                .WithMany(s => s.Breaks)
                .HasForeignKey(b => b.ShiftId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
