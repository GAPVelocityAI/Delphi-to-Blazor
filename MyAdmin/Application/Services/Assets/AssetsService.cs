using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyAdmin.Infrastructure.Data;

namespace MyAdmin.Application.Services.Assets;

public class AssetsService : IAssetsService
{
    private readonly IDbContextFactory<MyAdminDbContext> _dbFactory;
    private readonly ILogger<AssetsService> _logger;

    public AssetsService(
        IDbContextFactory<MyAdminDbContext> dbFactory,
        ILogger<AssetsService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Ported from legacy LoadAssets().
    /// Fetches all assets from the database ordered by AssetName,
    /// maps each to a TAssetInfoDto, and computes TotalValue / TotalDepreciated
    /// summary fields on the state DTO (mirroring lblTotalValue and lblTotalDepreciated captions).
    /// </summary>
    public async Task<List<TAssetInfoDto>> LoadAssetsAsync(AssetsStateDto state, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Legacy: Assets := FAdminBL.GetAssets  (ordered by AssetName)
        var entities = await db.Assets
            .AsNoTracking()
            .OrderBy(a => a.AssetName)
            .ToListAsync(ct);

        // Legacy: map each asset to grid cells, accumulate TotalValue and TotalDepreciated
        decimal totalValue = 0m;
        decimal totalDepreciated = 0m;

        var result = new List<TAssetInfoDto>(entities.Count);

        foreach (var e in entities)
        {
            var dto = MapToDto(e);
            result.Add(dto);

            totalValue += dto.Value;
            totalDepreciated += dto.DepreciatedValue;
        }

        // Legacy: lblTotalValue.Caption := 'Total Value: ' + CurrToStrF(TotalValue, ffCurrency, 2);
        // Legacy: lblTotalDepreciated.Caption := 'Total Depreciated: ' + CurrToStrF(TotalDepreciated, ffCurrency, 2);
        state.TotalValue = totalValue;
        state.TotalDepreciated = totalDepreciated;

        _logger.LogDebug("Loaded {Count} assets. TotalValue={TotalValue}, TotalDepreciated={TotalDepreciated}",
            result.Count,
            totalValue.ToString("C2", CultureInfo.InvariantCulture),
            totalDepreciated.ToString("C2", CultureInfo.InvariantCulture));

        return result;
    }

    /// <summary>
    /// Ported from legacy LoadAssets procedure.
    /// Convenience method that delegates to LoadAssetsAsync and updates
    /// the state's asset list in place, mirroring the legacy void procedure
    /// that refreshed the grid and summary labels.
    /// </summary>
    public async Task LoadAssets(AssetsStateDto state, CancellationToken ct = default)
    {
        // Legacy: procedure TfrmAssets.LoadAssets;
        //   Assets := FAdminBL.GetAssets;
        //   ... populate grid and totals ...
        await LoadAssetsAsync(state, ct);
    }

    /// <summary>
    /// Ported from legacy btnAddClick.
    /// Sets state to "adding" mode: clears all edit fields, sets FIsAdding = true,
    /// and makes the edit panel visible (via state flags the page reads).
    /// </summary>
    public async Task BtnAddClickAsync(AssetsStateDto state, TObject Sender, CancellationToken ct = default)
    {
        // Legacy: FIsAdding := True;
        state.FIsAdding = true;

        // Legacy: edtAssetName.Text := '';
        state.EdtAssetNameText = string.Empty;

        // Legacy: cmbAssetCategory.ItemIndex := -1;  (no selection)
        state.CmbAssetCategoryText = string.Empty;

        // Legacy: edtPurchaseDate.Text := '';
        state.EdtPurchaseDateText = string.Empty;

        // Legacy: edtValue.Text := '';
        state.EdtValueText = string.Empty;

        // Legacy: edtDepreciated.Text := '';
        state.EdtDepreciatedText = string.Empty;

        // Legacy: cmbAssetStatus.ItemIndex := -1;  (no selection)
        state.CmbAssetStatusText = string.Empty;

        // Legacy: pnlEdit.Visible := True;
        // FSelectedId is reset to 0 to avoid stale references.
        state.FSelectedId = 0;

        await Task.CompletedTask;
    }

    /// <summary>
    /// Ported from legacy btnEditClick.
    /// Populates the edit form fields from the selected asset (identified by FSelectedId on state).
    /// The page sets FSelectedId before calling this method.
    /// Returns the updated state with edit fields populated.
    /// </summary>
    public async Task<AssetsStateDto> BtnEditClickAsync(AssetsStateDto state, TObject Sender, CancellationToken ct = default)
    {
        // Legacy: Row := grdAssets.Row;
        //         if (Row < 1) or (grdAssets.Cells[0, Row] = '') then Exit;
        if (state.FSelectedId <= 0)
        {
            return state;
        }

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var entity = await db.Assets
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AssetId == state.FSelectedId, ct);

        if (entity == null)
        {
            _logger.LogWarning("Asset with Id {AssetId} not found for editing", state.FSelectedId);
            return state;
        }

        // Legacy: FIsAdding := False;
        state.FIsAdding = false;

        // Legacy: edtAssetName.Text := grdAssets.Cells[1, Row];
        state.EdtAssetNameText = entity.AssetName ?? string.Empty;

        // Legacy: cmbAssetCategory.Text := grdAssets.Cells[2, Row];
        state.CmbAssetCategoryText = entity.Category ?? string.Empty;

        // Legacy: edtPurchaseDate.Text := grdAssets.Cells[3, Row];
        state.EdtPurchaseDateText = (entity.PurchaseDate ?? default).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        // Legacy: edtValue.Text := StringReplace(StringReplace(grdAssets.Cells[4, Row], '$', '', ...), ',', '', ...);
        state.EdtValueText = (entity.Value ?? default).ToString("F2", CultureInfo.InvariantCulture);

        // Legacy: edtDepreciated.Text := StringReplace(StringReplace(grdAssets.Cells[5, Row], '$', '', ...), ',', '', ...);
        state.EdtDepreciatedText = (entity.DepreciatedValue ?? default).ToString("F2", CultureInfo.InvariantCulture);

        // Legacy: cmbAssetStatus.Text := grdAssets.Cells[6, Row];
        state.CmbAssetStatusText = entity.Status ?? string.Empty;

        // Legacy: pnlEdit.Visible := True;

        return state;
    }

