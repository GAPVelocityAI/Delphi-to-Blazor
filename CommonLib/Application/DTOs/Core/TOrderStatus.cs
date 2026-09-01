namespace CommonLib.Application.DTOs.Core;

/// <summary>Ported from Delphi <c>TOrderStatus</c> in uCommonTypes.pas.
/// Member prefixes (ts/os/pm) are dropped — C# scopes members by their enum type.</summary>
public enum TOrderStatus
{
    Pending,
    Preparing,
    Served,
    Paid,
    Cancelled
}
