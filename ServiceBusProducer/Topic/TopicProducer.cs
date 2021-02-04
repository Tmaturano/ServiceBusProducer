using Microsoft.Azure.ServiceBus;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusProducer.Topic
{
    public class TopicProducer : IProducer
    {
        private readonly Logger _logger;
        private readonly TopicClient _client;

        public TopicProducer(Logger logger, ExecutionParameters executionParameters)
        {
            _logger = logger;
            _client = new TopicClient(executionParameters.ConnectionString, executionParameters.Topic);
        }

        public async Task SendMessagesAsync(IList<string> messages)
        {
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
                await _client.CloseAsync();
                _logger.Information("Azure Service Bus closed!");
            }
        }
    }
}
