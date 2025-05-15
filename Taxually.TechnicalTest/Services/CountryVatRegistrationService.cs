using System.Text;
using System.Xml.Serialization;

namespace Taxually.TechnicalTest.Services;

/// <summary>
/// Interface for applying target country VAT logic upon company registration
/// </summary>
public interface ICountryVatRegistrationService
{
    string CountryCode { get; }
    Task RegisterCompanyForCountry(VatRegistrationRequest request);
}

/// <summary>
/// Service for Germany. Requires an XML document to be uploaded to register for a VAT number
/// </summary>
/// <param name="taxuallyQueueClient"></param>
public class GermanyRegistrationService(ITaxuallyQueueClient taxuallyQueueClient) : ICountryVatRegistrationService
{
    public string CountryCode => CountryCodes.GERMANY;
    public async Task RegisterCompanyForCountry(VatRegistrationRequest request)
    {
        // Queue xml doc to be processed
        await taxuallyQueueClient.EnqueueAsync("vat-registration-xml", GetSerializedXml());

        string GetSerializedXml()
        {
            using var stringwriter = new StringWriter();
            new XmlSerializer(typeof(VatRegistrationRequest)).Serialize(stringwriter, request);
            return stringwriter.ToString();
        }
    }
}

/// <summary>
/// Service for France. Requires an excel spreadsheet to be uploaded to register for a VAT number
/// </summary>
/// <param name="taxuallyQueueClient"></param>
public class FranceRegistrationService(ITaxuallyQueueClient taxuallyQueueClient) : ICountryVatRegistrationService
{
    public string CountryCode => CountryCodes.FRANCE;
    public async Task RegisterCompanyForCountry(VatRegistrationRequest request)
    {
        // Queue file to be processed
        await taxuallyQueueClient.EnqueueAsync("vat-registration-csv", GetUTFEncodedCSV());

        byte[] GetUTFEncodedCSV() => Encoding.UTF8.GetBytes(new StringBuilder("CompanyName,CompanyId").AppendLine($"{request.CompanyName}{request.CompanyId}").ToString());
    }
}

/// <summary>
/// Service for Great-Britain. Has an API to register for a VAT number
/// </summary>
/// <param name="taxuallyHttpClient"></param>
public class GreatBritainRegistrationService(ITaxuallyHttpClient taxuallyHttpClient) : ICountryVatRegistrationService
{
    public string CountryCode => CountryCodes.GREAT_BRITAIN;
    public async Task RegisterCompanyForCountry(VatRegistrationRequest request) => await taxuallyHttpClient.PostAsync("https://api.uktax.gov.uk", request);
}
