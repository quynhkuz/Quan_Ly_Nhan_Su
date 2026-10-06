using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.DAL.Data.Configruations
{
    internal class PayslipConfiguration : IEntityTypeConfiguration<Payslip>
    {
        public void Configure(EntityTypeBuilder<Payslip> builder)
        {
            builder.Property(S => S.BaseSalary).HasColumnType("decimal(18,2)");
            builder.Property(S => S.Allowance).HasColumnType("decimal(18,2)");
            builder.Property(S => S.Bonus).HasColumnType("decimal(18,2)");
            builder.Property(S => S.Deduction).HasColumnType("decimal(18,2)");
            builder.Property(S => S.Tax).HasColumnType("decimal(18,2)");

            builder.HasIndex(S => new { S.PayrollPeriodId, S.EmployeeId }).IsUnique();

            builder.HasOne(S => S.Employee)
                .WithMany()
                .HasForeignKey(S => S.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
