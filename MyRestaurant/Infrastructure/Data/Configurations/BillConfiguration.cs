using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyRestaurant.Infrastructure.Data.Configurations;

/// <summary>Deterministic EF relationship configuration for <see cref="global::MyRestaurant.Domain.Entities.Core.Bill"/>.</summary>
public class BillConfiguration : IEntityTypeConfiguration<global::MyRestaurant.Domain.Entities.Core.Bill>
{
    public void Configure(EntityTypeBuilder<global::MyRestaurant.Domain.Entities.Core.Bill> builder)
    {
        builder.HasOne(x => x.Order).WithMany(y => y.Bills).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
    }
}
