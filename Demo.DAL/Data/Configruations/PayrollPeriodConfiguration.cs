using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.DAL.Data.Configruations
{
    internal class PayrollPeriodConfiguration : IEntityTypeConfiguration<PayrollPeriod>
    {
        public void Configure(EntityTypeBuilder<PayrollPeriod> builder)
        {
            builder.HasIndex(P => new { P.Month, P.Year }).IsUnique();

            builder.Property(P => P.TotalNetSalary).HasColumnType("decimal(18,2)");

            builder.HasMany(P => P.Payslips)
                .WithOne(S => S.PayrollPeriod)
                .HasForeignKey(S => S.PayrollPeriodId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
