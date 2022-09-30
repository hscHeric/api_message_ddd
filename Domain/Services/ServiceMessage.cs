using Domain.Interfaces;
using Domain.Interfaces.Interfaces.Services;

namespace Domain.Services
{
    public class ServiceMessage : IServiceMessage
    {
        private readonly IMessage _Imessage;

        public ServiceMessage(IMessage imessage)
        {
            _Imessage = imessage;
        }
    }
}
