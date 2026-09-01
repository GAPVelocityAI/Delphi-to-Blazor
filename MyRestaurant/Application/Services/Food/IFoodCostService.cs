using MyRestaurant.Application.DTOs.Food;

namespace MyRestaurant.Application.Services.Food;

public interface IFoodCostService
{
    Task<List<TFoodCostInfoDto>> LoadFoodCostsAsync(FoodCostStateDto state, CancellationToken ct = default);
    List<TFoodCostInfoDto> LoadFoodCosts(FoodCostStateDto state);
    Task<FoodCostStateDto> BtnEditClickAsync(FoodCostStateDto state, int selectedId, CancellationToken ct = default);
    Task BtnDeleteClickAsync(FoodCostStateDto state, int selectedId, CancellationToken ct = default);
    Task BtnSaveClickAsync(FoodCostStateDto state, int selectedId, CancellationToken ct = default);
}
