using Taxually.TechnicalTest.Services;
using Taxually.TechnicalTest;
using FakeItEasy;
using Taxually.TechnicalTest.Data;
using System.Text;

namespace Taxually.Tests.Services;

public class CountryVatRegistrationServiceFranceTest
{
    private readonly ITaxuallyQueueClient _taxuallyQueueClient;
    private readonly ICountryVatRegistrationService _serviceUnderTest;

    public CountryVatRegistrationServiceFranceTest()
    {
        _taxuallyQueueClient = A.Fake<ITaxuallyQueueClient>();
        _serviceUnderTest = A.Fake<FranceRegistrationService>(x => x.WithArgumentsForConstructor(() => new(_taxuallyQueueClient)));
    }

    [Fact]
    public async Task RegisterCompanyForCountry_EnqueuesCsvWithCorrectQueueValue()
    {
        // Arrange
        var request = new VatRegistrationRequest
        {
            CompanyName = "Test GmbH",
            CompanyId = "DE123",
            Country = CountryCodes.FRANCE
        };

        var expectedCsv = Encoding.UTF8.GetBytes(new StringBuilder("CompanyName,CompanyId").AppendLine($"{request.CompanyName}{request.CompanyId}").ToString());

        // Act
        await _serviceUnderTest.RegisterCompanyForCountry(request);

        // Assert
        A.CallTo(() => _taxuallyQueueClient.EnqueueAsync("vat-registration-csv", expectedCsv)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void CountryCode_ReturnsFranceCode()
    {
        // Act
        var code = _serviceUnderTest.CountryCode;

        // Assert
        Assert.Equal(CountryCodes.FRANCE, code);
    }
}
