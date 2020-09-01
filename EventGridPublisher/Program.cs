using System;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventGridPublisher
{
    class Program
    {
        private const string topicEndpoint = "";
        private const string topicKey = "";
        static async Task Main(string[] args)
        {
            TopicCredentials credentials = new TopicCredentials(topicKey);
            EventGridClient client = new EventGridClient(credentials);

            List<EventGridEvent> events = new List<EventGridEvent>();

            var firstPerson = new 
            {
                FullName = "Alba Sutton",
                Address = "4567 Pine Avenue, Edison, WA 97202"
            };

            EventGridEvent firstEvent = new EventGridEvent
            {
                Id = Guid.NewGuid().ToString(),
                EventType = "Employees.Registration.New",
                EventTime = DateTime.Now,
                Subject = $"New Employee : {firstPerson.FullName}",
                Data = firstPerson.ToString(),
                DataVersion = "1.0.0"
            };

            events.Add(firstEvent);

            string topicHostName = new Uri(topicEndpoint).Host;
            await client.PublishEventsAsync(topicHostName, events);

            await Console.Out.WriteLineAsync("Events published");
        }
    }
}
