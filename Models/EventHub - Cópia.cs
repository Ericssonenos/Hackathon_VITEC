using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
namespace c153201_Hackathon_VITEC.Models
{
    public class EventHub2
    {
   
        // Serializar o envelope JSON em uma string
        string jsonEnvelope = JsonConvert.SerializeObject(value: RetornoSimulacao);

        // Gravar o envelope JSON no Event Hub
        string connectionString = "SuaConnectionString";
        string eventHubName = "NomeDoEventHub";

        await using (var producerClient = new EventHubProducerClient(connectionString, eventHubName))
    {
        using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
        eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(jsonEnvelope)));

        await producerClient.SendAsync(eventBatch);
}

    }
}
