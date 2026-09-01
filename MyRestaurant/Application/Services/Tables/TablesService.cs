using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRestaurant.Infrastructure.Data;

namespace MyRestaurant.Application.Services.Tables;

public class TablesService : ITablesService
{
    private readonly IDbContextFactory<MyRestaurantDbContext> _dbContextFactory;
    private readonly ILogger<TablesService> _logger;

    // Status index mapping matching legacy cmbEditStatus:
    // 0 = Available, 1 = Occupied, 2 = Reserved, 3 = Closed
    private static readonly string[] StatusNames = { "Available", "Occupied", "Reserved", "Closed" };

    public TablesService(
        IDbContextFactory<MyRestaurantDbContext> dbContextFactory,
        ILogger<TablesService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    /// <summary>
    /// Loads all tables from the database, optionally filtering to available only.
    /// Corresponds to legacy LoadTables + GetTables/GetAvailableTables.
    /// Legacy: LoadTables(const ATables: TArray&lt;TTableInfo&gt;)
    /// Legacy: btnRefreshClick, btnFilterAvailableClick, btnShowAllClick
    /// </summary>
    public async Task<List<TTableInfoDto>> LoadTablesAsync(bool availableOnly = false, CancellationToken ct = default)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        IQueryable<global::MyRestaurant.Domain.Entities.Core.Table> query = db.Tables.AsNoTracking();

        if (availableOnly)
        {
            // Legacy: GetAvailableTables filters Status = tsAvailable (index 0)
            query = query.Where(t => t.Status == TTableStatus.Available);
        }

        // Legacy: SQL_GET_TABLES orders by Zone, TableNumber
        query = query.OrderBy(t => t.Zone).ThenBy(t => t.TableNumber);

        var entities = await query.ToListAsync(ct);

        var result = new List<TTableInfoDto>(entities.Count);
        foreach (var entity in entities)
        {
            result.Add(MapToDto(entity));
        }

        return result;
    }

    /// <summary>
    /// Loads tables from an already-provided collection (pass-through mapping).
    /// Corresponds to legacy LoadTables(const ATables: TArray&lt;TTableInfo&gt;)
    /// which accepted a pre-filtered array and rendered it to the grid.
    /// </summary>
    public List<TTableInfoDto> LoadTables(IEnumerable<TTableInfoDto> tables)
    {
        // Legacy: LoadTables simply rendered whatever array was passed in.
        // In the Blazor world, this is a pass-through since the DTOs
        // are already in display-ready form, but we preserve the method
        // for parity with the legacy pattern.
        return new List<TTableInfoDto>(tables);
    }

    /// <summary>
    /// Populates the edit state from a selected table row for editing.
    /// Legacy: btnEditClick(Sender: TObject)
    /// </summary>
    public async Task<TablesStateDto> BtnEditClickAsync(TablesStateDto state, int selectedTableId, CancellationToken ct = default)
    {
        if (selectedTableId <= 0)
        {
            return state;
        }

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);
        var table = await db.Tables.AsNoTracking()
            .FirstOrDefaultAsync(t => t.TableId == selectedTableId, ct);

        if (table == null)
        {
            _logger.LogWarning("Table with ID {TableId} not found for editing.", selectedTableId);
            return state;
        }

        // Legacy: FSelectedId := StrToIntDef(grdTables.Cells[0, Row], 0);
        state.FSelectedId = table.TableId;

        // Legacy: edtNumber.Text := grdTables.Cells[1, Row];
        state.EdtNumberText = (table.TableNumber ?? default).ToString(CultureInfo.InvariantCulture);

        // Legacy: edtCapacity.Text := grdTables.Cells[2, Row];
        state.EdtCapacityText = (table.Capacity ?? default).ToString(CultureInfo.InvariantCulture);

        // Legacy: maps status string to cmbEditStatus.ItemIndex
        // Available=0, Occupied=1, Reserved=2, Closed=3
        state.StatusIndex = (int)table.Status;

        // Legacy: cmbEditZone.ItemIndex := cmbEditZone.Items.IndexOf(grdTables.Cells[4, Row]);
        state.Zone = table.Zone ?? string.Empty;

        // Legacy: FIsAdding := False;
        state.FIsAdding = false;

