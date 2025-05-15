namespace Taxually.TechnicalTest;

public interface ITaxuallyQueueClient
{
    Task EnqueueAsync<TPayload>(string queueName, TPayload payload);
}

public class TaxuallyQueueClient : ITaxuallyQueueClient
{
    public async Task EnqueueAsync<TPayload>(string queueName, TPayload payload)
    {
        // Code to send to message queue removed for brevity
        // Simulate run time
        await Task.Delay(1000);
    }
}
