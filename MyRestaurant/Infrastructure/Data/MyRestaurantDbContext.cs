using Microsoft.EntityFrameworkCore;
using MyRestaurant.Domain.Entities.Core;

namespace MyRestaurant.Infrastructure.Data;

public class MyRestaurantDbContext : DbContext
{
    public MyRestaurantDbContext(DbContextOptions<MyRestaurantDbContext> options)
        : base(options)
    {
    }

    public DbSet<Bill> Bills { get; set; } = null!;
    public DbSet<FoodCost> FoodCosts { get; set; } = null!;
    public DbSet<MenuItem> MenuItems { get; set; } = null!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Table> Tables { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Bill
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.ToTable("Bills");
            entity.HasKey(e => e.BillId);
            entity.Property(e => e.BillId).HasColumnName("BillId");
            entity.Property(e => e.OrderId).HasColumnName("OrderId");
            entity.Property(e => e.Subtotal).HasColumnName("Subtotal").HasColumnType("decimal(18,2)");
            entity.Property(e => e.Tax).HasColumnName("Tax").HasColumnType("decimal(18,2)");
            entity.Property(e => e.Tip).HasColumnName("Tip").HasColumnType("decimal(18,2)");
            entity.Property(e => e.Total).HasColumnName("Total").HasColumnType("decimal(18,2)");
            entity.Property(e => e.PaymentMethod).HasColumnName("PaymentMethod");
            entity.Property(e => e.PaidDate).HasColumnName("PaidDate");
        });

        // FoodCost
        modelBuilder.Entity<FoodCost>(entity =>
        {
            entity.ToTable("FoodCosts");
            entity.HasKey(e => e.RecipeId);
            entity.Property(e => e.RecipeId).HasColumnName("RecipeId");
            entity.Property(e => e.RecipeName).HasColumnName("RecipeName").IsRequired();
            entity.Property(e => e.IngredientCount).HasColumnName("IngredientCount");
            entity.Property(e => e.TotalCost).HasColumnName("TotalCost").HasColumnType("decimal(18,2)");
            entity.Property(e => e.SellingPrice).HasColumnName("SellingPrice").HasColumnType("decimal(18,2)");
            entity.Property(e => e.CostPercentage).HasColumnName("CostPercentage");
        });

        // MenuItem
        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.ToTable("MenuItems");
            entity.HasKey(e => e.ItemId);
            entity.Property(e => e.ItemId).HasColumnName("ItemId");
            entity.Property(e => e.ItemName).HasColumnName("ItemName").IsRequired();
            entity.Property(e => e.Category).HasColumnName("Category").IsRequired();
            entity.Property(e => e.Price).HasColumnName("Price").HasColumnType("decimal(18,2)");
            entity.Property(e => e.Cost).HasColumnName("Cost").HasColumnType("decimal(18,2)");
            entity.Property(e => e.Active).HasColumnName("Active");
        });

        // OrderDetail
        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetails");
            entity.HasKey(e => e.DetailId);
            entity.Property(e => e.DetailId).HasColumnName("DetailId");
            entity.Property(e => e.OrderId).HasColumnName("OrderId");
            entity.Property(e => e.ItemId).HasColumnName("ItemId");
            entity.Property(e => e.ItemName).HasColumnName("ItemName").IsRequired();
            entity.Property(e => e.Quantity).HasColumnName("Quantity");
            entity.Property(e => e.UnitPrice).HasColumnName("UnitPrice").HasColumnType("decimal(18,2)");
            entity.Property(e => e.Subtotal).HasColumnName("Subtotal").HasColumnType("decimal(18,2)");
        });

        // Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(e => e.OrderId);
            entity.Property(e => e.OrderId).HasColumnName("OrderId");
            entity.Property(e => e.TableId).HasColumnName("TableId");
            entity.Property(e => e.OrderDate).HasColumnName("OrderDate");
            entity.Property(e => e.Status).HasColumnName("Status");
            entity.Property(e => e.TotalAmount).HasColumnName("TotalAmount").HasColumnType("decimal(18,2)");
        });

        // Table
        modelBuilder.Entity<Table>(entity =>
        {
            entity.ToTable("Tables");
            entity.HasKey(e => e.TableId);
            entity.Property(e => e.TableId).HasColumnName("TableId");
            entity.Property(e => e.TableNumber).HasColumnName("TableNumber");
            entity.Property(e => e.Capacity).HasColumnName("Capacity");
            entity.Property(e => e.Status).HasColumnName("Status");
            entity.Property(e => e.Zone).HasColumnName("Zone").IsRequired();
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyRestaurantDbContext).Assembly);
    }
}
