namespace Taxually.TechnicalTest.Data;

public class VatRegistrationRequest
{
    public string CompanyName { get; init; } = string.Empty;
    public string CompanyId { get; init; } = string.Empty;
    public string? Country { get; init; }
}

public static class CountryCodes
{
    public const string GERMANY = "GE";
    public const string FRANCE = "FR";
    public const string GREAT_BRITAIN = "GB";

    public static readonly IEnumerable<string> Collection = [GERMANY, FRANCE, GREAT_BRITAIN];
}
