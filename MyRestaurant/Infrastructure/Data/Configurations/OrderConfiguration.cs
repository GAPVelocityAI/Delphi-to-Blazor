using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyRestaurant.Infrastructure.Data.Configurations;

/// <summary>Deterministic EF relationship configuration for <see cref="global::MyRestaurant.Domain.Entities.Core.Order"/>.</summary>
public class OrderConfiguration : IEntityTypeConfiguration<global::MyRestaurant.Domain.Entities.Core.Order>
{
    public void Configure(EntityTypeBuilder<global::MyRestaurant.Domain.Entities.Core.Order> builder)
    {
        builder.HasOne(x => x.Table).WithMany(y => y.Orders).HasForeignKey(x => x.TableId).OnDelete(DeleteBehavior.Restrict);
    }
}
