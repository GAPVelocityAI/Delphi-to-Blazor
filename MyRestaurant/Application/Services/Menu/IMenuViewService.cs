using MyRestaurant.Application.DTOs.Menu;

namespace MyRestaurant.Application.Services.Menu;

public interface IMenuViewService
{
    // Page-facing name for the legacy-shaped operation below.
    Task<List<TMenuItemInfo>> GetMenuItemsAsync(CancellationToken ct = default);
    // Page-facing name for the legacy-shaped operation below.
    Task DeleteMenuItemAsync(int id, CancellationToken ct = default);
    // Page-facing name for the legacy-shaped operation below.
    Task AddMenuItemAsync(TMenuItemInfo item, CancellationToken ct = default);
    // Page-facing name for the legacy-shaped operation below.
    Task UpdateMenuItemAsync(TMenuItemInfo item, CancellationToken ct = default);

    Task<List<TMenuItemInfoDto>> LoadMenuItemsAsync(MenuViewStateDto state, CancellationToken ct = default);

    List<TMenuItemInfoDto> LoadMenuItems(List<TMenuItemInfoDto> allItems, string category);

    Task FilterByCategoryAsync(string category, MenuViewStateDto state, CancellationToken ct = default);

    Task FilterByCategory(string category, MenuViewStateDto state, CancellationToken ct = default);

    Task<MenuViewStateDto> BtnEditClickAsync(MenuViewStateDto state, int selectedItemId, CancellationToken ct = default);

    Task BtnDeleteClickAsync(MenuViewStateDto state, int selectedItemId, CancellationToken ct = default);

    Task BtnSaveClickAsync(MenuViewStateDto state, int selectedItemId, CancellationToken ct = default);
}
