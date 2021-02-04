using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using ServiceBusProducer.Queue;
using ServiceBusProducer.Topic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBusProducer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
                .CreateLogger();


            Console.WriteLine("Azure Service Bus topics (Producer) tests: ");
            Console.WriteLine("Type 1 to test Azure Service Bus + Queue");
            Console.WriteLine("Type 2 to test Azure Service Bus + Topic");


            var line = Console.ReadLine();

            int.TryParse(line, out var selectedOption);
            Console.WriteLine($"selected option: {selectedOption}");

            if (selectedOption != 1 && selectedOption != 2)
            {
                Console.WriteLine("Please, select the correct options and try again");
                return;
            }

            Console.WriteLine("Type the Azure Service Bus Connection String: ");
            var connectionString = Console.ReadLine();

            if (selectedOption == 1)
            {
                logger.Information("Testing the messages sending to an Azure Service Bus + Queue");

                Console.WriteLine("Type the Queue name to send the messages: ");
                var queueName = Console.ReadLine();

                logger.Information($"Queue = {queueName}");

                var queueProducer = new QueueProducer(logger, new Queue.ExecutionParameters
                {
                    ConnectionString = connectionString,
                    Queue = queueName
                });

                var messages = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    messages.Add($"Message from Client ({i})");
                }

                await queueProducer.SendMessagesAsync(messages);
            }
            else
            {
                logger.Information("Testing the messages sending to an Azure Service Bus + Topics");

                Console.WriteLine("Type the Topic name to send the messages: ");
                var topicName = Console.ReadLine();

                logger.Information($"Topic = {topicName}");

                var topicProducer = new TopicProducer(logger, new Topic.ExecutionParameters
                {
                    ConnectionString = connectionString,
                    Topic = topicName
                });

                var messages = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    messages.Add($"Message from Client ({i})");
                }

                await topicProducer.SendMessagesAsync(messages);
            }

        }
    }
}
