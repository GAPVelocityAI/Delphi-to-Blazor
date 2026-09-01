using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyRestaurant.Infrastructure.Data.Configurations;

/// <summary>Deterministic EF relationship configuration for <see cref="global::MyRestaurant.Domain.Entities.Core.OrderDetail"/>.</summary>
public class OrderDetailConfiguration : IEntityTypeConfiguration<global::MyRestaurant.Domain.Entities.Core.OrderDetail>
{
    public void Configure(EntityTypeBuilder<global::MyRestaurant.Domain.Entities.Core.OrderDetail> builder)
    {
        builder.HasOne(x => x.Order).WithMany(y => y.OrderDetails).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
    }
}
