using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyAdmin.Infrastructure.Data.Configurations;

/// <summary>Deterministic EF relationship configuration for <see cref="global::MyAdmin.Domain.Entities.Core.Payroll"/>.</summary>
public class PayrollConfiguration : IEntityTypeConfiguration<global::MyAdmin.Domain.Entities.Core.Payroll>
{
    public void Configure(EntityTypeBuilder<global::MyAdmin.Domain.Entities.Core.Payroll> builder)
    {
        builder.HasOne(x => x.Employee).WithMany(y => y.Payrolls).HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}
