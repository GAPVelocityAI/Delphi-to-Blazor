
namespace MyAdmin.Application.Services.Assets;

public interface IAssetsService
{
    Task<List<TAssetInfoDto>> LoadAssetsAsync(AssetsStateDto state, CancellationToken ct = default);

    Task BtnAddClickAsync(AssetsStateDto state, TObject Sender, CancellationToken ct = default);

    Task<AssetsStateDto> BtnEditClickAsync(AssetsStateDto state, TObject Sender, CancellationToken ct = default);

    Task BtnDeleteClickAsync(AssetsStateDto state, TObject Sender, CancellationToken ct = default);

    Task BtnSaveClickAsync(AssetsStateDto state, TObject Sender, CancellationToken ct = default);

    Task LoadAssets(AssetsStateDto state, CancellationToken ct = default);
}
