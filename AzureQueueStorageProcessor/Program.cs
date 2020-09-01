using System;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Threading.Tasks;

namespace AzureQueueStorageProcessor
{
    class Program
    {
        private const string storageConnectionString = "";
        private const string queueName = "myqueues";
        static async Task Main(string[] args)
        {
            QueueClient client = new QueueClient(storageConnectionString, queueName);        
            await client.CreateAsync();

            await Console.Out.WriteLineAsync($"Account Uri:\t{client.Uri}");
            int batchSize = 10;
            TimeSpan visibilityTimeout = TimeSpan.FromSeconds(2.5d);

            Response<QueueMessage[]> messages = await client.ReceiveMessagesAsync(batchSize, visibilityTimeout);
            foreach(QueueMessage message in messages?.Value)
            {
                Console.WriteLine($"[{message.MessageId}]\t{message.MessageText}");
            }
        }
    }
}
