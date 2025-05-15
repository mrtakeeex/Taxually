namespace Taxually.TechnicalTest.Services;

/// <summary>
/// Interface for registering company VAT
/// </summary>
public interface IVatRegistrationService
{
    Task RegisterCompany(VatRegistrationRequest request);
}

/// <summary>
/// Service for registering company VAT
/// </summary>
public class VatRegistrationService(IEnumerable<ICountryVatRegistrationService> countryVatRegistrationServices) : IVatRegistrationService
{
    public async Task RegisterCompany(VatRegistrationRequest request) 
    {
        if (IsValidRequest())
        {
            var countryService = countryVatRegistrationServices.Single(s => s.CountryCode.Equals(request.Country, StringComparison.InvariantCultureIgnoreCase));
            await countryService.RegisterCompanyForCountry(request);
            return;
        }
        
        throw new ArgumentException($"Validation failed for Company with ID '{request.CompanyId}': Invalid country value: {request.Country}");

        bool IsValidRequest()
        {
            return CountryCodes.Collection.Contains(request.Country?.ToUpperInvariant());
        }
    }
}