    /// <summary>
    /// Ported from legacy btnDeleteClick.
    /// Deletes the asset identified by state.FSelectedId from the database.
    /// The page is responsible for the confirmation dialog before calling this.
    /// </summary>
    public async Task BtnDeleteClickAsync(AssetsStateDto state, TObject Sender, CancellationToken ct = default)
    {
        // Legacy: Row := grdAssets.Row;
        //         if (Row < 1) or (grdAssets.Cells[0, Row] = '') then Exit;
        //         Id := StrToIntDef(grdAssets.Cells[0, Row], 0);
        if (state.FSelectedId <= 0)
        {
            return;
        }

        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Legacy: FAdminBL.DeleteAsset(Id);
        var entity = await db.Assets
            .FirstOrDefaultAsync(a => a.AssetId == state.FSelectedId, ct);

        if (entity == null)
        {
            _logger.LogWarning("Asset with Id {AssetId} not found for deletion", state.FSelectedId);
            return;
        }

        db.Assets.Remove(entity);
        await db.SaveChangesAsync(ct);

        _logger.LogInformation("Deleted asset {AssetId} ({AssetName})", entity.AssetId, entity.AssetName);

        // Legacy: LoadAssets;
        // The page calls LoadAssetsAsync after this method returns to refresh the grid.
    }

