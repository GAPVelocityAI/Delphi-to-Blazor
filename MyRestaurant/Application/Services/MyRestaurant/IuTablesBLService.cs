
namespace MyRestaurant.Application.Services.MyRestaurant;

public interface IuTablesBLService
{
    Task EnsureInitializedAsync(CancellationToken ct = default);
    Task<List<TTableInfo>> GetTablesAsync(CancellationToken ct = default);
    Task<List<TTableInfo>> GetAvailableTablesAsync(CancellationToken ct = default);
    Task<List<TTableInfo>> GetTablesByZoneAsync(string zone, CancellationToken ct = default);
    Task AddTableAsync(TTableInfo table, CancellationToken ct = default);
    Task UpdateTableAsync(TTableInfo table, CancellationToken ct = default);
    Task DeleteTableAsync(int tableId, CancellationToken ct = default);

    void EnsureInitialized();
    List<TTableInfo> GetTables();
    List<TTableInfo> GetAvailableTables();
    List<TTableInfo> GetTablesByZone(string zone);
    void AddTable(TTableInfo table);
    void UpdateTable(TTableInfo table);
    void DeleteTable(int tableId);
}
