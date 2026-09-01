using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRestaurant.Infrastructure.Data;

namespace MyRestaurant.Application.Services.MyRestaurant;

public class uTablesBLService : IuTablesBLService
{
    private readonly IDbContextFactory<MyRestaurantDbContext> _dbContextFactory;
    private readonly ILogger<uTablesBLService> _logger;

    public uTablesBLService(
        IDbContextFactory<MyRestaurantDbContext> dbContextFactory,
        ILogger<uTablesBLService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    /// <summary>
    /// Legacy EnsureInitialized — seeds the Tables table if it is empty.
    /// Mirrors the 15 hard-coded rows from uTablesBL.EnsureInitialized.
    /// </summary>
    public async Task EnsureInitializedAsync(CancellationToken ct = default)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        if (await db.Tables.AnyAsync(ct))
            return;

        _logger.LogInformation("Seeding initial table data (15 rows).");

        var seeds = new List<global::MyRestaurant.Domain.Entities.Core.Table>
        {
            new() { TableNumber = 1,  Capacity = 4, Status = TTableStatus.Occupied,  Zone = "Main Hall" },
            new() { TableNumber = 2,  Capacity = 4, Status = TTableStatus.Available, Zone = "Main Hall" },
            new() { TableNumber = 3,  Capacity = 2, Status = TTableStatus.Occupied,  Zone = "Main Hall" },
            new() { TableNumber = 4,  Capacity = 6, Status = TTableStatus.Available, Zone = "Main Hall" },
            new() { TableNumber = 5,  Capacity = 2, Status = TTableStatus.Reserved,  Zone = "Main Hall" },
            new() { TableNumber = 6,  Capacity = 4, Status = TTableStatus.Available, Zone = "Terrace" },
            new() { TableNumber = 7,  Capacity = 6, Status = TTableStatus.Occupied,  Zone = "Terrace" },
            new() { TableNumber = 8,  Capacity = 2, Status = TTableStatus.Available, Zone = "Terrace" },
            new() { TableNumber = 9,  Capacity = 4, Status = TTableStatus.Occupied,  Zone = "Terrace" },
            new() { TableNumber = 10, Capacity = 8, Status = TTableStatus.Available, Zone = "Private" },
            new() { TableNumber = 11, Capacity = 8, Status = TTableStatus.Reserved,  Zone = "Private" },
            new() { TableNumber = 12, Capacity = 6, Status = TTableStatus.Occupied,  Zone = "Private" },
            new() { TableNumber = 13, Capacity = 2, Status = TTableStatus.Available, Zone = "Bar Area" },
            new() { TableNumber = 14, Capacity = 2, Status = TTableStatus.Occupied,  Zone = "Bar Area" },
            new() { TableNumber = 15, Capacity = 4, Status = TTableStatus.Closed,    Zone = "Bar Area" },
        };

        db.Tables.AddRange(seeds);
        await db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Synchronous legacy EnsureInitialized wrapper.
    /// </summary>
    public void EnsureInitialized()
    {
        EnsureInitializedAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy GetTables — returns all tables ordered by Zone then TableNumber.
    /// </summary>
    public async Task<List<TTableInfo>> GetTablesAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entities = await db.Tables
            .AsNoTracking()
            .OrderBy(t => t.Zone)
            .ThenBy(t => t.TableNumber)
            .ToListAsync(ct);

        return entities.ConvertAll(MapToDto);
    }

    /// <summary>
    /// Synchronous legacy GetTables wrapper.
    /// </summary>
    public List<TTableInfo> GetTables()
    {
        return GetTablesAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy GetAvailableTables — returns only tables with Status == Available,
    /// ordered by Zone then TableNumber.
    /// </summary>
    public async Task<List<TTableInfo>> GetAvailableTablesAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entities = await db.Tables
            .AsNoTracking()
            .Where(t => t.Status == TTableStatus.Available)
            .OrderBy(t => t.Zone)
            .ThenBy(t => t.TableNumber)
            .ToListAsync(ct);

        return entities.ConvertAll(MapToDto);
    }

    /// <summary>
    /// Synchronous legacy GetAvailableTables wrapper.
    /// </summary>
    public List<TTableInfo> GetAvailableTables()
    {
        return GetAvailableTablesAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy GetTablesByZone — returns tables matching the given zone (case-insensitive),
    /// ordered by TableNumber.
    /// </summary>
    public async Task<List<TTableInfo>> GetTablesByZoneAsync(string zone, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        // EF Core on SQL Server uses case-insensitive collation by default,
        // matching legacy SameText behavior.
        var entities = await db.Tables
            .AsNoTracking()
            .Where(t => t.Zone == zone)
            .OrderBy(t => t.TableNumber)
            .ToListAsync(ct);

        return entities.ConvertAll(MapToDto);
    }

    /// <summary>
    /// Synchronous legacy GetTablesByZone wrapper.
    /// </summary>
    public List<TTableInfo> GetTablesByZone(string zone)
    {
        return GetTablesByZoneAsync(zone).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy AddTable — inserts a new table row. The database assigns the ID
    /// (identity column), matching the legacy pattern of assigning FNextId.
    /// </summary>
    public async Task AddTableAsync(TTableInfo table, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entity = new global::MyRestaurant.Domain.Entities.Core.Table
        {
            TableNumber = table.TableNumber,
            Capacity = table.Capacity,
            Status = (TTableStatus)table.Status,
            Zone = table.Zone ?? string.Empty,
        };

        db.Tables.Add(entity);
        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Added table {TableId} (Number={Number}).", entity.TableId, entity.TableNumber);
    }

    /// <summary>
    /// Synchronous legacy AddTable wrapper.
    /// </summary>
    public void AddTable(TTableInfo table)
    {
        AddTableAsync(table).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy UpdateTable — finds the table by TableId and overwrites all fields.
    /// </summary>
    public async Task UpdateTableAsync(TTableInfo table, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entity = await db.Tables.FirstOrDefaultAsync(t => t.TableId == table.TableId, ct);
        if (entity is null)
        {
            _logger.LogWarning("UpdateTable: TableId {TableId} not found.", table.TableId);
            return;
        }

        entity.TableNumber = table.TableNumber;
        entity.Capacity = table.Capacity;
        entity.Status = (TTableStatus)table.Status;
        entity.Zone = table.Zone ?? string.Empty;

        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Updated table {TableId}.", table.TableId);
    }

    /// <summary>
    /// Synchronous legacy UpdateTable wrapper.
    /// </summary>
    public void UpdateTable(TTableInfo table)
    {
        UpdateTableAsync(table).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Legacy DeleteTable — removes the table row by TableId.
    /// </summary>
    public async Task DeleteTableAsync(int tableId, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        var entity = await db.Tables.FirstOrDefaultAsync(t => t.TableId == tableId, ct);
        if (entity is null)
        {
            _logger.LogWarning("DeleteTable: TableId {TableId} not found.", tableId);
            return;
        }

        db.Tables.Remove(entity);
        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Deleted table {TableId}.", tableId);
    }

    /// <summary>
    /// Synchronous legacy DeleteTable wrapper.
    /// </summary>
    public void DeleteTable(int tableId)
    {
        DeleteTableAsync(tableId).GetAwaiter().GetResult();
    }

    private static TTableInfo MapToDto(global::MyRestaurant.Domain.Entities.Core.Table entity)
    {
        return new TTableInfo
        {
            TableId = entity.TableId,
            TableNumber = (entity.TableNumber) ?? 0,
            Capacity = (entity.Capacity) ?? 0,
            Status = (TTableStatus)entity.Status,
            Zone = entity.Zone ?? string.Empty,
        };
    }
}
