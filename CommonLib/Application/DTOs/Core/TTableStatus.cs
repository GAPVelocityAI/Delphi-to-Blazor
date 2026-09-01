namespace CommonLib.Application.DTOs.Core;

/// <summary>Ported from Delphi <c>TTableStatus</c> in uCommonTypes.pas.
/// Member prefixes (ts/os/pm) are dropped — C# scopes members by their enum type.</summary>
public enum TTableStatus
{
    Available,
    Occupied,
    Reserved,
    Closed
}
