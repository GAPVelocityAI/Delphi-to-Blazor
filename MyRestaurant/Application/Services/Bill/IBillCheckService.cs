
namespace MyRestaurant.Application.Services.Bill;

public interface IBillCheckService
{
    Task<List<TBillInfoDto>> LoadBillsAsync(BillCheckStateDto state, CancellationToken ct = default);
    Task<BillCheckStateDto> BtnEditClickAsync(BillCheckStateDto state, object? sender, CancellationToken ct = default);
    Task BtnDeleteClickAsync(BillCheckStateDto state, object? sender, CancellationToken ct = default);
    Task BtnSaveClickAsync(BillCheckStateDto state, object? sender, CancellationToken ct = default);
}
