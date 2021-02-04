using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBusProducer
{
    public interface IProducer
    {
        Task SendMessagesAsync(IList<string> messages);
    }
}
