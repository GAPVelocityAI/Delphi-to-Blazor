namespace MyAdmin.Application.Services.Personnel;

public interface IPersonnelService
{
    Task<List<TEmployeeInfoDto>> LoadEmployeesAsync(PersonnelStateDto state, bool AActiveOnly, CancellationToken ct = default);
    Task LoadEmployees(PersonnelStateDto state, bool AActiveOnly, CancellationToken ct = default);
    Task BtnAddClickAsync(PersonnelStateDto state, object? Sender, CancellationToken ct = default);
    Task<PersonnelStateDto> BtnEditClickAsync(PersonnelStateDto state, object? Sender, CancellationToken ct = default);
    Task BtnDeleteClickAsync(PersonnelStateDto state, object? Sender, CancellationToken ct = default);
    Task BtnSaveClickAsync(PersonnelStateDto state, object? Sender, CancellationToken ct = default);
}