    /// <summary>
    /// Ported from legacy btnSaveClick.
    /// Either adds a new asset or updates an existing one based on state.FIsAdding.
    /// Reads form field values from the state DTO, persists to database.
    /// </summary>
    public async Task BtnSaveClickAsync(AssetsStateDto state, TObject Sender, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Legacy: Asset.AssetName := edtAssetName.Text;
        var assetName = state.EdtAssetNameText ?? string.Empty;

        // Legacy: Asset.Category := cmbAssetCategory.Text;
        var category = state.CmbAssetCategoryText ?? string.Empty;

        // Legacy: Asset.PurchaseDate := StrToDateDef(edtPurchaseDate.Text, Now);
        DateTime purchaseDate;
        if (!DateTime.TryParse(state.EdtPurchaseDateText, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out purchaseDate))
        {
            purchaseDate = DateTime.UtcNow;
        }

        // Legacy: Asset.Value := StrToCurrDef(StringReplace(edtValue.Text, '$', '', ...), 0);
        var valueText = (state.EdtValueText ?? string.Empty)
            .Replace("$", string.Empty, StringComparison.Ordinal)
            .Replace(",", string.Empty, StringComparison.Ordinal);
        if (!decimal.TryParse(valueText, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
        {
            value = 0m;
        }

        // Legacy: Asset.DepreciatedValue := StrToCurrDef(StringReplace(edtDepreciated.Text, '$', '', ...), 0);
        var depreciatedText = (state.EdtDepreciatedText ?? string.Empty)
            .Replace("$", string.Empty, StringComparison.Ordinal)
            .Replace(",", string.Empty, StringComparison.Ordinal);
        if (!decimal.TryParse(depreciatedText, NumberStyles.Any, CultureInfo.InvariantCulture, out var depreciatedValue))
        {
            depreciatedValue = 0m;
        }

        // Legacy: Asset.Status := cmbAssetStatus.Text;
        var status = state.CmbAssetStatusText ?? string.Empty;

        if (state.FIsAdding)
        {
            // Legacy: FAdminBL.AddAsset(Asset)
            // The database assigns the AssetId via identity column — no manual MAX+1.
            var newAsset = new global::MyAdmin.Domain.Entities.Core.Asset
            {
                AssetName = assetName,
                Category = category,
                PurchaseDate = purchaseDate,
                Value = value,
                DepreciatedValue = depreciatedValue,
                Status = status
            };

            db.Assets.Add(newAsset);
            await db.SaveChangesAsync(ct);

            _logger.LogInformation("Added new asset {AssetName} with Id {AssetId}", newAsset.AssetName, newAsset.AssetId);
        }
        else
        {
            // Legacy: Asset.AssetId := FSelectedId;
            //         FAdminBL.UpdateAsset(Asset);
            var existing = await db.Assets
                .FirstOrDefaultAsync(a => a.AssetId == state.FSelectedId, ct);

            if (existing == null)
            {
                _logger.LogWarning("Asset with Id {AssetId} not found for update", state.FSelectedId);
                return;
            }

            existing.AssetName = assetName;
            existing.Category = category;
            existing.PurchaseDate = purchaseDate;
            existing.Value = value;
            existing.DepreciatedValue = depreciatedValue;
            existing.Status = status;

            await db.SaveChangesAsync(ct);

            _logger.LogInformation("Updated asset {AssetId} ({AssetName})", existing.AssetId, existing.AssetName);
        }

        // Legacy: pnlEdit.Visible := False;
        // Reset edit state so the page hides the edit panel.
        state.FIsAdding = false;
        state.FSelectedId = 0;
        state.EdtAssetNameText = string.Empty;
        state.CmbAssetCategoryText = string.Empty;
        state.EdtPurchaseDateText = string.Empty;
        state.EdtValueText = string.Empty;
        state.EdtDepreciatedText = string.Empty;
        state.CmbAssetStatusText = string.Empty;

        // Legacy: LoadAssets;
        // The page calls LoadAssetsAsync after this method returns to refresh the grid.
    }

    /// <summary>
    /// Maps an Asset entity to a TAssetInfoDto.
    /// Coalesces all nullable string properties to string.Empty.
    /// </summary>
    private static TAssetInfoDto MapToDto(global::MyAdmin.Domain.Entities.Core.Asset entity)
    {
        return new TAssetInfoDto
        {
            AssetId = entity.AssetId,
            AssetName = entity.AssetName ?? string.Empty,
            Category = entity.Category ?? string.Empty,
            PurchaseDate = (entity.PurchaseDate) ?? default,
            Value = (entity.Value) ?? 0m,
            DepreciatedValue = (entity.DepreciatedValue) ?? 0m,
            Status = entity.Status ?? string.Empty
        };
    }
}
