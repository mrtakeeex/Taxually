using FakeItEasy;
using Taxually.TechnicalTest.Data;
using Taxually.TechnicalTest.Services;

namespace Taxually.Tests.Services;

public class VatRegistrationServiceTest
{
    private readonly IVatRegistrationService _serviceUnderTest;
    private readonly ICountryVatRegistrationService _germanService;
    private readonly ICountryVatRegistrationService _frenchService;

    public VatRegistrationServiceTest()
    {
        _germanService = A.Fake<ICountryVatRegistrationService>();
        _frenchService = A.Fake<ICountryVatRegistrationService>();

        A.CallTo(() => _germanService.CountryCode).Returns(CountryCodes.GERMANY);
        A.CallTo(() => _frenchService.CountryCode).Returns(CountryCodes.FRANCE);

        List<ICountryVatRegistrationService> services = [_germanService, _frenchService];

        _serviceUnderTest = A.Fake<VatRegistrationService>(x => x.WithArgumentsForConstructor(() => new(services))); 
    }

    [Fact]
    public async Task RegisterCompany_CallsGermanService_WhenCountryIsGermanyUppercase()
    {
        // Arrange
        var request = new VatRegistrationRequest
        {
            CompanyName = "Test GmbH",
            CompanyId = "DE123",
            Country = "GE"
        };

        // Act
        await _serviceUnderTest.RegisterCompany(request);

        // Assert
        A.CallTo(() => _germanService.RegisterCompanyForCountry(request)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _frenchService.RegisterCompanyForCountry(A<VatRegistrationRequest>.Ignored)).MustNotHaveHappened();
    }

    [Fact]
    public async Task RegisterCompany_CallsFrenchService_WhenCountryIsFranceLowercase()
    {
        // Arrange
        var request = new VatRegistrationRequest
        {
            CompanyName = "Test SARL",
            CompanyId = "FR123",
            Country = "fr"
        };

        // Act
        await _serviceUnderTest.RegisterCompany(request);

        // Assert
        A.CallTo(() => _frenchService.RegisterCompanyForCountry(request)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _germanService.RegisterCompanyForCountry(A<VatRegistrationRequest>.Ignored)).MustNotHaveHappened();
    }

    [Fact]
    public async Task RegisterCompany_ThrowsException_WhenCountryIsUnknown()
    {
        // Arrange
        var request = new VatRegistrationRequest
        {
            CompanyName = "Test Unknown",
            CompanyId = "XX123",
            Country = "XX"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _serviceUnderTest.RegisterCompany(request));
        Assert.Equal($"Validation failed for Company with ID '{request.CompanyId}': Invalid country value: {request.Country}", exception.Message);
    }
}