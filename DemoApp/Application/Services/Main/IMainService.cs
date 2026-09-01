using MyRestaurant.Pages.Tables;

namespace DemoApp.Application.Services.Main;

public interface IMainService
{
    /// <summary>
    /// Initializes the main layout status information.
    /// Equivalent to legacy FormCreate: sets status text and formatted date.
    /// </summary>
    Task<MainStatusDto> InitializeAsync(CancellationToken ct = default);

    /// <summary>
    /// Returns all navigation targets grouped by category (Restaurant, Administration).
    /// Each entry corresponds to a legacy sidebar button and its modal form target.
    /// </summary>
    Task<List<NavigationTarget>> GetNavigationTargetsAsync(CancellationToken ct = default);

    /// <summary>
    /// Resolves the route for a given module name (e.g. "Tables" -> "/tables").
    /// Equivalent to legacy btnXxxClick handlers that opened modal forms.
    /// </summary>
    Task<string> GetRouteForModuleAsync(string moduleName, CancellationToken ct = default);
}
