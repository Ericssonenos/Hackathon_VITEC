using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace c153201_Hackathon_VITEC.Models
{
    public  class AppEventHub
    {
        public static  async Task SendingJsonToEventHubAsync(RetornoSimulacao envelope)
        {

            // Serializar o envelope JSON em uma string
            string jsonEnvelope = JsonConvert.SerializeObject(envelope);

            // Gravar o envelope JSON no Event Hub
            string connectionString = "Endpoint=sb://eventhack.servicebus.windows.net/;SharedAccessKeyName=hack;SharedAccessKey=HeHeVaVqyVkntO2FnjQcs2Ilh/4MUDo4y+AEhKp8z+g=;EntityPath=simulacoes";
            

            await using (var producerClient = new EventHubProducerClient(connectionString))
            {
                using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(jsonEnvelope)));

                await producerClient.SendAsync(eventBatch);
            }
        }
    }
}
