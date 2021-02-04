using Microsoft.Azure.ServiceBus;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusProducer.Queue
{
    public class QueueProducer : IProducer
    {
        private readonly Logger _logger;
        private readonly QueueClient _client;

        public QueueProducer(Logger logger, ExecutionParameters executionParameters)
        {
            _logger = logger;
            _client = new QueueClient(executionParameters.ConnectionString, executionParameters.Queue, 
                ReceiveMode.ReceiveAndDelete);
        }

        public async Task SendMessagesAsync(IList<string> messages)
        {
            QueueClient client = null;
            try
            {
                foreach (var message in messages)
                {
                    await _client.SendAsync(new Message(Encoding.UTF8.GetBytes(message)));
                    _logger.Information($"[Message sent]: {message}");
                }

                _logger.Information("Finished sending messages");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception: {ex.GetType().FullName} | " +
                             $"Message: {ex.Message}");
            }
            finally
            {
                if (client is not null)
                {
                    await client.CloseAsync();
                    _logger.Information("Azure Service Bus closed!");
                }

            }
        }
    }
}