        return state;
    }

    /// <summary>
    /// Deletes a table from the database.
    /// Legacy: btnDeleteClick(Sender: TObject)
    /// Confirmation dialog is handled by the caller (Blazor page).
    /// </summary>
    public async Task<TablesStateDto> BtnDeleteClickAsync(TablesStateDto state, int selectedTableId, CancellationToken ct = default)
    {
        if (selectedTableId <= 0)
        {
            return state;
        }

        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        // Legacy: FTablesBL.DeleteTable(Id);
        var table = await db.Tables.FirstOrDefaultAsync(t => t.TableId == selectedTableId, ct);
        if (table != null)
        {
            db.Tables.Remove(table);
            await db.SaveChangesAsync(ct);
            _logger.LogInformation("Deleted table with ID {TableId}.", selectedTableId);
        }
        else
        {
            _logger.LogWarning("Table with ID {TableId} not found for deletion.", selectedTableId);
        }

        // Legacy: after delete, state resets
        state.FSelectedId = 0;

        return state;
    }

    /// <summary>
    /// Saves (adds or updates) a table based on the current edit state.
    /// Legacy: btnSaveClick(Sender: TObject)
    /// StatusIndex and Zone are read from state.StatusIndex and state.Zone.
    /// </summary>
    public async Task<TablesStateDto> BtnSaveClickAsync(TablesStateDto state, CancellationToken ct = default)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(ct);

        // Legacy: Table.TableNumber := StrToIntDef(edtNumber.Text, 0);
        int tableNumber = 0;
        if (!string.IsNullOrWhiteSpace(state.EdtNumberText))
        {
            int.TryParse(state.EdtNumberText, NumberStyles.Integer, CultureInfo.InvariantCulture, out tableNumber);
        }

        // Legacy: Table.Capacity := StrToIntDef(edtCapacity.Text, 0);
        int capacity = 0;
        if (!string.IsNullOrWhiteSpace(state.EdtCapacityText))
        {
            int.TryParse(state.EdtCapacityText, NumberStyles.Integer, CultureInfo.InvariantCulture, out capacity);
        }

        // Legacy: Table.Status := TTableStatus(cmbEditStatus.ItemIndex);
        var status = Enum.IsDefined(typeof(TTableStatus), state.StatusIndex)
            ? (TTableStatus)state.StatusIndex
            : TTableStatus.Available;

        // Legacy: Table.Zone := cmbEditZone.Text;
        string zoneValue = string.IsNullOrWhiteSpace(state.Zone) ? "Main Hall" : state.Zone;

        if (state.FIsAdding)
        {
            // Legacy: FTablesBL.AddTable(Table) — DB assigns ID via identity column
            var newTable = new global::MyRestaurant.Domain.Entities.Core.Table
            {
                TableNumber = tableNumber,
                Capacity = capacity,
                Status = status,
                Zone = zoneValue
            };

            db.Tables.Add(newTable);
            await db.SaveChangesAsync(ct);
            _logger.LogInformation("Added new table with ID {TableId}.", newTable.TableId);
        }
        else
        {
            // Legacy: Table.TableId := FSelectedId; FTablesBL.UpdateTable(Table);
            var existing = await db.Tables.FirstOrDefaultAsync(t => t.TableId == state.FSelectedId, ct);
            if (existing != null)
            {
                existing.TableNumber = tableNumber;
                existing.Capacity = capacity;
                existing.Status = status;
                existing.Zone = zoneValue;

                await db.SaveChangesAsync(ct);
                _logger.LogInformation("Updated table with ID {TableId}.", state.FSelectedId);
            }
            else
            {
                _logger.LogWarning("Table with ID {TableId} not found for update.", state.FSelectedId);
            }
        }

        // Legacy: pnlEdit.Visible := False; — UI concern, reset state
        state.FIsAdding = false;
        state.FSelectedId = 0;
        state.EdtNumberText = string.Empty;
        state.EdtCapacityText = string.Empty;

        return state;
    }

    /// <summary>
    /// Maps a Table entity to a TTableInfoDto.
    /// Legacy: the grid population loop in LoadTables.
    /// </summary>
    private static TTableInfoDto MapToDto(global::MyRestaurant.Domain.Entities.Core.Table entity)
    {
        int statusIndex = (int)entity.Status;
        string statusText = statusIndex >= 0 && statusIndex < StatusNames.Length
            ? StatusNames[statusIndex]
            : "Available";

        return new TTableInfoDto
        {
            TableId = entity.TableId,
            TableNumber = (entity.TableNumber) ?? 0,
            Capacity = (entity.Capacity) ?? 0,
            StatusIndex = statusIndex,
            StatusText = statusText,
            Zone = entity.Zone ?? string.Empty
        };
    }
}
