using FakeItEasy;
using Taxually.TechnicalTest;
using Taxually.TechnicalTest.Data;
using Taxually.TechnicalTest.Services;

namespace Taxually.Tests.Services;

public class CountryVatRegistrationServiceGermanyTest
{
    private readonly ITaxuallyQueueClient _taxuallyQueueClient;
    private readonly ICountryVatRegistrationService _serviceUnderTest;

    public CountryVatRegistrationServiceGermanyTest()
    {
        _taxuallyQueueClient = A.Fake<ITaxuallyQueueClient>();
        _serviceUnderTest = new GermanyRegistrationService(_taxuallyQueueClient);
    }

    [Fact]
    public async Task RegisterCompanyForCountry_EnqueuesXmlWithCorrectQueueValue()
    {
        // Arrange
        var request = new VatRegistrationRequest
        {
            CompanyName = "Test GmbH",
            CompanyId = "DE123",
            Country = CountryCodes.GERMANY
        };

        string expectedXml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<VatRegistrationRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <CompanyName>Test GmbH</CompanyName>\r\n  <CompanyId>DE123</CompanyId>\r\n  <Country>GE</Country>\r\n</VatRegistrationRequest>";

        // Act
        await _serviceUnderTest.RegisterCompanyForCountry(request);

        // Assert
        A.CallTo(() => _taxuallyQueueClient.EnqueueAsync("vat-registration-xml", expectedXml)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void CountryCode_ReturnsGermanyCode()
    {
        // Act
        var code = _serviceUnderTest.CountryCode;

        // Assert
        Assert.Equal(CountryCodes.GERMANY, code);
    }
}
