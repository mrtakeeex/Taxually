using Taxually.TechnicalTest.Services;
using Taxually.TechnicalTest;
using FakeItEasy;
using Taxually.TechnicalTest.Data;

namespace Taxually.Tests.Services;

public class CountryVatRegistrationServiceGreatBritainTest
{
    private readonly ITaxuallyHttpClient _taxuallyHttpClient;
    private readonly ICountryVatRegistrationService _serviceUnderTest;

    public CountryVatRegistrationServiceGreatBritainTest()
    {
        _taxuallyHttpClient = A.Fake<ITaxuallyHttpClient>();
        _serviceUnderTest = A.Fake<GreatBritainRegistrationService>(x => x.WithArgumentsForConstructor(() => new(_taxuallyHttpClient)));
    }

    [Fact]
    public async Task RegisterCompanyForCountry_PostAsyncWithCorrectUrlAndValue()
    {
        // Arrange
        var request = new VatRegistrationRequest
        {
            CompanyName = "Test GmbH",
            CompanyId = "DE123",
            Country = CountryCodes.FRANCE
        };

        // Act
        await _serviceUnderTest.RegisterCompanyForCountry(request);

        // Assert
        A.CallTo(() => _taxuallyHttpClient.PostAsync("https://api.uktax.gov.uk", request)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void CountryCode_ReturnsGreatBritainCode()
    {
        // Act
        var code = _serviceUnderTest.CountryCode;

        // Assert
        Assert.Equal(CountryCodes.GREAT_BRITAIN, code);
    }
}
