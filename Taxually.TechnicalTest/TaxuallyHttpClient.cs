namespace Taxually.TechnicalTest;
public interface ITaxuallyHttpClient
{
    Task PostAsync<TRequest>(string url, TRequest request);
}

public class TaxuallyHttpClient : ITaxuallyHttpClient
{
    public async Task PostAsync<TRequest>(string url, TRequest request)
    {
        // Actual HTTP call removed for purposes of this exercise
        // Simulate run time
        await Task.Delay(1000);
    }
}
