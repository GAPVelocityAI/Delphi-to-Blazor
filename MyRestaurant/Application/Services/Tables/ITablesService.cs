
namespace MyRestaurant.Application.Services.Tables;

public interface ITablesService
{
    Task<List<TTableInfoDto>> LoadTablesAsync(bool availableOnly = false, CancellationToken ct = default);
    List<TTableInfoDto> LoadTables(IEnumerable<TTableInfoDto> tables);
    Task<TablesStateDto> BtnEditClickAsync(TablesStateDto state, int selectedTableId, CancellationToken ct = default);
    Task<TablesStateDto> BtnDeleteClickAsync(TablesStateDto state, int selectedTableId, CancellationToken ct = default);
    Task<TablesStateDto> BtnSaveClickAsync(TablesStateDto state, CancellationToken ct = default);
}
