using Microsoft.EntityFrameworkCore;
using MyAdmin.Domain.Entities.Core;

namespace MyAdmin.Infrastructure.Data;

public class MyAdminDbContext : DbContext
{
    public MyAdminDbContext(DbContextOptions<MyAdminDbContext> options)
        : base(options)
    {
    }

    public DbSet<Asset> Assets { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<global::MyAdmin.Domain.Entities.Core.Payroll> Payrolls { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Asset configuration
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.ToTable("Assets");
            entity.HasKey(e => e.AssetId);
            entity.Property(e => e.AssetId).HasColumnName("AssetId");
            entity.Property(e => e.AssetName).HasColumnName("AssetName").IsRequired();
            entity.Property(e => e.Category).HasColumnName("Category").IsRequired();
            entity.Property(e => e.PurchaseDate).HasColumnName("PurchaseDate").IsRequired();
            entity.Property(e => e.Value).HasColumnName("Value").HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.DepreciatedValue).HasColumnName("DepreciatedValue").HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.Status).HasColumnName("Status").IsRequired();
        });

        // Employee configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employees");
            entity.HasKey(e => e.EmployeeId);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeId");
            entity.Property(e => e.FirstName).HasColumnName("FirstName").IsRequired();
            entity.Property(e => e.LastName).HasColumnName("LastName").IsRequired();
            entity.Property(e => e.Position).HasColumnName("Position").IsRequired();
            entity.Property(e => e.HireDate).HasColumnName("HireDate").IsRequired();
            entity.Property(e => e.Salary).HasColumnName("Salary").HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.Active).HasColumnName("Active").IsRequired();
        });

        // global::MyAdmin.Domain.Entities.Core.Payroll configuration
        modelBuilder.Entity<global::MyAdmin.Domain.Entities.Core.Payroll>(entity =>
        {
            entity.ToTable("Payrolls");
            entity.HasKey(e => e.PayrollId);
            entity.Property(e => e.PayrollId).HasColumnName("PayrollId");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeId").IsRequired();
            entity.Property(e => e.EmployeeName).HasColumnName("EmployeeName").IsRequired();
            entity.Property(e => e.Period).HasColumnName("Period").IsRequired();
            entity.Property(e => e.GrossPay).HasColumnName("GrossPay").HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.Deductions).HasColumnName("Deductions").HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.NetPay).HasColumnName("NetPay").HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.PayDate).HasColumnName("PayDate").IsRequired();
        });

        // Apply all IEntityTypeConfiguration<T> from this assembly (relationship configs)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyAdminDbContext).Assembly);
    }
}
